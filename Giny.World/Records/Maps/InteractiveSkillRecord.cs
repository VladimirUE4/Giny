using Giny.Core.DesignPattern;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Generic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Maps
{
    [Table("interactiveskills")]
    public class InteractiveSkillRecord : ITable
    {
        [Container]
        private static ConcurrentDictionary<long, InteractiveSkillRecord> InteractiveSkills = new ConcurrentDictionary<long, InteractiveSkillRecord>();

        [Primary]
        public long Id
        {
            get;
            set;
        }
        public long MapId
        {
            get;
            set;
        }
        public long Identifier
        {
            get;
            set;
        }
        public GenericActionEnum ActionIdentifier
        {
            get;
            set;
        }
        public InteractiveTypeEnum Type
        {
            get;
            set;
        }
        public SkillTypeEnum SkillEnum
        {
            get;
            set;
        }
        [Ignore]
        public SkillRecord Record
        {
            get;
            set;
        }
        public string Param1
        {
            get;
            set;
        }

        public string Param2
        {
            get;
            set;
        }

        public string Param3
        {
            get;
            set;
        }
        public static long PopNextId()
        {
            if (InteractiveSkills.Count == 0)
            {
                return 1;
            }
            else
            {
                return InteractiveSkills.Keys.Last() + 1;
            }
        }
        public string Criteria
        {
            get;
            set;
        }
       
        public static bool Exist(int identifier)
        {
            return InteractiveSkills.Any(x => x.Value.Identifier == identifier);
        }
        public static InteractiveSkillRecord GetInteractiveSkill(long id)
        {
            return InteractiveSkills.Values.FirstOrDefault(x => x.Identifier == id);
        }

        [StartupInvoke("Interactive Skills Bindings", StartupInvokePriority.SixthPath)]
        public static void Initialize()
        {
            foreach (var skill in InteractiveSkills.Values)
            {
                skill.Record = SkillRecord.GetSkill(skill.SkillEnum);
            }
        }
        public static IEnumerable<InteractiveSkillRecord> GetInteractiveSkills()
        {
            return InteractiveSkills.Values;
        }

    }
}
