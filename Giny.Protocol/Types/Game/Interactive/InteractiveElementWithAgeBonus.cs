using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class InteractiveElementWithAgeBonus : InteractiveElement  
    { 
        public const ushort Id = 5847;
        public override ushort TypeId => Id;

        public short ageBonus;

        public InteractiveElementWithAgeBonus()
        {
        }
        public InteractiveElementWithAgeBonus(short ageBonus)
        {
            this.ageBonus = ageBonus;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (ageBonus < -1 || ageBonus > 1000)
            {
                throw new Exception("Forbidden value (" + ageBonus + ") on element ageBonus.");
            }

            writer.WriteShort((short)ageBonus);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            ageBonus = (short)reader.ReadShort();
            if (ageBonus < -1 || ageBonus > 1000)
            {
                throw new Exception("Forbidden value (" + ageBonus + ") on element of InteractiveElementWithAgeBonus.ageBonus.");
            }

        }


    }
}








