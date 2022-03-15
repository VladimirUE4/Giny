using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyMemberInStandardFightMessage : AbstractPartyMemberInFightMessage  
    { 
        public  const ushort Id = 6653;
        public override ushort MessageId => Id;

        public MapCoordinatesExtended fightMap;

        public PartyMemberInStandardFightMessage()
        {
        }
        public PartyMemberInStandardFightMessage(MapCoordinatesExtended fightMap)
        {
            this.fightMap = fightMap;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            fightMap.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            fightMap = new MapCoordinatesExtended();
            fightMap.Deserialize(reader);
        }


    }
}








