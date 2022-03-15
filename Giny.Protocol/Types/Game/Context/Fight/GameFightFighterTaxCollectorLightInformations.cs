using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightFighterTaxCollectorLightInformations : GameFightFighterLightInformations  
    { 
        public const ushort Id = 6139;
        public override ushort TypeId => Id;

        public short firstNameId;
        public short lastNameId;

        public GameFightFighterTaxCollectorLightInformations()
        {
        }
        public GameFightFighterTaxCollectorLightInformations(short firstNameId,short lastNameId)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (firstNameId < 0)
            {
                throw new Exception("Forbidden value (" + firstNameId + ") on element firstNameId.");
            }

            writer.WriteVarShort((short)firstNameId);
            if (lastNameId < 0)
            {
                throw new Exception("Forbidden value (" + lastNameId + ") on element lastNameId.");
            }

            writer.WriteVarShort((short)lastNameId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            firstNameId = (short)reader.ReadVarUhShort();
            if (firstNameId < 0)
            {
                throw new Exception("Forbidden value (" + firstNameId + ") on element of GameFightFighterTaxCollectorLightInformations.firstNameId.");
            }

            lastNameId = (short)reader.ReadVarUhShort();
            if (lastNameId < 0)
            {
                throw new Exception("Forbidden value (" + lastNameId + ") on element of GameFightFighterTaxCollectorLightInformations.lastNameId.");
            }

        }


    }
}








