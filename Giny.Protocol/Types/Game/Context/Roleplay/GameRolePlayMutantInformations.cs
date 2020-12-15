using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayMutantInformations : GameRolePlayHumanoidInformations  
    { 
        public const ushort Id = 1099;
        public override ushort TypeId => Id;

        public short monsterId;
        public byte powerLevel;

        public GameRolePlayMutantInformations()
        {
        }
        public GameRolePlayMutantInformations(short monsterId,byte powerLevel)
        {
            this.monsterId = monsterId;
            this.powerLevel = powerLevel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (monsterId < 0)
            {
                throw new Exception("Forbidden value (" + monsterId + ") on element monsterId.");
            }

            writer.WriteVarShort((short)monsterId);
            writer.WriteByte((byte)powerLevel);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterId = (short)reader.ReadVarUhShort();
            if (monsterId < 0)
            {
                throw new Exception("Forbidden value (" + monsterId + ") on element of GameRolePlayMutantInformations.monsterId.");
            }

            powerLevel = (byte)reader.ReadByte();
        }


    }
}








