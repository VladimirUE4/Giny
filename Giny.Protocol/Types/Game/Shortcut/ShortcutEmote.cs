using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ShortcutEmote : Shortcut  
    { 
        public const ushort Id = 7079;
        public override ushort TypeId => Id;

        public byte emoteId;

        public ShortcutEmote()
        {
        }
        public ShortcutEmote(byte emoteId)
        {
            this.emoteId = emoteId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (emoteId < 0 || emoteId > 255)
            {
                throw new Exception("Forbidden value (" + emoteId + ") on element emoteId.");
            }

            writer.WriteByte((byte)emoteId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            emoteId = (byte)reader.ReadSByte();
            if (emoteId < 0 || emoteId > 255)
            {
                throw new Exception("Forbidden value (" + emoteId + ") on element of ShortcutEmote.emoteId.");
            }

        }


    }
}








