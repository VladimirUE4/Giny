using Giny.Core.DesignPattern;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects
{
    public class SpellEffectManager : Singleton<SpellEffectManager>
    {
        private static Dictionary<SpellEffectHandlerAttribute[], Type> Handlers = new Dictionary<SpellEffectHandlerAttribute[], Type>();

        [StartupInvoke(StartupInvokePriority.FifthPass)]
        public static void Initialize()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                SpellEffectHandlerAttribute[] handlers = type.GetCustomAttributes<SpellEffectHandlerAttribute>().ToArray();

                if (handlers.Length > 0)
                {
                    Handlers.Add(handlers, type);
                }
            }
        }

        public SpellEffectHandler GetSpellEffectHandler(Effect effect, SpellCastHandler castHandler)
        {
            var handler = Handlers.FirstOrDefault(x => x.Key.FirstOrDefault(w => w.Effect == effect.EffectEnum) != null);

            if (handler.Value != null)
            {
                return (SpellEffectHandler)Activator.CreateInstance(handler.Value, new object[] { effect, castHandler, });
            }
            else
            {
                return null;
            }
        }
    }
}
