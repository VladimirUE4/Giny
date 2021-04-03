using Giny.ORM;
using Giny.World;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Items;
using Giny.World.Records.Characters;
using Giny.World.Records.Donjon;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giny.Tests
{
    class Program
    {
        static void Main(string[] args)
        {

            DatabaseManager.Instance.Initialize(Assembly.GetAssembly(typeof(CharacterRecord)), "127.0.0.1",
           "giny_world", "root", "");


            DatabaseManager.Instance.LoadTables();

            Console.WriteLine("Tables loaded");

            foreach (var donjon in DonjonRecord.GetDonjons())
            {
                DungeonRecord newRecord = new DungeonRecord();

                newRecord.Id = donjon.Id;
                newRecord.Name = donjon.Name;
                newRecord.EntranceMapId = donjon.EntranceMap;
                newRecord.ExitMapId = donjon.ExitMap;
                newRecord.Rooms = new Dictionary<long, MonsterRoom>();

                foreach (var map in donjon.Maps)
                {
                    newRecord.Rooms.Add(map, new MonsterRoom());
                }

                newRecord.AddInstantElement();
            }

            Console.WriteLine("Finished");
            Console.Read();


        }

    }
}
