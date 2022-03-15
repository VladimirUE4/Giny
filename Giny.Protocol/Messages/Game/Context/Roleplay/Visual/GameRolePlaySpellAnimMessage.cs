using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameRolePlaySpellAnimMessage : NetworkMessage  
    { 
        public  const ushort Id = 8430;
        public override ushort MessageId => Id;

        public long casterId;
        public short targetCellId;
        public short spellId;
        public short spellLevel;

        public GameRolePlaySpellAnimMessage()
        {
        }
        public GameRolePlaySpellAnimMessage(long casterId,short targetCellId,short spellId,short spellLevel)
        {
            this.casterId = casterId;
            this.targetCellId = targetCellId;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (casterId < 0 || casterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + casterId + ") on element casterId.");
            }

            writer.WriteVarLong((long)casterId);
            if (targetCellId < 0 || targetCellId > 559)
            {
                throw new Exception("Forbidden value (" + targetCellId + ") on element targetCellId.");
            }

            writer.WriteVarShort((short)targetCellId);
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element spellId.");
            }

            writer.WriteVarShort((short)spellId);
            if (spellLevel < 1 || spellLevel > 32767)
            {
                throw new Exception("Forbidden value (" + spellLevel + ") on element spellLevel.");
            }

            writer.WriteShort((short)spellLevel);
        }
        public override void Deserialize(IDataReader reader)
        {
            casterId = (long)reader.ReadVarUhLong();
            if (casterId < 0 || casterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + casterId + ") on element of GameRolePlaySpellAnimMessage.casterId.");
            }

            targetCellId = (short)reader.ReadVarUhShort();
            if (targetCellId < 0 || targetCellId > 559)
            {
                throw new Exception("Forbidden value (" + targetCellId + ") on element of GameRolePlaySpellAnimMessage.targetCellId.");
            }

            spellId = (short)reader.ReadVarUhShort();
            if (spellId < 0)
            {
                throw new Exception("Forbidden value (" + spellId + ") on element of GameRolePlaySpellAnimMessage.spellId.");
            }

            spellLevel = (short)reader.ReadShort();
            if (spellLevel < 1 || spellLevel > 32767)
            {
                throw new Exception("Forbidden value (" + spellLevel + ") on element of GameRolePlaySpellAnimMessage.spellLevel.");
            }

        }


    }
}








