using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AllianceFactsErrorMessage : NetworkMessage  
    { 
        public new const ushort Id = 2007;
        public override ushort MessageId => Id;

        public int allianceId;

        public AllianceFactsErrorMessage()
        {
        }
        public AllianceFactsErrorMessage(int allianceId)
        {
            this.allianceId = allianceId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (allianceId < 0)
            {
                throw new Exception("Forbidden value (" + allianceId + ") on element allianceId.");
            }

            writer.WriteVarInt((int)allianceId);
        }
        public override void Deserialize(IDataReader reader)
        {
            allianceId = (int)reader.ReadVarUhInt();
            if (allianceId < 0)
            {
                throw new Exception("Forbidden value (" + allianceId + ") on element of AllianceFactsExceptionMessage.allianceId.");
            }

        }


    }
}








