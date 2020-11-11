using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class PartyInvitationMemberInformations : CharacterBaseInformations  
    { 
        public const ushort Id = 5851;
        public override ushort TypeId => Id;

        public short worldX;
        public short worldY;
        public double mapId;
        public short subAreaId;
        public PartyEntityBaseInformation[] entities;

        public PartyInvitationMemberInformations()
        {
        }
        public PartyInvitationMemberInformations(short worldX,short worldY,double mapId,short subAreaId,PartyEntityBaseInformation[] entities)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.entities = entities;
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
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element subAreaId.");
            }

            writer.WriteVarShort((short)subAreaId);
            writer.WriteShort((short)entities.Length);
            for (uint _i5 = 0;_i5 < entities.Length;_i5++)
            {
                (entities[_i5] as PartyEntityBaseInformation).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            PartyEntityBaseInformation _item5 = null;
            base.Deserialize(reader);
            worldX = (short)reader.ReadShort();
            if (worldX < -255 || worldX > 255)
            {
                throw new Exception("Forbidden value (" + worldX + ") on element of PartyInvitationMemberInformations.worldX.");
            }

            worldY = (short)reader.ReadShort();
            if (worldY < -255 || worldY > 255)
            {
                throw new Exception("Forbidden value (" + worldY + ") on element of PartyInvitationMemberInformations.worldY.");
            }

            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of PartyInvitationMemberInformations.mapId.");
            }

            subAreaId = (short)reader.ReadVarUhShort();
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element of PartyInvitationMemberInformations.subAreaId.");
            }

            uint _entitiesLen = (uint)reader.ReadUShort();
            for (uint _i5 = 0;_i5 < _entitiesLen;_i5++)
            {
                _item5 = new PartyEntityBaseInformation();
                _item5.Deserialize(reader);
                entities[_i5] = _item5;
            }

        }


    }
}








