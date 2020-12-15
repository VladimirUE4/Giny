using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class AchievementStartedObjective : AchievementObjective  
    { 
        public const ushort Id = 6439;
        public override ushort TypeId => Id;

        public short value;

        public AchievementStartedObjective()
        {
        }
        public AchievementStartedObjective(short value)
        {
            this.value = value;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (value < 0)
            {
                throw new Exception("Forbidden value (" + value + ") on element value.");
            }

            writer.WriteVarShort((short)value);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = (short)reader.ReadVarUhShort();
            if (value < 0)
            {
                throw new Exception("Forbidden value (" + value + ") on element of AchievementStartedObjective.value.");
            }

        }


    }
}








