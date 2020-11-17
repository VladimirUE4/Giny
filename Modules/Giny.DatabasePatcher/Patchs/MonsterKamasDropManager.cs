using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Time;
using Giny.ORM;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabasePatcher.Patchs
{
    public class MonsterKamasDropManager 
    {
        public const int BOSS_MULTIPLICATOR = 4;

        public const int DROPPED_KAMAS_RATIO = 3;

        public static void Initialize()
        {
            AsyncRandom random = new AsyncRandom();

            foreach (var monster in MonsterRecord.GetMonsterRecords())
            {
                int minDroppedKamas = 0;
                int maxDroppedKamas = 0;
                int level = monster.GetGrade(1).Level;

                minDroppedKamas = random.Next(level * DROPPED_KAMAS_RATIO, level * (DROPPED_KAMAS_RATIO * 2));

                maxDroppedKamas = minDroppedKamas + level * 2;

                if (maxDroppedKamas < 50)
                {
                    maxDroppedKamas = 50;
                }

                if (monster.IsBoss)
                {
                    minDroppedKamas *= BOSS_MULTIPLICATOR;
                    maxDroppedKamas *= BOSS_MULTIPLICATOR;
                }

                monster.MinDroppedKamas = minDroppedKamas / 2;
                monster.MaxDroppedKamas = maxDroppedKamas / 2;

                monster.UpdateInstantElement();

                Logger.Write("Fixed : " + monster.Name);
            }
        }
    }
}
