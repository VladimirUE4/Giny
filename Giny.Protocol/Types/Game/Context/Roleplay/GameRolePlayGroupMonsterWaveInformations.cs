using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayGroupMonsterWaveInformations : GameRolePlayGroupMonsterInformations  
    { 
        public const ushort Id = 521;
        public override ushort TypeId => Id;

        public byte nbWaves;
        public GroupMonsterStaticInformations[] alternatives;

        public GameRolePlayGroupMonsterWaveInformations()
        {
        }
        public GameRolePlayGroupMonsterWaveInformations(byte nbWaves,GroupMonsterStaticInformations[] alternatives)
        {
            this.nbWaves = nbWaves;
            this.alternatives = alternatives;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (nbWaves < 0)
            {
                throw new Exception("Forbidden value (" + nbWaves + ") on element nbWaves.");
            }

            writer.WriteByte((byte)nbWaves);
            writer.WriteShort((short)alternatives.Length);
            for (uint _i2 = 0;_i2 < alternatives.Length;_i2++)
            {
                writer.WriteShort((short)(alternatives[_i2] as GroupMonsterStaticInformations).TypeId);
                (alternatives[_i2] as GroupMonsterStaticInformations).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            uint _id2 = 0;
            GroupMonsterStaticInformations _item2 = null;
            base.Deserialize(reader);
            nbWaves = (byte)reader.ReadByte();
            if (nbWaves < 0)
            {
                throw new Exception("Forbidden value (" + nbWaves + ") on element of GameRolePlayGroupMonsterWaveInformations.nbWaves.");
            }

            uint _alternativesLen = (uint)reader.ReadUShort();
            for (uint _i2 = 0;_i2 < _alternativesLen;_i2++)
            {
                _id2 = (uint)reader.ReadUShort();
                _item2 = ProtocolTypeManager.GetInstance<GroupMonsterStaticInformations>((short)_id2);
                _item2.Deserialize(reader);
                alternatives[_i2] = _item2;
            }

        }


    }
}








