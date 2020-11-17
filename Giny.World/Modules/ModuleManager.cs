using Giny.Core;
using Giny.Core.DesignPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Modules
{
    public class ModuleManager : Singleton<ModuleManager>
    {
        private const string ModulesPath = "Modules\\";

        private const string Extension = ".dll";

        private readonly Dictionary<string, IModule> m_modules = new Dictionary<string, IModule>();

        [StartupInvoke("Modules", StartupInvokePriority.Initial)]
        public void Initialize()
        {
            string path = Path.Combine(Environment.CurrentDirectory, ModulesPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file).ToLower() == Extension)
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttribute<ModuleAttribute>() != null)
                        {
                            if (type.GetInterfaces().Contains(typeof(IModule)))
                            {
                                LoadModule(type);
                            }
                        }
                    }
                }
            }

        }
        [StartupInvoke("Modules", StartupInvokePriority.Modules)]
        public void LoadModules()
        {
            foreach (var module in m_modules.Values)
            {
                module.Initialize();
                module.CreateHooks();
            }
        }
        private void LoadModule(Type type)
        {
            string moduleName = type.GetCustomAttribute<ModuleAttribute>().ModuleName;
            IModule module = (IModule)Activator.CreateInstance(type);
            Logger.Write("Module '" + moduleName + "' loaded", MessageState.INFO2);
            m_modules.Add(moduleName, module);
        }
    }
}
