using Giny.Core;
using Giny.Core.IO;
using Giny.IO.D2I;
using Giny.IO.D2O;
using Giny.ORM;
using Giny.ORM.Interfaces;
using Giny.ORM.IO;
using Giny.World.Managers.Entities.Look;
using Giny.World.Records;
using Giny.World.Records.Breeds;
using Giny.World.Records.Characters;
using Giny.World.Records.Effects;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using Giny.World.Records.Npcs;
using Giny.World.Records.Spells;
using Giny.World.Records.Tinsel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabaseSynchronizer
{
    class Program
    {
        public const bool BUILD_MISC = true;

        public const bool BUILD_D2O_TABLES = false;

        public const bool BUILD_MAPS = false;

        public const string D2I_FILE_PATH_FR = "i18n_fr.d2i";
        public const string D2I_FILE_PATH_EN = "i18n_en.d2i";

        public static D2IFile D2IFileFR;
        public static D2IFile D2IFileEN;

        [STAThread]
        static void Main(string[] args)
        {
            Logger.OnStartup();

            ConfigFile.LoadConfig();

            D2IFileFR = new D2IFile(D2I_FILE_PATH_FR);
            D2IFileEN = new D2IFile(D2I_FILE_PATH_EN);

            DatabaseManager.Instance.Initialize(Assembly.GetAssembly(typeof(BreedRecord)),
              "127.0.0.1", "giny_world", "root", "");

            if (BUILD_D2O_TABLES)
            {
                DatabaseManager.Instance.DropTableIfExists<SubareaRecord>();
                DatabaseManager.Instance.DropTableIfExists<BreedRecord>();
                DatabaseManager.Instance.DropTableIfExists<ExperienceRecord>();
                DatabaseManager.Instance.DropTableIfExists<HeadRecord>();
                DatabaseManager.Instance.DropTableIfExists<EffectRecord>();
                DatabaseManager.Instance.DropTableIfExists<MapScrollActionRecord>();
                DatabaseManager.Instance.DropTableIfExists<SpellRecord>();
                DatabaseManager.Instance.DropTableIfExists<SpellVariantRecord>();
                DatabaseManager.Instance.DropTableIfExists<ItemRecord>();
                DatabaseManager.Instance.DropTableIfExists<SpellStateRecord>();
                DatabaseManager.Instance.DropTableIfExists<WeaponRecord>();
                DatabaseManager.Instance.DropTableIfExists<LivingObjectRecord>();
                DatabaseManager.Instance.DropTableIfExists<EmoteRecord>();
                DatabaseManager.Instance.DropTableIfExists<SpellLevelRecord>();
                DatabaseManager.Instance.DropTableIfExists<OrnamentRecord>();
                DatabaseManager.Instance.DropTableIfExists<TitleRecord>();
                DatabaseManager.Instance.DropTableIfExists<MonsterRecord>();
                DatabaseManager.Instance.DropTableIfExists<SkillRecord>();
                DatabaseManager.Instance.DropTableIfExists<MapPositionRecord>();
                DatabaseManager.Instance.DropTableIfExists<NpcRecord>();
            }

            if (BUILD_MAPS)
            {
                DatabaseManager.Instance.DropTableIfExists<MapRecord>();
            }


            DatabaseManager.Instance.CreateAllTablesIfNotExists();

            D2OSynchronizer.Synchronize(BUILD_MISC, BUILD_D2O_TABLES);

            if (BUILD_MAPS)
                MapSynchronizer.Synchronize();

            Logger.WriteColor1("Build finished.");
            Console.Read();

        }



    }
}
