using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameRolePlayShowChallengeMessage : NetworkMessage  
    { 
        public new const ushort Id = 5095;
        public override ushort MessageId => Id;

        public FightCommonInformations commonsInfos;

        public GameRolePlayShowChallengeMessage()
        {
        }
        public GameRolePlayShowChallengeMessage(FightCommonInformations commonsInfos)
        {
            this.commonsInfos = commonsInfos;
        }
        public override void Serialize(IDataWriter writer)
        {
            commonsInfos.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            commonsInfos = new FightCommonInformations();
            commonsInfos.Deserialize(reader);
        }


    }
}








