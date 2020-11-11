using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ZaapDestinationsMessage : TeleportDestinationsMessage  
    { 
        public new const ushort Id = 7339;
        public override ushort MessageId => Id;

        public double spawnMapId;

        public ZaapDestinationsMessage()
        {
        }
        public ZaapDestinationsMessage(double spawnMapId)
        {
            this.spawnMapId = spawnMapId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (spawnMapId < 0 || spawnMapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + spawnMapId + ") on element spawnMapId.");
            }

            writer.WriteDouble((double)spawnMapId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spawnMapId = (double)reader.ReadDouble();
            if (spawnMapId < 0 || spawnMapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + spawnMapId + ") on element of ZaapDestinationsMessage.spawnMapId.");
            }

        }


    }
}








