using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("QuestObjectiveGoToNpc", "com.ankamagames.dofus.datacenter.quest.objectives")]
    public class QuestObjectiveGoToNpc : QuestObjective , IIndexedData
    {

        public int Id => throw new NotImplementedException();



    }
}






