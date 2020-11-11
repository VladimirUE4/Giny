using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildFightTakePlaceRequestMessage : GuildFightJoinRequestMessage  
    { 
        public new const ushort Id = 8763;
        public override ushort MessageId => Id;

        public int replacedCharacterId;

        public GuildFightTakePlaceRequestMessage()
        {
        }
        public GuildFightTakePlaceRequestMessage(int replacedCharacterId)
        {
            this.replacedCharacterId = replacedCharacterId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt((int)replacedCharacterId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            replacedCharacterId = (int)reader.ReadInt();
        }


    }
}








