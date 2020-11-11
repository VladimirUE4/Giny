using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MapNpcsQuestStatusUpdateMessage : NetworkMessage  
    { 
        public new const ushort Id = 1168;
        public override ushort MessageId => Id;

        public double mapId;
        public int[] npcsIdsWithQuest;
        public GameRolePlayNpcQuestFlag[] questFlags;
        public int[] npcsIdsWithoutQuest;

        public MapNpcsQuestStatusUpdateMessage()
        {
        }
        public MapNpcsQuestStatusUpdateMessage(double mapId,int[] npcsIdsWithQuest,GameRolePlayNpcQuestFlag[] questFlags,int[] npcsIdsWithoutQuest)
        {
            this.mapId = mapId;
            this.npcsIdsWithQuest = npcsIdsWithQuest;
            this.questFlags = questFlags;
            this.npcsIdsWithoutQuest = npcsIdsWithoutQuest;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element mapId.");
            }

            writer.WriteDouble((double)mapId);
            writer.WriteShort((short)npcsIdsWithQuest.Length);
            for (uint _i2 = 0;_i2 < npcsIdsWithQuest.Length;_i2++)
            {
                writer.WriteInt((int)npcsIdsWithQuest[_i2]);
            }

            writer.WriteShort((short)questFlags.Length);
            for (uint _i3 = 0;_i3 < questFlags.Length;_i3++)
            {
                (questFlags[_i3] as GameRolePlayNpcQuestFlag).Serialize(writer);
            }

            writer.WriteShort((short)npcsIdsWithoutQuest.Length);
            for (uint _i4 = 0;_i4 < npcsIdsWithoutQuest.Length;_i4++)
            {
                writer.WriteInt((int)npcsIdsWithoutQuest[_i4]);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            int _val2 = 0;
            GameRolePlayNpcQuestFlag _item3 = null;
            int _val4 = 0;
            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of MapNpcsQuestStatusUpdateMessage.mapId.");
            }

            uint _npcsIdsWithQuestLen = (uint)reader.ReadUShort();
            npcsIdsWithQuest = new int[_npcsIdsWithQuestLen];
            for (uint _i2 = 0;_i2 < _npcsIdsWithQuestLen;_i2++)
            {
                _val2 = (int)reader.ReadInt();
                npcsIdsWithQuest[_i2] = (int)_val2;
            }

            uint _questFlagsLen = (uint)reader.ReadUShort();
            for (uint _i3 = 0;_i3 < _questFlagsLen;_i3++)
            {
                _item3 = new GameRolePlayNpcQuestFlag();
                _item3.Deserialize(reader);
                questFlags[_i3] = _item3;
            }

            uint _npcsIdsWithoutQuestLen = (uint)reader.ReadUShort();
            npcsIdsWithoutQuest = new int[_npcsIdsWithoutQuestLen];
            for (uint _i4 = 0;_i4 < _npcsIdsWithoutQuestLen;_i4++)
            {
                _val4 = (int)reader.ReadInt();
                npcsIdsWithoutQuest[_i4] = (int)_val4;
            }

        }


    }
}








