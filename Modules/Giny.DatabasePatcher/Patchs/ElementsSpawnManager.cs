using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.World.Managers.Generic;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabasePatcher.Patchs
{
    public class ElementsSpawnManager
    {
        public static void SynchronizeElements()
        {
            Logger.WriteColor2("Database Patcher > Synchronize interactives elements....");

            int count = 0;
            foreach (SkillRecord skillRecord in SkillRecord.GetSkills())
            {
                if (skillRecord.ParentBonesId != -1)
                {
                    InteractiveElementRecord[] elements = MapRecord.GetElementsByBonesId(skillRecord.ParentBonesId);

                    foreach (var element in elements)
                    {
                        if (!InteractiveSkillRecord.Exist(element.Identifier))
                        {
                            InteractiveSkillRecord interactiveSkillRecord = new InteractiveSkillRecord()
                            {
                                ActionIdentifier = GenericActionEnum.Collect,
                                Criteria = string.Empty,
                                Id = InteractiveSkillRecord.PopNextId(),
                                Identifier = element.Identifier,
                                MapId = element.MapId,
                                Param1 = string.Empty,
                                Param2 = string.Empty,
                                Param3 = string.Empty,
                                SkillId = (SkillTypeEnum)skillRecord.Id,
                                Type = (InteractiveTypeEnum)skillRecord.InteractiveTypeId,
                            };
                            element.Skill = interactiveSkillRecord;
                            interactiveSkillRecord.AddInstantElement();
                            count++;

                        }
                    }
                }
            }


            if (count > 0)
            {
                Logger.Write(count + " collect stated elements added on maps.");

                foreach (var map in MapRecord.GetMaps())
                {
                    map.Instance.Reload();
                }
            }
        }
    }
}
