using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class EntitiesPreset : Preset  
    { 
        public const ushort Id = 1197;
        public override ushort TypeId => Id;

        public short iconId;
        public short[] entityIds;

        public EntitiesPreset()
        {
        }
        public EntitiesPreset(short iconId,short[] entityIds)
        {
            this.iconId = iconId;
            this.entityIds = entityIds;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (iconId < 0)
            {
                throw new Exception("Forbidden value (" + iconId + ") on element iconId.");
            }

            writer.WriteShort((short)iconId);
            writer.WriteShort((short)entityIds.Length);
            for (uint _i2 = 0;_i2 < entityIds.Length;_i2++)
            {
                if (entityIds[_i2] < 0)
                {
                    throw new Exception("Forbidden value (" + entityIds[_i2] + ") on element 2 (starting at 1) of entityIds.");
                }

                writer.WriteVarShort((short)entityIds[_i2]);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            uint _val2 = 0;
            base.Deserialize(reader);
            iconId = (short)reader.ReadShort();
            if (iconId < 0)
            {
                throw new Exception("Forbidden value (" + iconId + ") on element of EntitiesPreset.iconId.");
            }

            uint _entityIdsLen = (uint)reader.ReadUShort();
            entityIds = new short[_entityIdsLen];
            for (uint _i2 = 0;_i2 < _entityIdsLen;_i2++)
            {
                _val2 = (uint)reader.ReadVarUhShort();
                if (_val2 < 0)
                {
                    throw new Exception("Forbidden value (" + _val2 + ") on elements of entityIds.");
                }

                entityIds[_i2] = (short)_val2;
            }

        }


    }
}








