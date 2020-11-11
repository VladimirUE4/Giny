using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class SlaveSwitchContextMessage : NetworkMessage  
    { 
        public new const ushort Id = 9736;
        public override ushort MessageId => Id;

        public double masterId;
        public double slaveId;
        public SpellItem[] slaveSpells;
        public CharacterCharacteristicsInformations slaveStats;
        public Shortcut[] shortcuts;

        public SlaveSwitchContextMessage()
        {
        }
        public SlaveSwitchContextMessage(double masterId,double slaveId,SpellItem[] slaveSpells,CharacterCharacteristicsInformations slaveStats,Shortcut[] shortcuts)
        {
            this.masterId = masterId;
            this.slaveId = slaveId;
            this.slaveSpells = slaveSpells;
            this.slaveStats = slaveStats;
            this.shortcuts = shortcuts;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (masterId < -9.00719925474099E+15 || masterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + masterId + ") on element masterId.");
            }

            writer.WriteDouble((double)masterId);
            if (slaveId < -9.00719925474099E+15 || slaveId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + slaveId + ") on element slaveId.");
            }

            writer.WriteDouble((double)slaveId);
            writer.WriteShort((short)slaveSpells.Length);
            for (uint _i3 = 0;_i3 < slaveSpells.Length;_i3++)
            {
                (slaveSpells[_i3] as SpellItem).Serialize(writer);
            }

            slaveStats.Serialize(writer);
            writer.WriteShort((short)shortcuts.Length);
            for (uint _i5 = 0;_i5 < shortcuts.Length;_i5++)
            {
                writer.WriteShort((short)(shortcuts[_i5] as Shortcut).TypeId);
                (shortcuts[_i5] as Shortcut).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            SpellItem _item3 = null;
            uint _id5 = 0;
            Shortcut _item5 = null;
            masterId = (double)reader.ReadDouble();
            if (masterId < -9.00719925474099E+15 || masterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + masterId + ") on element of SlaveSwitchContextMessage.masterId.");
            }

            slaveId = (double)reader.ReadDouble();
            if (slaveId < -9.00719925474099E+15 || slaveId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + slaveId + ") on element of SlaveSwitchContextMessage.slaveId.");
            }

            uint _slaveSpellsLen = (uint)reader.ReadUShort();
            for (uint _i3 = 0;_i3 < _slaveSpellsLen;_i3++)
            {
                _item3 = new SpellItem();
                _item3.Deserialize(reader);
                slaveSpells[_i3] = _item3;
            }

            slaveStats = new CharacterCharacteristicsInformations();
            slaveStats.Deserialize(reader);
            uint _shortcutsLen = (uint)reader.ReadUShort();
            for (uint _i5 = 0;_i5 < _shortcutsLen;_i5++)
            {
                _id5 = (uint)reader.ReadUShort();
                _item5 = ProtocolTypeManager.GetInstance<Shortcut>((short)_id5);
                _item5.Deserialize(reader);
                shortcuts[_i5] = _item5;
            }

        }


    }
}








