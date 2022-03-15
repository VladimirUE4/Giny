using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GuildInAllianceInformations : GuildInformations  
    { 
        public const ushort Id = 8343;
        public override ushort TypeId => Id;

        public byte nbMembers;
        public int joinDate;

        public GuildInAllianceInformations()
        {
        }
        public GuildInAllianceInformations(byte nbMembers,int joinDate)
        {
            this.nbMembers = nbMembers;
            this.joinDate = joinDate;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (nbMembers < 1 || nbMembers > 240)
            {
                throw new Exception("Forbidden value (" + nbMembers + ") on element nbMembers.");
            }

            writer.WriteByte((byte)nbMembers);
            if (joinDate < 0)
            {
                throw new Exception("Forbidden value (" + joinDate + ") on element joinDate.");
            }

            writer.WriteInt((int)joinDate);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            nbMembers = (byte)reader.ReadSByte();
            if (nbMembers < 1 || nbMembers > 240)
            {
                throw new Exception("Forbidden value (" + nbMembers + ") on element of GuildInAllianceInformations.nbMembers.");
            }

            joinDate = (int)reader.ReadInt();
            if (joinDate < 0)
            {
                throw new Exception("Forbidden value (" + joinDate + ") on element of GuildInAllianceInformations.joinDate.");
            }

        }


    }
}








