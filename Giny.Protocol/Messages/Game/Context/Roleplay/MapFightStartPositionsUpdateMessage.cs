using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MapFightStartPositionsUpdateMessage : NetworkMessage  
    { 
        public new const ushort Id = 8447;
        public override ushort MessageId => Id;

        public double mapId;
        public FightStartingPositions fightStartPositions;

        public MapFightStartPositionsUpdateMessage()
        {
        }
        public MapFightStartPositionsUpdateMessage(double mapId,FightStartingPositions fightStartPositions)
        {
            this.mapId = mapId;
            this.fightStartPositions = fightStartPositions;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element mapId.");
            }

            writer.WriteDouble((double)mapId);
            fightStartPositions.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of MapFightStartPositionsUpdateMessage.mapId.");
            }

            fightStartPositions = new FightStartingPositions();
            fightStartPositions.Deserialize(reader);
        }


    }
}








