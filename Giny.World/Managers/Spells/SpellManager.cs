using Giny.Core.DesignPattern;
using Giny.Protocol.Custom.Enums;
using Giny.World.Managers.Fights.Cast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Spells
{
    public class SpellManager : Singleton<SpellManager>
    {
        private readonly Dictionary<SpellEnum, Type> m_handlers = new Dictionary<SpellEnum, Type>();

        [StartupInvoke(StartupInvokePriority.FourthPass)]
        public void Initialize()
        {
            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                var attribute = type.GetCustomAttribute<SpellCastHandlerAttribute>();

                if (attribute != null)
                {
                    m_handlers.Add(attribute.SpellEnum, type);
                }
            }
        }
        public SpellCastHandler GetSpellCastHandler(SpellCast cast)
        {
            if (m_handlers.ContainsKey((SpellEnum)cast.SpellId))
            {
                return (SpellCastHandler)Activator.CreateInstance(m_handlers[(SpellEnum)cast.SpellId], cast);
            }

            return new DefaultSpellCastHandler(cast);
        }
    }
}
