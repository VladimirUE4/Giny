using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class IgnoredOnlineInformations : IgnoredInformations  
    { 
        public const ushort Id = 8684;
        public override ushort TypeId => Id;

        public long playerId;
        public string playerName;
        public byte breed;
        public bool sex;

        public IgnoredOnlineInformations()
        {
        }
        public IgnoredOnlineInformations(long playerId,string playerName,byte breed,bool sex)
        {
            this.playerId = playerId;
            this.playerName = playerName;
            this.breed = breed;
            this.sex = sex;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (playerId < 0 || playerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + playerId + ") on element playerId.");
            }

            writer.WriteVarLong((long)playerId);
            writer.WriteUTF((string)playerName);
            writer.WriteByte((byte)breed);
            writer.WriteBoolean((bool)sex);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = (long)reader.ReadVarUhLong();
            if (playerId < 0 || playerId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + playerId + ") on element of IgnoredOnlineInformations.playerId.");
            }

            playerName = (string)reader.ReadUTF();
            breed = (byte)reader.ReadByte();
            if (breed < (byte)PlayableBreedEnum.Feca || breed > (byte)PlayableBreedEnum.Ouginak)
            {
                throw new Exception("Forbidden value (" + breed + ") on element of IgnoredOnlineInformations.breed.");
            }

            sex = (bool)reader.ReadBoolean();
        }


    }
}








