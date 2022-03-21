using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GuildHouseUpdateInformationMessage : NetworkMessage  
    { 
        public new const ushort Id = 6703;
        public override ushort MessageId => Id;

        public HouseInformationsForGuild housesInformations;

        public GuildHouseUpdateInformationMessage()
        {
        }
        public GuildHouseUpdateInformationMessage(HouseInformationsForGuild housesInformations)
        {
            this.housesInformations = housesInformations;
        }
        public override void Serialize(IDataWriter writer)
        {
            housesInformations.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            housesInformations = new HouseInformationsForGuild();
            housesInformations.Deserialize(reader);
        }


    }
}








