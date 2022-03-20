using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildApplicationAnswerMessage : NetworkMessage  
    { 
        public  const ushort Id = 5404;
        public override ushort MessageId => Id;

        public bool accepted;
        public int playerId;

        public GuildApplicationAnswerMessage()
        {
        }
        public GuildApplicationAnswerMessage(bool accepted,int playerId)
        {
            this.accepted = accepted;
            this.playerId = playerId;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean((bool)accepted);
            if (playerId < 0)
            {
                throw new System.Exception("Forbidden value (" + playerId + ") on element playerId.");
            }

            writer.WriteVarInt((int)playerId);
        }
        public override void Deserialize(IDataReader reader)
        {
            accepted = (bool)reader.ReadBoolean();
            playerId = (int)reader.ReadVarUhInt();
            if (playerId < 0)
            {
                throw new System.Exception("Forbidden value (" + playerId + ") on element of GuildApplicationAnswerMessage.playerId.");
            }

        }


    }
}








