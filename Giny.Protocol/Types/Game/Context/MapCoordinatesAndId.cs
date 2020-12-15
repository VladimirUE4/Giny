using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class MapCoordinatesAndId : MapCoordinates  
    { 
        public const ushort Id = 3427;
        public override ushort TypeId => Id;

        public double mapId;

        public MapCoordinatesAndId()
        {
        }
        public MapCoordinatesAndId(double mapId)
        {
            this.mapId = mapId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element mapId.");
            }

            writer.WriteDouble((double)mapId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of MapCoordinatesAndId.mapId.");
            }

        }


    }
}








