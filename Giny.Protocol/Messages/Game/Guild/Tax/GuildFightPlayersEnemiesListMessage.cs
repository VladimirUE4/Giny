using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildFightPlayersEnemiesListMessage : NetworkMessage  
    { 
        public new const ushort Id = 1605;
        public override ushort MessageId => Id;

        public double fightId;
        public CharacterMinimalPlusLookInformations[] playerInfo;

        public GuildFightPlayersEnemiesListMessage()
        {
        }
        public GuildFightPlayersEnemiesListMessage(double fightId,CharacterMinimalPlusLookInformations[] playerInfo)
        {
            this.fightId = fightId;
            this.playerInfo = playerInfo;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (fightId < 0 || fightId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + fightId + ") on element fightId.");
            }

            writer.WriteDouble((double)fightId);
            writer.WriteShort((short)playerInfo.Length);
            for (uint _i2 = 0;_i2 < playerInfo.Length;_i2++)
            {
                (playerInfo[_i2] as CharacterMinimalPlusLookInformations).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            CharacterMinimalPlusLookInformations _item2 = null;
            fightId = (double)reader.ReadDouble();
            if (fightId < 0 || fightId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + fightId + ") on element of GuildFightPlayersEnemiesListMessage.fightId.");
            }

            uint _playerInfoLen = (uint)reader.ReadUShort();
            for (uint _i2 = 0;_i2 < _playerInfoLen;_i2++)
            {
                _item2 = new CharacterMinimalPlusLookInformations();
                _item2.Deserialize(reader);
                playerInfo[_i2] = _item2;
            }

        }


    }
}








