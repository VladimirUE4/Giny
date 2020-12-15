using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PrismSettingsRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 9213;
        public override ushort MessageId => Id;

        public short subAreaId;
        public byte startDefenseTime;

        public PrismSettingsRequestMessage()
        {
        }
        public PrismSettingsRequestMessage(short subAreaId,byte startDefenseTime)
        {
            this.subAreaId = subAreaId;
            this.startDefenseTime = startDefenseTime;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element subAreaId.");
            }

            writer.WriteVarShort((short)subAreaId);
            if (startDefenseTime < 0)
            {
                throw new Exception("Forbidden value (" + startDefenseTime + ") on element startDefenseTime.");
            }

            writer.WriteByte((byte)startDefenseTime);
        }
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = (short)reader.ReadVarUhShort();
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element of PrismSettingsRequestMessage.subAreaId.");
            }

            startDefenseTime = (byte)reader.ReadByte();
            if (startDefenseTime < 0)
            {
                throw new Exception("Forbidden value (" + startDefenseTime + ") on element of PrismSettingsRequestMessage.startDefenseTime.");
            }

        }


    }
}








