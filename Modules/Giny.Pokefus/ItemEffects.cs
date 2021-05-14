using Giny.Protocol.Enums;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Items;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus
{
    public class ItemEffects
    {
        [ItemEffect(EffectsEnum.Effect_Followed)]
        public static void Followed(Character character, int delta)
        {
            var monsterTemplate = MonsterRecord.GetMonsterRecord((short)Math.Abs(delta));

            if (delta > 0)
            {
                character.AddFollower(monsterTemplate.Look);
            }
            else
            {
                character.RemoveFollower(monsterTemplate.Look);
            }
        }
    }
}
