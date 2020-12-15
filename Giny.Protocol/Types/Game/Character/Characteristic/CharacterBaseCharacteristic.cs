using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class CharacterBaseCharacteristic  
    { 
        public const ushort Id = 5795;
        public virtual ushort TypeId => Id;

        public short @base;
        public short additionnal;
        public short objectsAndMountBonus;
        public short alignGiftBonus;
        public short contextModif;

        public CharacterBaseCharacteristic()
        {
        }
        public CharacterBaseCharacteristic(short @base, short additionnal,short objectsAndMountBonus,short alignGiftBonus,short contextModif)
        {
            this.@base = @base;
            this.additionnal = additionnal;
            this.objectsAndMountBonus = objectsAndMountBonus;
            this.alignGiftBonus = alignGiftBonus;
            this.contextModif = contextModif;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort((short)@base);
            writer.WriteVarShort((short)additionnal);
            writer.WriteVarShort((short)objectsAndMountBonus);
            writer.WriteVarShort((short)alignGiftBonus);
            writer.WriteVarShort((short)contextModif);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            @base = (short)reader.ReadVarShort();
            additionnal = (short)reader.ReadVarShort();
            objectsAndMountBonus = (short)reader.ReadVarShort();
            alignGiftBonus = (short)reader.ReadVarShort();
            contextModif = (short)reader.ReadVarShort();
        }


    }
}








