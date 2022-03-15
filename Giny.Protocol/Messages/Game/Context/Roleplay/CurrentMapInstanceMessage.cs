using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class CurrentMapInstanceMessage : CurrentMapMessage  
    { 
        public  const ushort Id = 7422;
        public override ushort MessageId => Id;

        public double instantiatedMapId;

        public CurrentMapInstanceMessage()
        {
        }
        public CurrentMapInstanceMessage(double instantiatedMapId)
        {
            this.instantiatedMapId = instantiatedMapId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (instantiatedMapId < 0 || instantiatedMapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + instantiatedMapId + ") on element instantiatedMapId.");
            }

            writer.WriteDouble((double)instantiatedMapId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            instantiatedMapId = (double)reader.ReadDouble();
            if (instantiatedMapId < 0 || instantiatedMapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + instantiatedMapId + ") on element of CurrentMapInstanceMessage.instantiatedMapId.");
            }

        }


    }
}








