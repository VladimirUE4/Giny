using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class KamaDebtInformation : DebtInformation  
    { 
        public const ushort Id = 6089;
        public override ushort TypeId => Id;

        public long kamas;

        public KamaDebtInformation()
        {
        }
        public KamaDebtInformation(long kamas)
        {
            this.kamas = kamas;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (kamas < 0 || kamas > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kamas + ") on element kamas.");
            }

            writer.WriteVarLong((long)kamas);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            kamas = (long)reader.ReadVarUhLong();
            if (kamas < 0 || kamas > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + kamas + ") on element of KamaDebtInformation.kamas.");
            }

        }


    }
}








