using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class BasicWhoIsRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 5985;
        public override ushort MessageId => Id;

        public bool verbose;
        public string search;

        public BasicWhoIsRequestMessage()
        {
        }
        public BasicWhoIsRequestMessage(bool verbose,string search)
        {
            this.verbose = verbose;
            this.search = search;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean((bool)verbose);
            writer.WriteUTF((string)search);
        }
        public override void Deserialize(IDataReader reader)
        {
            verbose = (bool)reader.ReadBoolean();
            search = (string)reader.ReadUTF();
        }


    }
}








