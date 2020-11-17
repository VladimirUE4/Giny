using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects
{
    public class SpellAppearances
    {
        [SpellAppearance(729)]
        public static void MomificationLook(Fighter fighter, ServerEntityLook look)
        {
            if (!look.IsRiding)
            {
                look.SetBones(113);
            }
            else
            {
                look.ActorLook.SetBones(1068);
            }
        }
        [SpellAppearance(106)]
        public static void CowardLook(Fighter fighter, ServerEntityLook look)
        {
            if (!look.IsRiding)
            {
                look.SetBones(1576);
            }

            if (fighter.Sex)
            {
                look.AddSkin(1450);
            }
            else
            {
                look.AddSkin(1449);
            }
        }

        [SpellAppearance(105)]
        public static void PsychopathLook(Fighter fighter, ServerEntityLook look)
        {
            if (!look.IsRiding)
                look.SetBones(1575);

            if (fighter.Sex)
            {
                look.AddSkin(1448);
            }
            else
            {
                look.AddSkin(1443);
            }
        }

        [SpellAppearance(1318)]
        public static void SentinelLook(Fighter fighter, ServerEntityLook look)
        {
            /* Todo */
        }
    }
}
