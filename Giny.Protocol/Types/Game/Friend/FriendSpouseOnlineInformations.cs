using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FriendSpouseOnlineInformations : FriendSpouseInformations  
    { 
        public const ushort Id = 1910;
        public override ushort TypeId => Id;

        public double mapId;
        public short subAreaId;
        public bool inFight;
        public bool followSpouse;

        public FriendSpouseOnlineInformations()
        {
        }
        public FriendSpouseOnlineInformations(double mapId,short subAreaId,bool inFight,bool followSpouse)
        {
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.inFight = inFight;
            this.followSpouse = followSpouse;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            byte _box0 = 0;
            _box0 = BooleanByteWrapper.SetFlag(_box0,0,inFight);
            _box0 = BooleanByteWrapper.SetFlag(_box0,1,followSpouse);
            writer.WriteByte((byte)_box0);
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
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            byte _box0 = reader.ReadByte();
            inFight = BooleanByteWrapper.GetFlag(_box0,0);
            followSpouse = BooleanByteWrapper.GetFlag(_box0,1);
            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of FriendSpouseOnlineInformations.mapId.");
            }

            subAreaId = (short)reader.ReadVarUhShort();
            if (subAreaId < 0)
            {
                throw new Exception("Forbidden value (" + subAreaId + ") on element of FriendSpouseOnlineInformations.subAreaId.");
            }

        }


    }
}








