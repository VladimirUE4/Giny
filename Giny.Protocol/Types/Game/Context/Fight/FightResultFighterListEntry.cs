using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightResultFighterListEntry : FightResultListEntry  
    { 
        public const ushort Id = 7464;
        public override ushort TypeId => Id;

        public double id;
        public bool alive;

        public FightResultFighterListEntry()
        {
        }
        public FightResultFighterListEntry(double id,bool alive)
        {
            this.id = id;
            this.alive = alive;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (id < -9.00719925474099E+15 || id > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + id + ") on element id.");
            }

            writer.WriteDouble((double)id);
            writer.WriteBoolean((bool)alive);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            id = (double)reader.ReadDouble();
            if (id < -9.00719925474099E+15 || id > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + id + ") on element of FightResultFighterListEntry.id.");
            }

            alive = (bool)reader.ReadBoolean();
        }


    }
}








