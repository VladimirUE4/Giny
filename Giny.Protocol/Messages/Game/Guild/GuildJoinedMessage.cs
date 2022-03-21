using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildJoinedMessage : NetworkMessage  
    { 
        public new const ushort Id = 1218;
        public override ushort MessageId => Id;

        public GuildInformations guildInfo;
        public int memberRights;

        public GuildJoinedMessage()
        {
        }
        public GuildJoinedMessage(GuildInformations guildInfo,int memberRights)
        {
            this.guildInfo = guildInfo;
            this.memberRights = memberRights;
        }
        public override void Serialize(IDataWriter writer)
        {
            guildInfo.Serialize(writer);
            if (memberRights < 0)
            {
                throw new System.Exception("Forbidden value (" + memberRights + ") on element memberRights.");
            }

            writer.WriteVarInt((int)memberRights);
        }
        public override void Deserialize(IDataReader reader)
        {
            guildInfo = new GuildInformations();
            guildInfo.Deserialize(reader);
            memberRights = (int)reader.ReadVarUhInt();
            if (memberRights < 0)
            {
                throw new System.Exception("Forbidden value (" + memberRights + ") on element of GuildJoinedMessage.memberRights.");
            }

        }


    }
}








