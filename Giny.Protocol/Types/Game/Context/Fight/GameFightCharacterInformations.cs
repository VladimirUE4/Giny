using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightCharacterInformations : GameFightFighterNamedInformations  
    { 
        public const ushort Id = 5123;
        public override ushort TypeId => Id;

        public short level;
        public ActorAlignmentInformations alignmentInfos;
        public byte breed;
        public bool sex;

        public GameFightCharacterInformations()
        {
        }
        public GameFightCharacterInformations(short level,ActorAlignmentInformations alignmentInfos,byte breed,bool sex)
        {
            this.level = level;
            this.alignmentInfos = alignmentInfos;
            this.breed = breed;
            this.sex = sex;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (level < 0)
            {
                throw new Exception("Forbidden value (" + level + ") on element level.");
            }

            writer.WriteVarShort((short)level);
            alignmentInfos.Serialize(writer);
            writer.WriteByte((byte)breed);
            writer.WriteBoolean((bool)sex);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            level = (short)reader.ReadVarUhShort();
            if (level < 0)
            {
                throw new Exception("Forbidden value (" + level + ") on element of GameFightCharacterInformations.level.");
            }

            alignmentInfos = new ActorAlignmentInformations();
            alignmentInfos.Deserialize(reader);
            breed = (byte)reader.ReadByte();
            sex = (bool)reader.ReadBoolean();
        }


    }
}








