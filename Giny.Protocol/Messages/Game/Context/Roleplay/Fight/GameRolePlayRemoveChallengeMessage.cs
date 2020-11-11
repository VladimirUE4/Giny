using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameRolePlayRemoveChallengeMessage : NetworkMessage  
    { 
        public new const ushort Id = 889;
        public override ushort MessageId => Id;

        public short fightId;

        public GameRolePlayRemoveChallengeMessage()
        {
        }
        public GameRolePlayRemoveChallengeMessage(short fightId)
        {
            this.fightId = fightId;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (fightId < 0)
            {
                throw new Exception("Forbidden value (" + fightId + ") on element fightId.");
            }

            writer.WriteVarShort((short)fightId);
        }
        public override void Deserialize(IDataReader reader)
        {
            fightId = (short)reader.ReadVarUhShort();
            if (fightId < 0)
            {
                throw new Exception("Forbidden value (" + fightId + ") on element of GameRolePlayRemoveChallengeMessage.fightId.");
            }

        }


    }
}








