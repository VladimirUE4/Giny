using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class MonsterInGroupInformations : MonsterInGroupLightInformations  
    { 
        public const ushort Id = 2614;
        public override ushort TypeId => Id;

        public EntityLook look;

        public MonsterInGroupInformations()
        {
        }
        public MonsterInGroupInformations(EntityLook look,int genericId,byte grade,short level)
        {
            this.look = look;
            this.genericId = genericId;
            this.grade = grade;
            this.level = level;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            look.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            look = new EntityLook();
            look.Deserialize(reader);
        }


    }
}








