using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeMultiCraftCrafterCanUseHisRessourcesMessage : NetworkMessage  
    { 
        public new const ushort Id = 264;
        public override ushort MessageId => Id;

        public bool allowed;

        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage()
        {
        }
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(bool allowed)
        {
            this.allowed = allowed;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean((bool)allowed);
        }
        public override void Deserialize(IDataReader reader)
        {
            allowed = (bool)reader.ReadBoolean();
        }


    }
}








