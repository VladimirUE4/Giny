using Giny.Core.DesignPattern;
using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Effects
{
    public class SpellAppearanceManager : Singleton<SpellAppearanceManager>
    {
        public delegate void SpellAppearanceDelegate(Fighter fighter, ServerEntityLook look);

        private Dictionary<int, SpellAppearanceDelegate> m_handlers = new Dictionary<int, SpellAppearanceDelegate>();

        [StartupInvoke(StartupInvokePriority.FourthPass)]
        public void Initialize()
        {
            foreach (var method in typeof(SpellAppearances).GetMethods())
            {
                SpellAppearanceAttribute attribute = method.GetCustomAttribute<SpellAppearanceAttribute>();

                if (attribute != null)
                {
                    SpellAppearanceDelegate @delegate = (SpellAppearanceDelegate)Delegate.CreateDelegate(typeof(SpellAppearanceDelegate), method);
                    m_handlers.Add(attribute.AppearanceId, @delegate);
                }
            }
        }
        public ServerEntityLook Apply(Fighter fighter, int appearanceId)
        {
            if (m_handlers.ContainsKey(appearanceId))
            {
                var look = fighter.Look.Clone();
                m_handlers[appearanceId](fighter, look);
                return look;
            }
            else
            {
                fighter.Fight.Warn("Unhandled appearanceId : " + appearanceId);
                return null;
            }
        }

    }
    public class SpellAppearanceAttribute : Attribute
    {
        public int AppearanceId
        {
            get;
            set;
        }
        public SpellAppearanceAttribute(int appearanceId)
        {
            this.AppearanceId = appearanceId;
        }
    }

}
