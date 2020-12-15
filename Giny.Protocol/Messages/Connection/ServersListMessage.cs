using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ServersListMessage : NetworkMessage  
    { 
        public new const ushort Id = 5241;
        public override ushort MessageId => Id;

        public GameServerInformations[] servers;
        public short alreadyConnectedToServerId;
        public bool canCreateNewCharacter;

        public ServersListMessage()
        {
        }
        public ServersListMessage(GameServerInformations[] servers,short alreadyConnectedToServerId,bool canCreateNewCharacter)
        {
            this.servers = servers;
            this.alreadyConnectedToServerId = alreadyConnectedToServerId;
            this.canCreateNewCharacter = canCreateNewCharacter;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)servers.Length);
            for (uint _i1 = 0;_i1 < servers.Length;_i1++)
            {
                (servers[_i1] as GameServerInformations).Serialize(writer);
            }

            if (alreadyConnectedToServerId < 0)
            {
                throw new Exception("Forbidden value (" + alreadyConnectedToServerId + ") on element alreadyConnectedToServerId.");
            }

            writer.WriteVarShort((short)alreadyConnectedToServerId);
            writer.WriteBoolean((bool)canCreateNewCharacter);
        }
        public override void Deserialize(IDataReader reader)
        {
            GameServerInformations _item1 = null;
            uint _serversLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _serversLen;_i1++)
            {
                _item1 = new GameServerInformations();
                _item1.Deserialize(reader);
                servers[_i1] = _item1;
            }

            alreadyConnectedToServerId = (short)reader.ReadVarUhShort();
            if (alreadyConnectedToServerId < 0)
            {
                throw new Exception("Forbidden value (" + alreadyConnectedToServerId + ") on element of ServersListMessage.alreadyConnectedToServerId.");
            }

            canCreateNewCharacter = (bool)reader.ReadBoolean();
        }


    }
}








