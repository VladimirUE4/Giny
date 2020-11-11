using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabaseSynchronizer
{
    class SelfMadeSkillRecord
    {
        private static Dictionary<long, int> SkillsBonesIds = new Dictionary<long, int>()
        {
            {45 , 660},
            {46,  661 },
            {296, 660},
            {50,  662},
            {300, 662},
            {54,  663 },
            {37,  654 },
            {102, 4938 },
            {6,   650 },
            {57,  701 },
            {53,  664},
            {68,  3212 },
            {124, 1018 },
            {35,  659 },
        };
        public static void Apply(SkillRecord record)
        {
            if (SkillsBonesIds.ContainsKey(record.Id))
            {
                record.ParentBonesId = SkillsBonesIds[record.Id];
            }
        }
    }
}
