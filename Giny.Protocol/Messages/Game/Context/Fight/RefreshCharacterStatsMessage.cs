using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class RefreshCharacterStatsMessage : NetworkMessage  
    { 
        public new const ushort Id = 3914;
        public override ushort MessageId => Id;

        public double fighterId;
        public GameFightMinimalStats stats;

        public RefreshCharacterStatsMessage()
        {
        }
        public RefreshCharacterStatsMessage(double fighterId,GameFightMinimalStats stats)
        {
            this.fighterId = fighterId;
            this.stats = stats;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (fighterId < -9.00719925474099E+15 || fighterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + fighterId + ") on element fighterId.");
            }

            writer.WriteDouble((double)fighterId);
            stats.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            fighterId = (double)reader.ReadDouble();
            if (fighterId < -9.00719925474099E+15 || fighterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + fighterId + ") on element of RefreshCharacterStatsMessage.fighterId.");
            }

            stats = new GameFightMinimalStats();
            stats.Deserialize(reader);
        }


    }
}








