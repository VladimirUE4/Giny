using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Experiences;
using Giny.World.Records.Characters;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus.Effects
{
    [ProtoContract]
    public class EffectPokefusLevel : EffectCustom
    {
        private const string Description = "Niveau {0}";

        [ProtoMember(21)]
        public long Exp
        {
            get;
            set;
        }
        public short Level
        {
            get
            {
                return ExperienceManager.Instance.GetCharacterLevel(Exp);
            }
        }
        public long LowerBoundExperience
        {
            get
            {
                return ExperienceManager.Instance.GetCharacterXPForLevel(Level);
            }
        }
        public long UpperBoundExperience
        {
            get
            {
                return ExperienceManager.Instance.GetCharacterXPForNextLevel(Level);
            }
        }
        public EffectPokefusLevel()
        {
            EffectId = TextEffectId;
        }
        public EffectPokefusLevel(long exp)
        {
            this.Exp = exp;
            EffectId = TextEffectId;
        }

        public void AddExperience(long value)
        {
            Exp += value;
        }

        protected override string GetEffectDescription()
        {
            return string.Format(Description, Level);
        }

        public override bool Equals(object obj)
        {
            return obj is EffectPokefusLevel ? Equals((EffectPokefusLevel)obj) : false;
        }
        private bool Equals(EffectPokefusLevel effect)
        {
            return Exp == effect.Exp;
        }

        public override object Clone()
        {
            return new EffectPokefusLevel(Exp);
        }

    }
}
