using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Cast
{
    public abstract class SpellCastHandler
    {
        public SpellCast Cast
        {
            get;
            private set;
        }

        protected SpellEffectHandler[] Handlers
        {
            get;
            set;
        }

        public IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return Handlers;
        }

        public bool Initialized => m_initialized;

        protected bool m_initialized = false;

        public SpellCastHandler(SpellCast cast)
        {
            this.Cast = cast;
        }

        public abstract bool Initialize();

        public abstract bool Execute();

    }
}
