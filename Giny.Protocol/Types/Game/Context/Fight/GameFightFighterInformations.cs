using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightFighterInformations : GameContextActorInformations  
    { 
        public const ushort Id = 4629;
        public override ushort TypeId => Id;

        public GameContextBasicSpawnInformation spawnInfo;
        public byte wave;
        public GameFightMinimalStats stats;
        public short[] previousPositions;

        public GameFightFighterInformations()
        {
        }
        public GameFightFighterInformations(GameContextBasicSpawnInformation spawnInfo,byte wave,GameFightMinimalStats stats,short[] previousPositions)
        {
            this.spawnInfo = spawnInfo;
            this.wave = wave;
            this.stats = stats;
            this.previousPositions = previousPositions;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            spawnInfo.Serialize(writer);
            if (wave < 0)
            {
                throw new Exception("Forbidden value (" + wave + ") on element wave.");
            }

            writer.WriteByte((byte)wave);
            writer.WriteShort((short)stats.TypeId);
            stats.Serialize(writer);
            writer.WriteShort((short)previousPositions.Length);
            for (uint _i4 = 0;_i4 < previousPositions.Length;_i4++)
            {
                if (previousPositions[_i4] < 0 || previousPositions[_i4] > 559)
                {
                    throw new Exception("Forbidden value (" + previousPositions[_i4] + ") on element 4 (starting at 1) of previousPositions.");
                }

                writer.WriteVarShort((short)previousPositions[_i4]);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            uint _val4 = 0;
            base.Deserialize(reader);
            spawnInfo = new GameContextBasicSpawnInformation();
            spawnInfo.Deserialize(reader);
            wave = (byte)reader.ReadByte();
            if (wave < 0)
            {
                throw new Exception("Forbidden value (" + wave + ") on element of GameFightFighterInformations.wave.");
            }

            uint _id3 = (uint)reader.ReadUShort();
            stats = ProtocolTypeManager.GetInstance<GameFightMinimalStats>((short)_id3);
            stats.Deserialize(reader);
            uint _previousPositionsLen = (uint)reader.ReadUShort();
            previousPositions = new short[_previousPositionsLen];
            for (uint _i4 = 0;_i4 < _previousPositionsLen;_i4++)
            {
                _val4 = (uint)reader.ReadVarUhShort();
                if (_val4 < 0 || _val4 > 559)
                {
                    throw new Exception("Forbidden value (" + _val4 + ") on elements of previousPositions.");
                }

                previousPositions[_i4] = (short)_val4;
            }

        }


    }
}








