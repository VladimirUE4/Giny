using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MapFightCountMessage : NetworkMessage  
    { 
        public new const ushort Id = 7990;
        public override ushort MessageId => Id;

        public short fightCount;

        public MapFightCountMessage()
        {
        }
        public MapFightCountMessage(short fightCount)
        {
            this.fightCount = fightCount;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (fightCount < 0)
            {
                throw new Exception("Forbidden value (" + fightCount + ") on element fightCount.");
            }

            writer.WriteVarShort((short)fightCount);
        }
        public override void Deserialize(IDataReader reader)
        {
            fightCount = (short)reader.ReadVarUhShort();
            if (fightCount < 0)
            {
                throw new Exception("Forbidden value (" + fightCount + ") on element of MapFightCountMessage.fightCount.");
            }

        }


    }
}








