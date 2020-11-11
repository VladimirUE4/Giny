using Giny.Core.DesignPattern;
using Giny.World.Records;
using Giny.World.Records.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Experiences
{
    public class ExperienceManager : Singleton<ExperienceManager>
    {
        public const short MAX_LEVEL = 200;

        public const short MAX_LEVEL_OMEGA = 1200;

        private ExperienceRecord HighestExperienceOmega;

        private ExperienceRecord HighestExperience;

        [StartupInvoke(StartupInvokePriority.SixthPath)]
        public void Initialize()
        {
            HighestExperienceOmega = ExperienceRecord.GetExperienceForLevel(MAX_LEVEL_OMEGA);
            HighestExperience = ExperienceRecord.GetExperienceForLevel(MAX_LEVEL);
        }

        public short GetCharacterLevel(long experience)
        {
            try
            {
                if (experience >= HighestExperienceOmega.ExperienceCharacter)
                    return MAX_LEVEL_OMEGA;

                return (short)(ExperienceRecord.GetExperiences().First(entry => entry.ExperienceCharacter > experience).Level - 1);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(string.Format("Experience {0} isn't bind to a character level", experience), ex);
            }
        }
        public short GetJobLevel(long experience)
        {
            try
            {
                if (experience >= HighestExperience.ExperienceJob)
                    return MAX_LEVEL;

                return (short)(ExperienceRecord.GetExperiences().First(entry => entry.ExperienceJob > experience).Level - 1);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(string.Format("Experience {0} isn't bind to a character level", experience), ex);
            }
        }

        public long GetJobXPForLevel(short level)
        {
            return ExperienceRecord.GetExperienceForLevel(level).ExperienceJob;
        }
        public long GetJobXPForNextLevel(short level)
        {
            if (level >= MAX_LEVEL)
                return HighestExperience.ExperienceJob;

            return GetJobXPForLevel((short)(level + 1));
        }
        public long GetCharacterXPForLevel(short level)
        {
            return ExperienceRecord.GetExperienceForLevel(level).ExperienceCharacter;
        }
        public long GetCharacterXPForNextLevel(short level)
        {
            if (level >= MAX_LEVEL_OMEGA)
                return HighestExperienceOmega.ExperienceCharacter;

            return GetCharacterXPForLevel((short)(level + 1));
        }
    }
}
