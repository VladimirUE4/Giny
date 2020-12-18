using Giny.Protocol.Enums;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects.Targets
{
    public class BreedCriterion : TargetCriterion
    {
        public BreedCriterion(bool caster, int breed, bool required) : base(caster)
        {
            Breed = breed;
            Required = required;
        }

        public int Breed
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsDisjonction => false;

        public override bool IsTargetValid(Fighter actor, SpellEffectHandler handler)
        {
            if (Caster)
                actor = handler.Source;

            if (actor is CharacterFighter)
                return Required ? (int)((CharacterFighter)actor).Character.Breed.Id == Breed : (int)((CharacterFighter)actor).Character.Breed.Id != Breed;
            else
                return Required ? (int)BreedEnum.MONSTER == Breed : (int)BreedEnum.MONSTER != Breed;
        }
        public override string ToString()
        {
            return "Breed (" + (BreedEnum)Breed + ")";
        }
    }
}
