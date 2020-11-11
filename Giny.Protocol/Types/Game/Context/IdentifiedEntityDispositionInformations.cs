using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class IdentifiedEntityDispositionInformations : EntityDispositionInformations  
    { 
        public const ushort Id = 7938;
        public override ushort TypeId => Id;

        public double id;

        public IdentifiedEntityDispositionInformations()
        {
        }
        public IdentifiedEntityDispositionInformations(double id)
        {
            this.id = id;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (id < -9.00719925474099E+15 || id > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + id + ") on element id.");
            }

            writer.WriteDouble((double)id);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            id = (double)reader.ReadDouble();
            if (id < -9.00719925474099E+15 || id > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + id + ") on element of IdentifiedEntityDispositionInformations.id.");
            }

        }


    }
}








