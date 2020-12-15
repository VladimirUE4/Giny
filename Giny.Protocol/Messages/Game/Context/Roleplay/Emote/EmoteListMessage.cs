using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class EmoteListMessage : NetworkMessage  
    { 
        public new const ushort Id = 4861;
        public override ushort MessageId => Id;

        public byte[] emoteIds;

        public EmoteListMessage()
        {
        }
        public EmoteListMessage(byte[] emoteIds)
        {
            this.emoteIds = emoteIds;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)emoteIds.Length);
            for (uint _i1 = 0;_i1 < emoteIds.Length;_i1++)
            {
                if (emoteIds[_i1] < 0 || emoteIds[_i1] > 255)
                {
                    throw new Exception("Forbidden value (" + emoteIds[_i1] + ") on element 1 (starting at 1) of emoteIds.");
                }

                writer.WriteByte((byte)emoteIds[_i1]);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            uint _val1 = 0;
            uint _emoteIdsLen = (uint)reader.ReadUShort();
            emoteIds = new byte[_emoteIdsLen];
            for (uint _i1 = 0;_i1 < _emoteIdsLen;_i1++)
            {
                _val1 = (uint)reader.ReadSByte();
                if (_val1 < 0 || _val1 > 255)
                {
                    throw new Exception("Forbidden value (" + _val1 + ") on elements of emoteIds.");
                }

                emoteIds[_i1] = (byte)_val1;
            }

        }


    }
}








