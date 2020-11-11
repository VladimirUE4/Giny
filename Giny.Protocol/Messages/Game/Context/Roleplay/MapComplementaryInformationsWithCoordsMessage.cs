using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MapComplementaryInformationsWithCoordsMessage : MapComplementaryInformationsDataMessage  
    { 
        public new const ushort Id = 8121;
        public override ushort MessageId => Id;

        public short worldX;
        public short worldY;

        public MapComplementaryInformationsWithCoordsMessage()
        {
        }
        public MapComplementaryInformationsWithCoordsMessage(short worldX,short worldY)
        {
            this.worldX = worldX;
            this.worldY = worldY;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (worldX < -255 || worldX > 255)
            {
                throw new Exception("Forbidden value (" + worldX + ") on element worldX.");
            }

            writer.WriteShort((short)worldX);
            if (worldY < -255 || worldY > 255)
            {
                throw new Exception("Forbidden value (" + worldY + ") on element worldY.");
            }

            writer.WriteShort((short)worldY);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            worldX = (short)reader.ReadShort();
            if (worldX < -255 || worldX > 255)
            {
                throw new Exception("Forbidden value (" + worldX + ") on element of MapComplementaryInformationsWithCoordsMessage.worldX.");
            }

            worldY = (short)reader.ReadShort();
            if (worldY < -255 || worldY > 255)
            {
                throw new Exception("Forbidden value (" + worldY + ") on element of MapComplementaryInformationsWithCoordsMessage.worldY.");
            }

        }


    }
}








