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
        public const int MaxJobLevelGap = 100;

        public int GetCollectedItemQuantity(int jobLevel, SkillRecord skillRecord)
        {
            AsyncRandom rd = new AsyncRandom();
            return rd.Next(jobLevel == ExperienceManager.MaxLevel ? 7 : 1, skillRecord.MinLevel == ExperienceManager.MaxLevel ? 1 : (int)(7d + ((jobLevel - skillRecord.MinLevel) / 10)));
        }

        public int GetCraftExperience(short resultLevel, short crafterLevel, int craftXpRatio)
        {
            double result = 0;
            double value = 0;

            if (resultLevel - MaxJobLevelGap > crafterLevel)
            {
                return 0;
            }

            value = 20.0d * resultLevel / (Math.Pow((double)crafterLevel - resultLevel, 1.1) / 10.0 + 1.0);

            if (craftXpRatio > -1)
            {
                result = (double)value * (craftXpRatio / 100d);
            }
            else if (craftXpRatio > -1)
            {
                result = value * (craftXpRatio / 100);
            }
            else
            {
                result = value;
            }
            return (int)Math.Floor(result) * ConfigFile.Instance.CraftXpRate;
        }
    }
}
