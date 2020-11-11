using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MapComplementaryInformationsDataInHavenBagMessage : MapComplementaryInformationsDataMessage  
    { 
        public new const ushort Id = 3465;
        public override ushort MessageId => Id;

        public CharacterMinimalInformations ownerInformations;
        public byte theme;
        public byte roomId;
        public byte maxRoomId;

        public MapComplementaryInformationsDataInHavenBagMessage()
        {
        }
        public MapComplementaryInformationsDataInHavenBagMessage(CharacterMinimalInformations ownerInformations,byte theme,byte roomId,byte maxRoomId)
        {
            this.ownerInformations = ownerInformations;
            this.theme = theme;
            this.roomId = roomId;
            this.maxRoomId = maxRoomId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            ownerInformations.Serialize(writer);
            writer.WriteByte((byte)theme);
            if (roomId < 0)
            {
                throw new Exception("Forbidden value (" + roomId + ") on element roomId.");
            }

            writer.WriteByte((byte)roomId);
            if (maxRoomId < 0)
            {
                throw new Exception("Forbidden value (" + maxRoomId + ") on element maxRoomId.");
            }

            writer.WriteByte((byte)maxRoomId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            ownerInformations = new CharacterMinimalInformations();
            ownerInformations.Deserialize(reader);
            theme = (byte)reader.ReadByte();
            roomId = (byte)reader.ReadByte();
            if (roomId < 0)
            {
                throw new Exception("Forbidden value (" + roomId + ") on element of MapComplementaryInformationsDataInHavenBagMessage.roomId.");
            }

            maxRoomId = (byte)reader.ReadByte();
            if (maxRoomId < 0)
            {
                throw new Exception("Forbidden value (" + maxRoomId + ") on element of MapComplementaryInformationsDataInHavenBagMessage.maxRoomId.");
            }

        }


    }
}








