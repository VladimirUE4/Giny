using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class CharacterCreationRequestMessage : NetworkMessage  
    { 
        public new const ushort Id = 1633;
        public override ushort MessageId => Id;

        public string name;
        public byte breed;
        public bool sex;
        public int[] colors;
        public short cosmeticId;

        public CharacterCreationRequestMessage()
        {
        }
        public CharacterCreationRequestMessage(string name,byte breed,bool sex,int[] colors,short cosmeticId)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.colors = colors;
            this.cosmeticId = cosmeticId;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF((string)name);
            writer.WriteByte((byte)breed);
            writer.WriteBoolean((bool)sex);
            for (uint _i4 = 0;_i4 < 5;_i4++)
            {
                writer.WriteInt((int)colors[_i4]);
            }

            if (cosmeticId < 0)
            {
                throw new Exception("Forbidden value (" + cosmeticId + ") on element cosmeticId.");
            }

            writer.WriteVarShort((short)cosmeticId);
        }
        public override void Deserialize(IDataReader reader)
        {
            name = (string)reader.ReadUTF();
            breed = (byte)reader.ReadByte();
            if (breed < (byte)PlayableBreedEnum.Feca || breed > (byte)PlayableBreedEnum.Ouginak)
            {
                throw new Exception("Forbidden value (" + breed + ") on element of CharacterCreationRequestMessage.breed.");
            }

            colors = new int[5];

            sex = (bool)reader.ReadBoolean();
            for (uint _i4 = 0;_i4 < 5;_i4++)
            {
                colors[_i4] = reader.ReadInt();
            }

            cosmeticId = (short)reader.ReadVarUhShort();
            if (cosmeticId < 0)
            {
                throw new Exception("Forbidden value (" + cosmeticId + ") on element of CharacterCreationRequestMessage.cosmeticId.");
            }

        }


    }
}








