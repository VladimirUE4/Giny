using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class HouseInformationsForSell  
    { 
        public const ushort Id = 432;
        public virtual ushort TypeId => Id;

        public int instanceId;
        public bool secondHand;
        public int modelId;
        public string ownerName;
        public string ownerCharacterName;
        public short worldX;
        public short worldY;
        public short subAreaId;
        public byte nbRoom;
        public byte nbChest;
        public int[] skillListIds;
        public bool isLocked;
        public long price;

        public HouseInformationsForSell()
        {
        }
        public HouseInformationsForSell(int instanceId,bool secondHand,int modelId,string ownerName,string ownerCharacterName,short worldX,short worldY,short subAreaId,byte nbRoom,byte nbChest,int[] skillListIds,bool isLocked,long price)
        {
            this.instanceId = instanceId;
            this.secondHand = secondHand;
            this.modelId = modelId;
            this.ownerName = ownerName;
            this.ownerCharacterName = ownerCharacterName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.subAreaId = subAreaId;
            this.nbRoom = nbRoom;
            this.nbChest = nbChest;
            this.skillListIds = skillListIds;
            this.isLocked = isLocked;
            this.price = price;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            if (instanceId < 0)
            {
                throw new Exception("Forbidden value (" + instanceId + ") on element instanceId.");
            }

            writer.WriteInt((int)instanceId);
            writer.WriteBoolean((bool)secondHand);
            if (modelId < 0)
            {
                throw new Exception("Forbidden value (" + modelId + ") on element modelId.");
            }

            writer.WriteVarInt((int)modelId);
            writer.WriteUTF((string)ownerName);
            writer.WriteUTF((string)ownerCharacterName);
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
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element subAreaId.");
            }

            writer.WriteVarShort((short)subAreaId);
            writer.WriteByte((byte)nbRoom);
            writer.WriteByte((byte)nbChest);
            writer.WriteShort((short)skillListIds.Length);
            for (uint _i11 = 0;_i11 < skillListIds.Length;_i11++)
            {
                writer.WriteInt((int)skillListIds[_i11]);
            }

            writer.WriteBoolean((bool)isLocked);
            if (price < 0 || price > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + price + ") on element price.");
            }

            writer.WriteVarLong((long)price);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            int _val11 = 0;
            instanceId = (int)reader.ReadInt();
            if (instanceId < 0)
            {
                throw new Exception("Forbidden value (" + instanceId + ") on element of HouseInformationsForSell.instanceId.");
            }

            secondHand = (bool)reader.ReadBoolean();
            modelId = (int)reader.ReadVarUhInt();
            if (modelId < 0)
            {
                throw new Exception("Forbidden value (" + modelId + ") on element of HouseInformationsForSell.modelId.");
            }

            ownerName = (string)reader.ReadUTF();
            ownerCharacterName = (string)reader.ReadUTF();
            worldX = (short)reader.ReadShort();
            if (worldX < -255 || worldX > 255)
            {
                throw new Exception("Forbidden value (" + worldX + ") on element of HouseInformationsForSell.worldX.");
            }

            worldY = (short)reader.ReadShort();
            if (worldY < -255 || worldY > 255)
            {
                throw new Exception("Forbidden value (" + worldY + ") on element of HouseInformationsForSell.worldY.");
            }

            subAreaId = (short)reader.ReadVarUhShort();
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element of HouseInformationsForSell.subAreaId.");
            }

            nbRoom = (byte)reader.ReadByte();
            nbChest = (byte)reader.ReadByte();
            uint _skillListIdsLen = (uint)reader.ReadUShort();
            skillListIds = new int[_skillListIdsLen];
            for (uint _i11 = 0;_i11 < _skillListIdsLen;_i11++)
            {
                _val11 = (int)reader.ReadInt();
                skillListIds[_i11] = (int)_val11;
            }

            isLocked = (bool)reader.ReadBoolean();
            price = (long)reader.ReadVarUhLong();
            if (price < 0 || price > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + price + ") on element of HouseInformationsForSell.price.");
            }

        }


    }
}








