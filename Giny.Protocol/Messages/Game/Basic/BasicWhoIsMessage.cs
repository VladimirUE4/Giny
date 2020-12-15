using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class BasicWhoIsMessage : NetworkMessage  
    { 
        public new const ushort Id = 2658;
        public override ushort MessageId => Id;

        public bool self;
        public byte position;
        public string accountNickname;
        public int accountId;
        public string playerName;
        public long playerId;
        public short areaId;
        public short serverId;
        public short originServerId;
        public AbstractSocialGroupInfos[] socialGroups;
        public bool verbose;
        public byte playerState;

        public BasicWhoIsMessage()
        {
        }
        public BasicWhoIsMessage(bool self,byte position,string accountNickname,int accountId,string playerName,long playerId,short areaId,short serverId,short originServerId,AbstractSocialGroupInfos[] socialGroups,bool verbose,byte playerState)
        {
            this.self = self;
            this.position = position;
            this.accountNickname = accountNickname;
            this.accountId = accountId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.areaId = areaId;
            this.serverId = serverId;
            this.originServerId = originServerId;
            this.socialGroups = socialGroups;
            this.verbose = verbose;
            this.playerState = playerState;
        }
        public override void Serialize(IDataWriter writer)
        {
            byte _box0 = 0;
            _box0 = BooleanByteWrapper.SetFlag(_box0,0,self);
            _box0 = BooleanByteWrapper.SetFlag(_box0,1,verbose);
            writer.WriteByte((byte)_box0);
            writer.WriteByte((byte)position);
            writer.WriteUTF((string)accountNickname);
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element accountId.");
            }

            writer.WriteInt((int)accountId);
            writer.WriteUTF((string)playerName);
            if (playerId < 0 || playerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + playerId + ") on element playerId.");
            }

            writer.WriteVarLong((long)playerId);
            writer.WriteShort((short)areaId);
            writer.WriteShort((short)serverId);
            writer.WriteShort((short)originServerId);
            writer.WriteShort((short)socialGroups.Length);
            for (uint _i10 = 0;_i10 < socialGroups.Length;_i10++)
            {
                writer.WriteShort((short)(socialGroups[_i10] as AbstractSocialGroupInfos).TypeId);
                (socialGroups[_i10] as AbstractSocialGroupInfos).Serialize(writer);
            }

            writer.WriteByte((byte)playerState);
        }
        public override void Deserialize(IDataReader reader)
        {
            uint _id10 = 0;
            AbstractSocialGroupInfos _item10 = null;
            byte _box0 = reader.ReadByte();
            self = BooleanByteWrapper.GetFlag(_box0,0);
            verbose = BooleanByteWrapper.GetFlag(_box0,1);
            position = (byte)reader.ReadByte();
            accountNickname = (string)reader.ReadUTF();
            accountId = (int)reader.ReadInt();
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element of BasicWhoIsMessage.accountId.");
            }

            playerName = (string)reader.ReadUTF();
            playerId = (long)reader.ReadVarUhLong();
            if (playerId < 0 || playerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + playerId + ") on element of BasicWhoIsMessage.playerId.");
            }

            areaId = (short)reader.ReadShort();
            serverId = (short)reader.ReadShort();
            originServerId = (short)reader.ReadShort();
            uint _socialGroupsLen = (uint)reader.ReadUShort();
            for (uint _i10 = 0;_i10 < _socialGroupsLen;_i10++)
            {
                _id10 = (uint)reader.ReadUShort();
                _item10 = ProtocolTypeManager.GetInstance<AbstractSocialGroupInfos>((short)_id10);
                _item10.Deserialize(reader);
                socialGroups[_i10] = _item10;
            }

            playerState = (byte)reader.ReadByte();
            if (playerState < 0)
            {
                throw new Exception("Forbidden value (" + playerState + ") on element of BasicWhoIsMessage.playerState.");
            }

        }


    }
}








