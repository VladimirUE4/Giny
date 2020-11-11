using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("DayItemCriterion", "com.ankamagames.dofus.datacenter.items.criterion")]
    public class DayItemCriterion : ItemCriterion , IIndexedData
    {

        public int Id => throw new NotImplementedException();



    }
}






