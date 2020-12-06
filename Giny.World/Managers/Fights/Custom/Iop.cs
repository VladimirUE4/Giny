﻿using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.World.Managers.Fights.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Custom
{
    public class Iop
    {
        [SpellCastHandler(SpellEnum.Conquest)]
        public class Conquest : DefaultSpellCastHandler
        {
            public Conquest(SpellCast cast) : base(cast)
            {

            }
            protected override IEnumerable<SpellEffectHandler> OrderHandlers()
            {
                EffectsEnum[] order = new EffectsEnum[]
                {
                   EffectsEnum.Effect_Kill,
                   EffectsEnum.Effect_Summon,
                };

                return OrderByEffects(order);
            }
        }
    }
}