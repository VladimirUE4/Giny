using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GuildInsiderFactSheetInformations : GuildFactSheetInformations  
    { 
        public const ushort Id = 9940;
        public override ushort TypeId => Id;

        public string leaderName;
        public short nbConnectedMembers;
        public byte nbTaxCollectors;
        public int lastActivity;

        public GuildInsiderFactSheetInformations()
        {
        }
        public GuildInsiderFactSheetInformations(string leaderName,short nbConnectedMembers,byte nbTaxCollectors,int lastActivity)
        {
            this.leaderName = leaderName;
            this.nbConnectedMembers = nbConnectedMembers;
            this.nbTaxCollectors = nbTaxCollectors;
            this.lastActivity = lastActivity;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)leaderName);
            if (nbConnectedMembers < 0)
            {
                throw new Exception("Forbidden value (" + nbConnectedMembers + ") on element nbConnectedMembers.");
            }

            writer.WriteVarShort((short)nbConnectedMembers);
            if (nbTaxCollectors < 0)
            {
                throw new Exception("Forbidden value (" + nbTaxCollectors + ") on element nbTaxCollectors.");
            }

            writer.WriteByte((byte)nbTaxCollectors);
            if (lastActivity < 0)
            {
                throw new Exception("Forbidden value (" + lastActivity + ") on element lastActivity.");
            }

            writer.WriteInt((int)lastActivity);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            leaderName = (string)reader.ReadUTF();
            nbConnectedMembers = (short)reader.ReadVarUhShort();
            if (nbConnectedMembers < 0)
            {
                throw new Exception("Forbidden value (" + nbConnectedMembers + ") on element of GuildInsiderFactSheetInformations.nbConnectedMembers.");
            }

            nbTaxCollectors = (byte)reader.ReadByte();
            if (nbTaxCollectors < 0)
            {
                throw new Exception("Forbidden value (" + nbTaxCollectors + ") on element of GuildInsiderFactSheetInformations.nbTaxCollectors.");
            }

            lastActivity = (int)reader.ReadInt();
            if (lastActivity < 0)
            {
                throw new Exception("Forbidden value (" + lastActivity + ") on element of GuildInsiderFactSheetInformations.lastActivity.");
            }

        }


    }
}








