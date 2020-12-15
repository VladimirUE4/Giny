using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MountRenamedMessage : NetworkMessage  
    { 
        public new const ushort Id = 5372;
        public override ushort MessageId => Id;

        public int mountId;
        public string name;

        public MountRenamedMessage()
        {
        }
        public MountRenamedMessage(int mountId,string name)
        {
            this.mountId = mountId;
            this.name = name;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt((int)mountId);
            writer.WriteUTF((string)name);
        }
        public override void Deserialize(IDataReader reader)
        {
            mountId = (int)reader.ReadVarInt();
            name = (string)reader.ReadUTF();
        }


    }
}








