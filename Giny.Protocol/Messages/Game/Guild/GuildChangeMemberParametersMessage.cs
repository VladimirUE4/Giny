using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildChangeMemberParametersMessage : NetworkMessage  
    { 
        public new const ushort Id = 2996;
        public override ushort MessageId => Id;

        public long memberId;
        public short rank;
        public byte experienceGivenPercent;
        public int rights;

        public GuildChangeMemberParametersMessage()
        {
        }
        public GuildChangeMemberParametersMessage(long memberId,short rank,byte experienceGivenPercent,int rights)
        {
            this.memberId = memberId;
            this.rank = rank;
            this.experienceGivenPercent = experienceGivenPercent;
            this.rights = rights;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (memberId < 0 || memberId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + memberId + ") on element memberId.");
            }

            writer.WriteVarLong((long)memberId);
            if (rank < 0)
            {
                throw new Exception("Forbidden value (" + rank + ") on element rank.");
            }

            writer.WriteVarShort((short)rank);
            if (experienceGivenPercent < 0 || experienceGivenPercent > 100)
            {
                throw new Exception("Forbidden value (" + experienceGivenPercent + ") on element experienceGivenPercent.");
            }

            writer.WriteByte((byte)experienceGivenPercent);
            if (rights < 0)
            {
                throw new Exception("Forbidden value (" + rights + ") on element rights.");
            }

            writer.WriteVarInt((int)rights);
        }
        public override void Deserialize(IDataReader reader)
        {
            memberId = (long)reader.ReadVarUhLong();
            if (memberId < 0 || memberId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + memberId + ") on element of GuildChangeMemberParametersMessage.memberId.");
            }

            rank = (short)reader.ReadVarUhShort();
            if (rank < 0)
            {
                throw new Exception("Forbidden value (" + rank + ") on element of GuildChangeMemberParametersMessage.rank.");
            }

            experienceGivenPercent = (byte)reader.ReadByte();
            if (experienceGivenPercent < 0 || experienceGivenPercent > 100)
            {
                throw new Exception("Forbidden value (" + experienceGivenPercent + ") on element of GuildChangeMemberParametersMessage.experienceGivenPercent.");
            }

            rights = (int)reader.ReadVarUhInt();
            if (rights < 0)
            {
                throw new Exception("Forbidden value (" + rights + ") on element of GuildChangeMemberParametersMessage.rights.");
            }

        }


    }
}








