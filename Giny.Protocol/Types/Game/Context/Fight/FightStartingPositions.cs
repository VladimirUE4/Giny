using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightStartingPositions  
    { 
        public const ushort Id = 9707;
        public virtual ushort TypeId => Id;

        public short[] positionsForChallengers;
        public short[] positionsForDefenders;

        public FightStartingPositions()
        {
        }
        public FightStartingPositions(short[] positionsForChallengers,short[] positionsForDefenders)
        {
            this.positionsForChallengers = positionsForChallengers;
            this.positionsForDefenders = positionsForDefenders;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)positionsForChallengers.Length);
            for (uint _i1 = 0;_i1 < positionsForChallengers.Length;_i1++)
            {
                if (positionsForChallengers[_i1] < 0 || positionsForChallengers[_i1] > 559)
                {
                    throw new Exception("Forbidden value (" + positionsForChallengers[_i1] + ") on element 1 (starting at 1) of positionsForChallengers.");
                }

                writer.WriteVarShort((short)positionsForChallengers[_i1]);
            }

            writer.WriteShort((short)positionsForDefenders.Length);
            for (uint _i2 = 0;_i2 < positionsForDefenders.Length;_i2++)
            {
                if (positionsForDefenders[_i2] < 0 || positionsForDefenders[_i2] > 559)
                {
                    throw new Exception("Forbidden value (" + positionsForDefenders[_i2] + ") on element 2 (starting at 1) of positionsForDefenders.");
                }

                writer.WriteVarShort((short)positionsForDefenders[_i2]);
            }

        }
        public virtual void Deserialize(IDataReader reader)
        {
            uint _val1 = 0;
            uint _val2 = 0;
            uint _positionsForChallengersLen = (uint)reader.ReadUShort();
            positionsForChallengers = new short[_positionsForChallengersLen];
            for (uint _i1 = 0;_i1 < _positionsForChallengersLen;_i1++)
            {
                _val1 = (uint)reader.ReadVarUhShort();
                if (_val1 < 0 || _val1 > 559)
                {
                    throw new Exception("Forbidden value (" + _val1 + ") on elements of positionsForChallengers.");
                }

                positionsForChallengers[_i1] = (short)_val1;
            }

            uint _positionsForDefendersLen = (uint)reader.ReadUShort();
            positionsForDefenders = new short[_positionsForDefendersLen];
            for (uint _i2 = 0;_i2 < _positionsForDefendersLen;_i2++)
            {
                _val2 = (uint)reader.ReadVarUhShort();
                if (_val2 < 0 || _val2 > 559)
                {
                    throw new Exception("Forbidden value (" + _val2 + ") on elements of positionsForDefenders.");
                }

                positionsForDefenders[_i2] = (short)_val2;
            }

        }


    }
}








