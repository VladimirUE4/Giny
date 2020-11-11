using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class SkillActionDescription  
    { 
        public const ushort Id = 6637;
        public virtual ushort TypeId => Id;

        public short skillId;

        public SkillActionDescription()
        {
        }
        public SkillActionDescription(short skillId)
        {
            this.skillId = skillId;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            if (skillId < 0)
            {
                throw new Exception("Forbidden value (" + skillId + ") on element skillId.");
            }

            writer.WriteVarShort((short)skillId);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            skillId = (short)reader.ReadVarUhShort();
            if (skillId < 0)
            {
                throw new Exception("Forbidden value (" + skillId + ") on element of SkillActionDescription.skillId.");
            }

        }


    }
}








