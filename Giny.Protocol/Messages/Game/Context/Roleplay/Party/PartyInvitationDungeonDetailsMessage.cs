using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyInvitationDungeonDetailsMessage : PartyInvitationDetailsMessage  
    { 
        public  const ushort Id = 7340;
        public override ushort MessageId => Id;

        public short dungeonId;
        public bool[] playersDungeonReady;

        public PartyInvitationDungeonDetailsMessage()
        {
        }
        public PartyInvitationDungeonDetailsMessage(short dungeonId,bool[] playersDungeonReady)
        {
            this.dungeonId = dungeonId;
            this.playersDungeonReady = playersDungeonReady;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (dungeonId < 0)
            {
                throw new Exception("Forbidden value (" + dungeonId + ") on element dungeonId.");
            }

            writer.WriteVarShort((short)dungeonId);
            writer.WriteShort((short)playersDungeonReady.Length);
            for (uint _i2 = 0;_i2 < playersDungeonReady.Length;_i2++)
            {
                writer.WriteBoolean((bool)playersDungeonReady[_i2]);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            bool _val2 = false;
            base.Deserialize(reader);
            dungeonId = (short)reader.ReadVarUhShort();
            if (dungeonId < 0)
            {
                throw new Exception("Forbidden value (" + dungeonId + ") on element of PartyInvitationDungeonDetailsMessage.dungeonId.");
            }

            uint _playersDungeonReadyLen = (uint)reader.ReadUShort();
            playersDungeonReady = new bool[_playersDungeonReadyLen];
            for (uint _i2 = 0;_i2 < _playersDungeonReadyLen;_i2++)
            {
                _val2 = (bool)reader.ReadBoolean();
                playersDungeonReady[_i2] = (bool)_val2;
            }

        }


    }
}








