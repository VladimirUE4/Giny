using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class CurrentMapMessage : NetworkMessage  
    { 
        public new const ushort Id = 7422;
        public override ushort MessageId => Id;

        public double mapId;
        public string mapKey;

        public CurrentMapMessage()
        {
        }
        public CurrentMapMessage(double mapId,string mapKey)
        {
            this.mapId = mapId;
            this.mapKey = mapKey;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element mapId.");
            }

            writer.WriteDouble((double)mapId);
            writer.WriteUTF((string)mapKey);
        }
        public override void Deserialize(IDataReader reader)
        {
            mapId = (double)reader.ReadDouble();
            if (mapId < 0 || mapId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + mapId + ") on element of CurrentMapMessage.mapId.");
            }

            mapKey = (string)reader.ReadUTF();
        }


    }
}








