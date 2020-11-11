using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameRolePlayArenaUpdatePlayerInfosAllQueuesMessage : GameRolePlayArenaUpdatePlayerInfosMessage  
    { 
        public new const ushort Id = 5122;
        public override ushort MessageId => Id;

        public ArenaRankInfos team;
        public ArenaRankInfos duel;

        public GameRolePlayArenaUpdatePlayerInfosAllQueuesMessage()
        {
        }
        public GameRolePlayArenaUpdatePlayerInfosAllQueuesMessage(ArenaRankInfos team,ArenaRankInfos duel)
        {
            this.team = team;
            this.duel = duel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            team.Serialize(writer);
            duel.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            team = new ArenaRankInfos();
            team.Deserialize(reader);
            duel = new ArenaRankInfos();
            duel.Deserialize(reader);
        }


    }
}








