using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayPortalInformations : GameRolePlayActorInformations  
    { 
        public const ushort Id = 8125;
        public override ushort TypeId => Id;

        public PortalInformation portal;

        public GameRolePlayPortalInformations()
        {
        }
        public GameRolePlayPortalInformations(PortalInformation portal)
        {
            this.portal = portal;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)portal.TypeId);
            portal.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            uint _id1 = (uint)reader.ReadUShort();
            portal = ProtocolTypeManager.GetInstance<PortalInformation>((short)_id1);
            portal.Deserialize(reader);
        }


    }
}








