using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayPrismInformations : GameRolePlayActorInformations  
    { 
        public const ushort Id = 4866;
        public override ushort TypeId => Id;

        public PrismInformation prism;

        public GameRolePlayPrismInformations()
        {
        }
        public GameRolePlayPrismInformations(PrismInformation prism)
        {
            this.prism = prism;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)prism.TypeId);
            prism.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            uint _id1 = (uint)reader.ReadUShort();
            prism = ProtocolTypeManager.GetInstance<PrismInformation>((short)_id1);
            prism.Deserialize(reader);
        }


    }
}








