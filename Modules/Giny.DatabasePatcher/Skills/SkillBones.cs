using Giny.ORM;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabasePatcher.Skills
{
    class SkillBones
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
            {24,1081 },
            {69,3213 },
            {40,652 },
            {39,651 },
            {125 ,1019 },
        };

        public static void Patch()
        {
            foreach (var skillRecord in SkillRecord.GetSkills())
            {
                if (SkillsBonesIds.ContainsKey(skillRecord.Id))
                {
                    skillRecord.ParentBonesId = SkillsBonesIds[skillRecord.Id];
                }

                skillRecord.UpdateInstantElement();
            }
        }
    }
}
