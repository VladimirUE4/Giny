using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameActionFightCloseCombatMessage : AbstractGameActionFightTargetedAbilityMessage  
    { 
        public new const ushort Id = 1677;
        public override ushort MessageId => Id;

        public short weaponGenericId;

        public GameActionFightCloseCombatMessage()
        {
        }
        public GameActionFightCloseCombatMessage(short weaponGenericId)
        {
            this.weaponGenericId = weaponGenericId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (weaponGenericId < 0)
            {
                throw new Exception("Forbidden value (" + weaponGenericId + ") on element weaponGenericId.");
            }

            writer.WriteVarShort((short)weaponGenericId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            weaponGenericId = (short)reader.ReadVarUhShort();
            if (weaponGenericId < 0)
            {
                throw new Exception("Forbidden value (" + weaponGenericId + ") on element of GameActionFightCloseCombatMessage.weaponGenericId.");
            }

        }


    }
}








