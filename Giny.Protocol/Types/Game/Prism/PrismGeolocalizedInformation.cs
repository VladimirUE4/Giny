using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class PrismGeolocalizedInformation : PrismSubareaEmptyInfo  
    { 
        public const ushort Id = 6199;
        public override ushort TypeId => Id;

        public short worldX;
        public short worldY;
        public double mapId;
        public PrismInformation prism;

        public PrismGeolocalizedInformation()
        {
        }
        public PrismGeolocalizedInformation(short worldX,short worldY,double mapId,PrismInformation prism)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.prism = prism;
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
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element mapId.");
            }

            writer.WriteDouble((double)mapId);
            writer.WriteShort((short)prism.TypeId);
            prism.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            worldX = (short)reader.ReadShort();
            if (worldX < -255 || worldX > 255)
            {
                throw new Exception("Forbidden value (" + worldX + ") on element of PrismGeolocalizedInformation.worldX.");
            }

            worldY = (short)reader.ReadShort();
            if (worldY < -255 || worldY > 255)
            {
                throw new Exception("Forbidden value (" + worldY + ") on element of PrismGeolocalizedInformation.worldY.");
            }

            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of PrismGeolocalizedInformation.mapId.");
            }

            uint _id4 = (uint)reader.ReadUShort();
            prism = ProtocolTypeManager.GetInstance<PrismInformation>((short)_id4);
            prism.Deserialize(reader);
        }


    }
}








