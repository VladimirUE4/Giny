using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("PVPRankItemCriterion", "com.ankamagames.dofus.datacenter.items.criterion")]
    public class PVPRankItemCriterion : ItemCriterion , IIndexedData
    {

        public int Id => throw new NotImplementedException();



    }
}






