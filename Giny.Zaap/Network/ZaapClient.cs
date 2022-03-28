using Giny.Core;
using Giny.Core.IO;
using Giny.Core.Network;
using Giny.Core.Network.Messages;
using Giny.Zaap.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpClient = Giny.Core.Network.TcpClient;

namespace Giny.Zaap.Network
{
    public class ZaapClient : TcpClient
    {
        private int SequenceId
        {
            get;
            set;
        }
        private TProtocol TProtocol
        {
            get;
            set;
        }
        public ZaapClient(Socket socket) : base(socket)
        {
            this.TProtocol = new TProtocol();
        }
        public override void OnConnected()
        {
            throw new NotImplementedException();
        }

        public override void OnConnectionClosed()
        {
            Console.WriteLine("Client disconnected.");
        }

        public void Send(ZaapMessage message)
        {
            if (message.TMessage.TypeEnum == TMessageType.REPLY)
            {
                SequenceId++;
            }

            using (BigEndianWriter writer = new BigEndianWriter())
            {
                TProtocol.WriteMessageBegin(message.TMessage, writer);

                message.Serialize(TProtocol, writer);

                Socket.BeginSend(writer.Data, 0, writer.Data.Length, SocketFlags.None, OnSended, message);
            }
        }

        protected override void OnDataArrival(int dataSize)
        {
            ZaapMessage message = null;

            using (BigEndianReader reader = new BigEndianReader(Buffer))
            {
                TMessage tMessage = TProtocol.ReadMessageBegin(reader);

                switch (tMessage.Name)
                {
                    case "connect":
                        message = new ConnectArgs(tMessage);
                        break;
                    case "settings_get":
                        message = new SettingsGet(tMessage);
                        break;
                    case "userInfo_get":
                        message = new UserInfoGet(tMessage);
                        break;
                    case "auth_getGameToken":
                        message = new AuthGetGameToken(tMessage);
                        break;
                    default:
                        Logger.Write("Receive Unknown message : " + tMessage.Name, Channels.Warning);
                        return;
                }

                Logger.Write("Received : " + message.GetType().Name);

                message.Deserialize(TProtocol, reader);
            }

            MessagesHandler.Handle(this, message);
        }

        public override void OnDisconnected()
        {
            Console.WriteLine("Zaap client disconnected.");
        }

        public override void OnFailToConnect(Exception ex)
        {
            throw new NotImplementedException();
        }

        public override void OnSended(IAsyncResult result)
        {
            Console.WriteLine("Send : " + result.AsyncState.GetType().Name.ToString());
        }
    }
}
