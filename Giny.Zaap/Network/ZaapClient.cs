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
        private TProtocol TProtocol
        {
            get;
            set;
        }
        private ConnectArgs ConnectArgs
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

        protected override void OnDataArrival(int dataSize)
        {
            using (BigEndianReader reader = new BigEndianReader(Buffer))
            {
                var message = TProtocol.ReadMessageBegin(reader);

                this.ConnectArgs = new ConnectArgs();
                ConnectArgs.Deserialize(TProtocol, reader);
                Console.WriteLine(ConnectArgs.ToString());
            }

            using (BigEndianWriter writer = new BigEndianWriter())
            {
                TMessage message = new TMessage()
                {
                    Name = "success",
                    SequenceId = 0,
                    Type = (int)TMessageType.REPLY,
                };

                TProtocol.WriteMessageBegin(message, writer);

                var result = new ConnectResult();
                result.Write(TProtocol, writer);

                Socket.BeginSend(writer.Data, 0, writer.Data.Length, SocketFlags.None, OnSended, result);
            }


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
            Console.WriteLine("Sended " + result.AsyncState.ToString());
        }
    }
}
