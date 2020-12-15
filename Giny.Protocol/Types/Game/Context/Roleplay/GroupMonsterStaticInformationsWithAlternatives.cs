using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GroupMonsterStaticInformationsWithAlternatives : GroupMonsterStaticInformations  
    { 
        public const ushort Id = 7681;
        public override ushort TypeId => Id;

        public AlternativeMonstersInGroupLightInformations[] alternatives;

        public GroupMonsterStaticInformationsWithAlternatives()
        {
        }
        public GroupMonsterStaticInformationsWithAlternatives(AlternativeMonstersInGroupLightInformations[] alternatives)
        {
            this.alternatives = alternatives;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)alternatives.Length);
            for (uint _i1 = 0;_i1 < alternatives.Length;_i1++)
            {
                (alternatives[_i1] as AlternativeMonstersInGroupLightInformations).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            AlternativeMonstersInGroupLightInformations _item1 = null;
            base.Deserialize(reader);
            uint _alternativesLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _alternativesLen;_i1++)
            {
                _item1 = new AlternativeMonstersInGroupLightInformations();
                _item1.Deserialize(reader);
                alternatives[_i1] = _item1;
            }

        }


    }
}








