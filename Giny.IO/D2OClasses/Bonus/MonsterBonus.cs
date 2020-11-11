using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("MonsterBonus", "com.ankamagames.dofus.datacenter.bonus")]
    public class MonsterBonus : MonsterLightBonus , IIndexedData
    {

        public int Id => throw new NotImplementedException();



    }
}






