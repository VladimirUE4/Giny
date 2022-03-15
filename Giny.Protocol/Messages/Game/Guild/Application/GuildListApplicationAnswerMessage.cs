using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildListApplicationAnswerMessage : PaginationAnswerAbstractMessage  
    { 
        public  const ushort Id = 1223;
        public override ushort MessageId => Id;

        public GuildApplicationInformation[] applies;

        public GuildListApplicationAnswerMessage()
        {
        }
        public GuildListApplicationAnswerMessage(GuildApplicationInformation[] applies)
        {
            this.applies = applies;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)applies.Length);
            for (uint _i1 = 0;_i1 < applies.Length;_i1++)
            {
                (applies[_i1] as GuildApplicationInformation).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            GuildApplicationInformation _item1 = null;
            base.Deserialize(reader);
            uint _appliesLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _appliesLen;_i1++)
            {
                _item1 = new GuildApplicationInformation();
                _item1.Deserialize(reader);
                applies[_i1] = _item1;
            }

        }


    }
}








