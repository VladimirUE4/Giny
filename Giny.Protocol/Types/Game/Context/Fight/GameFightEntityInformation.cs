using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightEntityInformation : GameFightFighterInformations  
    { 
        public const ushort Id = 5544;
        public override ushort TypeId => Id;

        public byte entityModelId;
        public short level;
        public double masterId;

        public GameFightEntityInformation()
        {
        }
        public GameFightEntityInformation(byte entityModelId,short level,double masterId)
        {
            this.entityModelId = entityModelId;
            this.level = level;
            this.masterId = masterId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (entityModelId < 0)
            {
                throw new Exception("Forbidden value (" + entityModelId + ") on element entityModelId.");
            }

            writer.WriteByte((byte)entityModelId);
            if (level < 1 || level > 200)
            {
                throw new Exception("Forbidden value (" + level + ") on element level.");
            }

            writer.WriteVarShort((short)level);
            if (masterId < -9.00719925474099E+15 || masterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + masterId + ") on element masterId.");
            }

            writer.WriteDouble((double)masterId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            entityModelId = (byte)reader.ReadByte();
            if (entityModelId < 0)
            {
                throw new Exception("Forbidden value (" + entityModelId + ") on element of GameFightEntityInformation.entityModelId.");
            }

            level = (short)reader.ReadVarUhShort();
            if (level < 1 || level > 200)
            {
                throw new Exception("Forbidden value (" + level + ") on element of GameFightEntityInformation.level.");
            }

            masterId = (double)reader.ReadDouble();
            if (masterId < -9.00719925474099E+15 || masterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + masterId + ") on element of GameFightEntityInformation.masterId.");
            }

        }


    }
}








