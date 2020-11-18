using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus.Effects
{
    public abstract class EffectCustom : Effect
    {
        public const short TextEffectId = 990;

        public EffectCustom()
        {
            this.EffectId = TextEffectId;
        }
        
        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectString()
            {
                actionId = EffectId,
                value = GetEffectDescription(),
            };
        }


        public abstract string GetEffectDescription();
    }
}
