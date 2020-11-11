using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightMinimalStatsPreparation : GameFightMinimalStats  
    { 
        public const ushort Id = 5419;
        public override ushort TypeId => Id;

        public int initiative;

        public GameFightMinimalStatsPreparation()
        {
        }
        public GameFightMinimalStatsPreparation(int initiative)
        {
            this.initiative = initiative;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (initiative < 0)
            {
                throw new Exception("Forbidden value (" + initiative + ") on element initiative.");
            }

            writer.WriteVarInt((int)initiative);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            initiative = (int)reader.ReadVarUhInt();
            if (initiative < 0)
            {
                throw new Exception("Forbidden value (" + initiative + ") on element of GameFightMinimalStatsPreparation.initiative.");
            }

        }


    }
}








