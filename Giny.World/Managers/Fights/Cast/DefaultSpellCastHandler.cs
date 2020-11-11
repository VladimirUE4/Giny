using Giny.Core.Time;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Cast
{
    public class DefaultSpellCastHandler : SpellCastHandler
    {
        public DefaultSpellCastHandler(SpellCast cast) : base(cast)
        {

        }

        public override bool Initialize()
        {
            var random = new AsyncRandom();

            var effects = Cast.IsCriticalHit ? Cast.Spell.Level.CriticalEffects : Cast.Spell.Level.Effects;

            var handlers = new List<SpellEffectHandler>();

            var groups = effects.GroupBy(x => x.Group);
            double totalRandSum = effects.Sum(entry => entry.Random);
            var randGroup = random.NextDouble();
            var stopRandGroup = false;

            foreach (var groupEffects in groups)
            {
                double randSum = groupEffects.Sum(entry => entry.Random);

                if (randSum > 0)
                {
                    if (stopRandGroup)
                        continue;

                    if (randGroup > randSum / totalRandSum)
                    {
                        randGroup -= randSum / totalRandSum;
                        continue;
                    }

                    stopRandGroup = true;
                }

                var randEffect = random.NextDouble();
                var stopRandEffect = false;

                foreach (var effect in groupEffects)
                {
                    if (groups.Count() <= 1)
                    {
                        if (effect.Random > 0)
                        {
                            if (stopRandEffect)
                                continue;

                            if (randEffect > effect.Random / randSum)
                            {
                                randEffect -= effect.Random / randSum;
                                continue;
                            }

                            stopRandEffect = true;
                        }
                    }

                    var handler = SpellEffectManager.Instance.GetSpellEffectHandler(effect, this);

                    if (handler != null)
                    {
                        /*   if (MarkTrigger != null)
                               handler.MarkTrigger = MarkTrigger; */

                        if (!handler.CanApply())
                        {
                            // log ?
                            return false;
                        }

                        handlers.Add(handler);
                    }
                    else
                    {
                        Cast.Source.Fight.Warn("Unknown effect handler " + effect.EffectEnum);
                    }
                }
            }


            Handlers = handlers.ToArray();
            m_initialized = true;

            return true;
        }
        public override bool Execute()
        {
            if (!m_initialized)
            {
                if (!Initialize())
                {
                    return false;
                }
            }

            BeforeExecute();

            IEnumerable<SpellEffectHandler> handlers = Handlers.OrderBy(x => x.GetPriority());

            foreach (var handler in handlers)
            {
                handler.SetTriggerToken(Cast.Token);
                handler.Execute();
            }

            return true;
        }

        public virtual void BeforeExecute()
        {
             
        }
    }
}
