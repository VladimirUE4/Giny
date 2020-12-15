using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameContextRemoveElementMessage : NetworkMessage  
    { 
        public new const ushort Id = 3288;
        public override ushort MessageId => Id;

        public double id;

        public GameContextRemoveElementMessage()
        {
        }
        public GameContextRemoveElementMessage(double id)
        {
            this.id = id;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (id < -9.00719925474099E+15 || id > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + id + ") on element id.");
            }

            writer.WriteDouble((double)id);
        }
        public override void Deserialize(IDataReader reader)
        {
            id = (double)reader.ReadDouble();
            if (id < -9.00719925474099E+15 || id > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + id + ") on element of GameContextRemoveElementMessage.id.");
            }

        }


    }
}








