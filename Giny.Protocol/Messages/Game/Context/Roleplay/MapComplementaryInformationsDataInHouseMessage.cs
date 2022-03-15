using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MapComplementaryInformationsDataInHouseMessage : MapComplementaryInformationsDataMessage  
    { 
        public  const ushort Id = 2024;
        public override ushort MessageId => Id;

        public HouseInformationsInside currentHouse;

        public MapComplementaryInformationsDataInHouseMessage()
        {
        }
        public MapComplementaryInformationsDataInHouseMessage(HouseInformationsInside currentHouse)
        {
            this.currentHouse = currentHouse;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            currentHouse.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            currentHouse = new HouseInformationsInside();
            currentHouse.Deserialize(reader);
        }


    }
}








