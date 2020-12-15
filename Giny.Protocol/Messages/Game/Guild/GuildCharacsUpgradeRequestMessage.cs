using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildCharacsUpgradeRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 6711;
        public override ushort MessageId => Id;

        public byte charaTypeTarget;

        public GuildCharacsUpgradeRequestMessage()
        {
        }
        public GuildCharacsUpgradeRequestMessage(byte charaTypeTarget)
        {
            this.charaTypeTarget = charaTypeTarget;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte((byte)charaTypeTarget);
        }
        public override void Deserialize(IDataReader reader)
        {
            charaTypeTarget = (byte)reader.ReadByte();
            if (charaTypeTarget < 0)
            {
                throw new Exception("Forbidden value (" + charaTypeTarget + ") on element of GuildCharacsUpgradeRequestMessage.charaTypeTarget.");
            }

        }


    }
}








