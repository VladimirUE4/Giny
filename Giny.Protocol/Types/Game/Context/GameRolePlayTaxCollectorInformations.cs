using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayTaxCollectorInformations : GameRolePlayActorInformations  
    { 
        public const ushort Id = 487;
        public override ushort TypeId => Id;

        public TaxCollectorStaticInformations identification;
        public byte guildLevel;
        public int taxCollectorAttack;

        public GameRolePlayTaxCollectorInformations()
        {
        }
        public GameRolePlayTaxCollectorInformations(TaxCollectorStaticInformations identification,byte guildLevel,int taxCollectorAttack)
        {
            this.identification = identification;
            this.guildLevel = guildLevel;
            this.taxCollectorAttack = taxCollectorAttack;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)identification.TypeId);
            identification.Serialize(writer);
            if (guildLevel < 0 || guildLevel > 255)
            {
                throw new Exception("Forbidden value (" + guildLevel + ") on element guildLevel.");
            }

            writer.WriteByte((byte)guildLevel);
            writer.WriteInt((int)taxCollectorAttack);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            uint _id1 = (uint)reader.ReadUShort();
            identification = ProtocolTypeManager.GetInstance<TaxCollectorStaticInformations>((short)_id1);
            identification.Deserialize(reader);
            guildLevel = (byte)reader.ReadSByte();
            if (guildLevel < 0 || guildLevel > 255)
            {
                throw new Exception("Forbidden value (" + guildLevel + ") on element of GameRolePlayTaxCollectorInformations.guildLevel.");
            }

            taxCollectorAttack = (int)reader.ReadInt();
        }


    }
}








