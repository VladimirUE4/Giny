using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterUsableCharacteristicDetailed : CharacterCharacteristicDetailed  
    { 
        public const ushort Id = 9575;
        public override ushort TypeId => Id;

        public short used;

        public CharacterUsableCharacteristicDetailed()
        {
        }
        public CharacterUsableCharacteristicDetailed(short used)
        {
            this.used = used;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (used < 0)
            {
                throw new Exception("Forbidden value (" + used + ") on element used.");
            }

            writer.WriteVarShort((short)used);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            used = (short)reader.ReadVarUhShort();
            if (used < 0)
            {
                throw new Exception("Forbidden value (" + used + ") on element of CharacterUsableCharacteristicDetailed.used.");
            }

        }


    }
}








