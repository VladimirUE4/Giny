using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyEntityUpdateLightMessage : PartyUpdateLightMessage  
    { 
        public new const ushort Id = 8730;
        public override ushort MessageId => Id;

        public byte indexId;

        public PartyEntityUpdateLightMessage()
        {
        }
        public PartyEntityUpdateLightMessage(byte indexId)
        {
            this.indexId = indexId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (indexId < 0)
            {
                throw new Exception("Forbidden value (" + indexId + ") on element indexId.");
            }

            writer.WriteByte((byte)indexId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            indexId = (byte)reader.ReadByte();
            if (indexId < 0)
            {
                throw new Exception("Forbidden value (" + indexId + ") on element of PartyEntityUpdateLightMessage.indexId.");
            }

        }


    }
}








