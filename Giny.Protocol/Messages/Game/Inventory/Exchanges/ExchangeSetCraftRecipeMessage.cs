using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeSetCraftRecipeMessage : NetworkMessage  
    { 
        public new const ushort Id = 49;
        public override ushort MessageId => Id;

        public short objectGID;

        public ExchangeSetCraftRecipeMessage()
        {
        }
        public ExchangeSetCraftRecipeMessage(short objectGID)
        {
            this.objectGID = objectGID;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (objectGID < 0)
            {
                throw new Exception("Forbidden value (" + objectGID + ") on element objectGID.");
            }

            writer.WriteVarShort((short)objectGID);
        }
        public override void Deserialize(IDataReader reader)
        {
            objectGID = (short)reader.ReadVarUhShort();
            if (objectGID < 0)
            {
                throw new Exception("Forbidden value (" + objectGID + ") on element of ExchangeSetCraftRecipeMessage.objectGID.");
            }

        }


    }
}








