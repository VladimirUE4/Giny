using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyMemberInBreachFightMessage : AbstractPartyMemberInFightMessage  
    { 
        public new const ushort Id = 3143;
        public override ushort MessageId => Id;

        public int floor;
        public byte room;

        public PartyMemberInBreachFightMessage()
        {
        }
        public PartyMemberInBreachFightMessage(int floor,byte room)
        {
            this.floor = floor;
            this.room = room;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (floor < 0)
            {
                throw new Exception("Forbidden value (" + floor + ") on element floor.");
            }

            writer.WriteVarInt((int)floor);
            if (room < 0)
            {
                throw new Exception("Forbidden value (" + room + ") on element room.");
            }

            writer.WriteByte((byte)room);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            floor = (int)reader.ReadVarUhInt();
            if (floor < 0)
            {
                throw new Exception("Forbidden value (" + floor + ") on element of PartyMemberInBreachFightMessage.floor.");
            }

            room = (byte)reader.ReadByte();
            if (room < 0)
            {
                throw new Exception("Forbidden value (" + room + ") on element of PartyMemberInBreachFightMessage.room.");
            }

        }


    }
}








