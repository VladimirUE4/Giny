using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GuildFactSheetInformations : GuildInformations  
    { 
        public const ushort Id = 7387;
        public override ushort TypeId => Id;

        public long leaderId;
        public short nbMembers;
        public short lastActivityDay;
        public GuildRecruitmentInformation recruitment;
        public int nbPendingApply;

        public GuildFactSheetInformations()
        {
        }
        public GuildFactSheetInformations(long leaderId,short nbMembers,short lastActivityDay,GuildRecruitmentInformation recruitment,int nbPendingApply)
        {
            this.leaderId = leaderId;
            this.nbMembers = nbMembers;
            this.lastActivityDay = lastActivityDay;
            this.recruitment = recruitment;
            this.nbPendingApply = nbPendingApply;
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
            if (lastActivityDay < 0)
            {
                throw new Exception("Forbidden value (" + lastActivityDay + ") on element lastActivityDay.");
            }

            writer.WriteShort((short)lastActivityDay);
            recruitment.Serialize(writer);
            if (nbPendingApply < 0)
            {
                throw new Exception("Forbidden value (" + nbPendingApply + ") on element nbPendingApply.");
            }

            writer.WriteInt((int)nbPendingApply);
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

            lastActivityDay = (short)reader.ReadShort();
            if (lastActivityDay < 0)
            {
                throw new Exception("Forbidden value (" + lastActivityDay + ") on element of GuildFactSheetInformations.lastActivityDay.");
            }

            recruitment = new GuildRecruitmentInformation();
            recruitment.Deserialize(reader);
            nbPendingApply = (int)reader.ReadInt();
            if (nbPendingApply < 0)
            {
                throw new Exception("Forbidden value (" + nbPendingApply + ") on element of GuildFactSheetInformations.nbPendingApply.");
            }

        }


    }
}








