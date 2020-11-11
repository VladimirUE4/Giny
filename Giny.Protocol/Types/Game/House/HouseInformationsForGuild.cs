using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class HouseInformationsForGuild : HouseInformations  
    { 
        public const ushort Id = 3959;
        public override ushort TypeId => Id;

        public int instanceId;
        public bool secondHand;
        public string ownerName;
        public short worldX;
        public short worldY;
        public double mapId;
        public short subAreaId;
        public int[] skillListIds;
        public int guildshareParams;

        public HouseInformationsForGuild()
        {
        }
        public HouseInformationsForGuild(int instanceId,bool secondHand,string ownerName,short worldX,short worldY,double mapId,short subAreaId,int[] skillListIds,int guildshareParams)
        {
            this.instanceId = instanceId;
            this.secondHand = secondHand;
            this.ownerName = ownerName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.skillListIds = skillListIds;
            this.guildshareParams = guildshareParams;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (instanceId < 0)
            {
                throw new Exception("Forbidden value (" + instanceId + ") on element instanceId.");
            }

            writer.WriteInt((int)instanceId);
            writer.WriteBoolean((bool)secondHand);
            writer.WriteUTF((string)ownerName);
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
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element subAreaId.");
            }

            writer.WriteVarShort((short)subAreaId);
            writer.WriteShort((short)skillListIds.Length);
            for (uint _i8 = 0;_i8 < skillListIds.Length;_i8++)
            {
                writer.WriteInt((int)skillListIds[_i8]);
            }

            if (guildshareParams < 0)
            {
                throw new Exception("Forbidden value (" + guildshareParams + ") on element guildshareParams.");
            }

            writer.WriteVarInt((int)guildshareParams);
        }
        public override void Deserialize(IDataReader reader)
        {
            int _val8 = 0;
            base.Deserialize(reader);
            instanceId = (int)reader.ReadInt();
            if (instanceId < 0)
            {
                throw new Exception("Forbidden value (" + instanceId + ") on element of HouseInformationsForGuild.instanceId.");
            }

            secondHand = (bool)reader.ReadBoolean();
            ownerName = (string)reader.ReadUTF();
            worldX = (short)reader.ReadShort();
            if (worldX < -255 || worldX > 255)
            {
                throw new Exception("Forbidden value (" + worldX + ") on element of HouseInformationsForGuild.worldX.");
            }

            worldY = (short)reader.ReadShort();
            if (worldY < -255 || worldY > 255)
            {
                throw new Exception("Forbidden value (" + worldY + ") on element of HouseInformationsForGuild.worldY.");
            }

            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of HouseInformationsForGuild.mapId.");
            }

            subAreaId = (short)reader.ReadVarUhShort();
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element of HouseInformationsForGuild.subAreaId.");
            }

            uint _skillListIdsLen = (uint)reader.ReadUShort();
            skillListIds = new int[_skillListIdsLen];
            for (uint _i8 = 0;_i8 < _skillListIdsLen;_i8++)
            {
                _val8 = (int)reader.ReadInt();
                skillListIds[_i8] = (int)_val8;
            }

            guildshareParams = (int)reader.ReadVarUhInt();
            if (guildshareParams < 0)
            {
                throw new Exception("Forbidden value (" + guildshareParams + ") on element of HouseInformationsForGuild.guildshareParams.");
            }

        }


    }
}








