using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("BonusMonsterFamilyCriterion", "com.ankamagames.dofus.datacenter.bonus.criterion")]
    public class BonusMonsterFamilyCriterion : BonusCriterion , IIndexedData
    {

        public int Id => throw new NotImplementedException();



    }
}






