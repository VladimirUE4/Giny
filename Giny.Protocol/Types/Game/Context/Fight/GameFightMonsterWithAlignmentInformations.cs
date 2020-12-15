using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightMonsterWithAlignmentInformations : GameFightMonsterInformations  
    { 
        public const ushort Id = 4758;
        public override ushort TypeId => Id;

        public ActorAlignmentInformations alignmentInfos;

        public GameFightMonsterWithAlignmentInformations()
        {
        }
        public GameFightMonsterWithAlignmentInformations(ActorAlignmentInformations alignmentInfos)
        {
            this.alignmentInfos = alignmentInfos;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            alignmentInfos.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            alignmentInfos = new ActorAlignmentInformations();
            alignmentInfos.Deserialize(reader);
        }


    }
}








