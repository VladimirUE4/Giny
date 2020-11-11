using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class AllianceFactSheetInformations : AllianceInformations  
    { 
        public const ushort Id = 4179;
        public override ushort TypeId => Id;

        public int creationDate;

        public AllianceFactSheetInformations()
        {
        }
        public AllianceFactSheetInformations(int creationDate)
        {
            this.creationDate = creationDate;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (creationDate < 0)
            {
                throw new Exception("Forbidden value (" + creationDate + ") on element creationDate.");
            }

            writer.WriteInt((int)creationDate);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            creationDate = (int)reader.ReadInt();
            if (creationDate < 0)
            {
                throw new Exception("Forbidden value (" + creationDate + ") on element of AllianceFactSheetInformations.creationDate.");
            }

        }


    }
}








