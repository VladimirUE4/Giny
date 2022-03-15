using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class MapCoordinatesExtended : MapCoordinatesAndId  
    { 
        public const ushort Id = 3550;
        public override ushort TypeId => Id;

        public short subAreaId;

        public MapCoordinatesExtended()
        {
        }
        public MapCoordinatesExtended(short subAreaId)
        {
            this.subAreaId = subAreaId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element subAreaId.");
            }

            writer.WriteVarShort((short)subAreaId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            subAreaId = (short)reader.ReadVarUhShort();
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element of MapCoordinatesExtended.subAreaId.");
            }

        }


    }
}








