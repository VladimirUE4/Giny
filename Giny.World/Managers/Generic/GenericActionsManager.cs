using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Generic
{
    public class GenericActionsManager : Singleton<GenericActionsManager>
    {
        private Dictionary<GenericActionEnum, MethodInfo> m_handlers = new Dictionary<GenericActionEnum, MethodInfo>();

        [StartupInvoke(StartupInvokePriority.SixthPath)]
        public void Initialize()
        {
            foreach (var method in typeof(GenericActions).GetMethods())
            {
                var attribute = method.GetCustomAttribute<GenericActionHandlerAttribute>();

                if (attribute != null)
                {
                    m_handlers.Add(attribute.ActionEnum, method);
                }
            }
        }
        public bool IsHandled(IGenericActionParameter parameter)
        {
            return m_handlers.ContainsKey(parameter.ActionIdentifier);
        }
        public bool Handle(Character character, IGenericActionParameter parameter)
        {
            if (m_handlers.ContainsKey(parameter.ActionIdentifier))
            {
                m_handlers[parameter.ActionIdentifier].Invoke(null, new object[] { character, parameter });
                return true;
            }
            else
            {
                Logger.Write("Unknown action identifier: " + parameter.ActionIdentifier, MessageState.WARNING);
                return false;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class GenericActionHandlerAttribute : Attribute
    {
        public GenericActionEnum ActionEnum
        {
            get;
            set;
        }
        public GenericActionHandlerAttribute(GenericActionEnum actionEnum)
        {
            this.ActionEnum = actionEnum;
        }
    }
}
