using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GuildFactSheetInformations : GuildInformations  
    { 
        public const ushort Id = 3641;
        public override ushort TypeId => Id;

        public long leaderId;
        public short nbMembers;

        public GuildFactSheetInformations()
        {
        }
        public GuildFactSheetInformations(long leaderId,short nbMembers)
        {
            this.leaderId = leaderId;
            this.nbMembers = nbMembers;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (leaderId < 0 || leaderId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + leaderId + ") on element leaderId.");
            }

            writer.WriteVarLong((long)leaderId);
            if (nbMembers < 0)
            {
                throw new Exception("Forbidden value (" + nbMembers + ") on element nbMembers.");
            }

            writer.WriteVarShort((short)nbMembers);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            leaderId = (long)reader.ReadVarUhLong();
            if (leaderId < 0 || leaderId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + leaderId + ") on element of GuildFactSheetInformations.leaderId.");
            }

            nbMembers = (short)reader.ReadVarUhShort();
            if (nbMembers < 0)
            {
                throw new Exception("Forbidden value (" + nbMembers + ") on element of GuildFactSheetInformations.nbMembers.");
            }

        }


    }
}








