using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class JobExperienceOtherPlayerUpdateMessage : JobExperienceUpdateMessage  
    { 
        public  const ushort Id = 5477;
        public override ushort MessageId => Id;

        public long playerId;

        public JobExperienceOtherPlayerUpdateMessage()
        {
        }
        public JobExperienceOtherPlayerUpdateMessage(long playerId)
        {
            this.playerId = playerId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (playerId < 0 || playerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + playerId + ") on element playerId.");
            }

            writer.WriteVarLong((long)playerId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = (long)reader.ReadVarUhLong();
            if (playerId < 0 || playerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + playerId + ") on element of JobExperienceOtherPlayerUpdateMessage.playerId.");
            }

        }


    }
}








