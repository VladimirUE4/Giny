using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class InteractiveUseWithParamRequestMessage : InteractiveUseRequestMessage  
    { 
        public new const ushort Id = 9161;
        public override ushort MessageId => Id;

        public int id;

        public InteractiveUseWithParamRequestMessage()
        {
        }
        public InteractiveUseWithParamRequestMessage(int id)
        {
            this.id = id;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)id);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            id = (int)reader.ReadInt();
        }


    }
}








