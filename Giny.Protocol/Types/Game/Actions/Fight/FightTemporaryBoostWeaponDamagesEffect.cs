using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FightTemporaryBoostWeaponDamagesEffect : FightTemporaryBoostEffect  
    { 
        public const ushort Id = 3021;
        public override ushort TypeId => Id;

        public short weaponTypeId;

        public FightTemporaryBoostWeaponDamagesEffect()
        {
        }
        public FightTemporaryBoostWeaponDamagesEffect(short weaponTypeId)
        {
            this.weaponTypeId = weaponTypeId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)weaponTypeId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            weaponTypeId = (short)reader.ReadShort();
        }


    }
}








