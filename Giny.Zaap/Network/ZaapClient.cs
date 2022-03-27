using Giny.Core.IO;
using Giny.Core.Network;
using Giny.Core.Network.Messages;
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
            throw new NotImplementedException();
        }

        protected override void OnDataArrival(int dataSize)
        {
            using (BigEndianReader reader = new BigEndianReader(Buffer))
            {
                TProtocol.ReadMessageBegin(reader);

                 reader.ReadByte(); // TBinaryProtocol , read struct begin ZaapServiceImpl
                reader.ReadShort();
                var test = reader.ReadUTF7BitLength();
            }

          /*  var t = reader.ReadInt();

            var t1 = reader.ReadShort();


            var str1 = reader.ReadUTF();

            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            var str2 = reader.ReadUTF(); */


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
            throw new NotImplementedException();
        }
    }
}
