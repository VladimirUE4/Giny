using Giny.Core.DesignPattern;
using Giny.Core.Time;
using Giny.World.Managers.Experiences;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Formulas
{
    public class JobFormulas : Singleton<JobFormulas>
    {
        public int GetCollectedItemQuantity(int jobLevel, SkillRecord skillRecord)
        {
            AsyncRandom rd = new AsyncRandom();
            return rd.Next(jobLevel == ExperienceManager.MaxLevel ? 7 : 1, skillRecord.MinLevel == ExperienceManager.MaxLevel ? 1 : (int)(7d + ((jobLevel - skillRecord.MinLevel) / 10)));
        }
    }
}
