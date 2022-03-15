using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class SelectedServerDataExtendedMessage : SelectedServerDataMessage  
    { 
        public  const ushort Id = 2850;
        public override ushort MessageId => Id;

        public GameServerInformations[] servers;

        public SelectedServerDataExtendedMessage()
        {
        }
        public SelectedServerDataExtendedMessage(GameServerInformations[] servers)
        {
            this.servers = servers;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)servers.Length);
            for (uint _i1 = 0;_i1 < servers.Length;_i1++)
            {
                (servers[_i1] as GameServerInformations).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            GameServerInformations _item1 = null;
            base.Deserialize(reader);
            uint _serversLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _serversLen;_i1++)
            {
                _item1 = new GameServerInformations();
                _item1.Deserialize(reader);
                servers[_i1] = _item1;
            }

        }


    }
}








