using Giny.Core.IO;
using Giny.Core.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Core.Network
{
    public abstract class Client
    {
        public const int BUFFER_LENGTH = 8192;

        private Socket m_socket;

        private byte[] m_buffer;

        private int m_bufferEndPosition;

        public IPEndPoint EndPoint
        {
            get
            {
                return m_socket.RemoteEndPoint as IPEndPoint;
            }
        }
        public string Ip
        {
            get
            {
                return EndPoint.Address.ToString();
            }
        }
        public bool Connected
        {
            get
            {
                return m_socket != null && m_socket.Connected;
            }
        }

        public Client()
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_buffer = new byte[BUFFER_LENGTH];
        }
        public Client(Socket socket)
        {
            this.m_buffer = new byte[BUFFER_LENGTH];
            this.m_socket = socket;
            BeginReceive();
        }

        public abstract void OnConnectionClosed();

        public abstract void OnConnected();

        public abstract void OnDisconnected();

        public abstract void OnMessageReceived(NetworkMessage message);

        public abstract void OnFailToConnect(Exception ex);

        private void OnDataArrival(int dataSize)
        {
            if (m_buffer.Max() == 0)
            {
                m_bufferEndPosition = dataSize;
            }
            else
            {
                m_bufferEndPosition += dataSize;

                if (m_bufferEndPosition > BUFFER_LENGTH)
                {
                    throw new Exception("Too large amount of data."); // todo copy in new buffer
                }
            }

            while (m_bufferEndPosition > 0)
            {
                BigEndianReader reader = new BigEndianReader(m_buffer);
                NetworkMessage message = ProtocolMessageManager.BuildMessage(reader);

                if (message != null)
                {
                    OnMessageReceived(message);
                }
                else
                {
                    Logger.Write("Received Unknown Data", MessageState.WARNING);
                    m_bufferEndPosition = 0;
                }

                var newBuffer = new byte[BUFFER_LENGTH];
                Array.Copy(m_buffer, reader.Position, newBuffer, 0, newBuffer.Length - reader.Position);

                m_buffer = newBuffer;

                m_bufferEndPosition -= (int)reader.Position;

                reader.Dispose();
            }


        }
        public void Send(NetworkMessage message)
        {
            if (m_socket != null && m_socket.Connected)
            {
                try
                {

                    using (var writer = new BigEndianWriter())
                    {
                        message.Pack(writer);
                        m_socket.BeginSend(writer.Data, 0, writer.Data.Length, SocketFlags.None, OnSended, message);
                    }
                }
                catch
                {
                    Logger.Write("Unable to send message to " + Ip + ".", MessageState.WARNING);
                    Disconnect();
                }
            }
            else
            {
                Logger.Write("Attempt was made to send data to disconnect socket.", MessageState.WARNING);
            }

        }
        public abstract void OnSended(IAsyncResult result);

        public void Connect(string host, int port)
        {
            m_socket?.BeginConnect(new IPEndPoint(IPAddress.Parse(host), port), new AsyncCallback(OnConnectionResulted), m_socket);
        }

        public void OnConnectionResulted(IAsyncResult result)
        {
            try
            {
                m_socket.EndConnect(result);
                BeginReceive();
                OnConnected();
            }
            catch (Exception ex)
            {
                OnFailToConnect(ex);
            }
        }
        private void BeginReceive()
        {
            try
            {
                m_socket?.BeginReceive(m_buffer, 0, m_buffer.Length, SocketFlags.None, OnReceived, null);
            }
            catch
            {
                Logger.Write("Unable to receive from " + Ip, MessageState.WARNING);
                Disconnect();
            }
        }
        public void OnReceived(IAsyncResult result)
        {
            if (m_socket == null)
            {
                return;
            }

            int size = 0;
            try
            {
                size = m_socket.EndReceive(result);

                if (size == 0)
                {
                    Dispose();
                    OnConnectionClosed();
                    return;
                }

            }
            catch (Exception ex)
            {
                Dispose();
                OnConnectionClosed();
                return;
            }

            OnDataArrival(size);
            BeginReceive();
        }
        public void Disconnect()
        {
            if (m_socket != null)
            {
                Dispose();
                OnDisconnected();
            }
        }

        private void Dispose()
        {
            m_socket?.Shutdown(SocketShutdown.Both);
            m_socket?.Close();
            m_socket?.Dispose();
            m_socket = null;

        }
    }
}
