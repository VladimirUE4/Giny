using Giny.ORM;
using Giny.World;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Items;
using Giny.World.Records.Characters;
using Giny.World.Records.Items;
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
           "giny_tests", "root", "");


            DatabaseManager.Instance.DeleteTable<CharacterItemRecord>();


            DatabaseManager.Instance.LoadTables();

            TestThreadSafeness();

            Console.Read();


        }
        private static void TestThreadSafeness()
        {
            ItemManager.Instance.Initialize();

            int iterations = 100;

            for (int i = 0; i < 100; i++)
            {
                Thread thread3 = new Thread(new ThreadStart(delegate ()
                {
                    for (int w = 0; w < iterations; w++)
                    {
                        InsertItem(); // call unsafe code
                    }
                }));

                thread3.Start();
            }
        
            
        }

        private static void InsertItem()
        {
            int id = ItemManager.Instance.PopItemUID();
            CharacterItemRecord item = new CharacterItemRecord(1, id, 2469, 1, 1, new EffectCollection(), 1, "");
            item.AddInstantElement();
        }
    }
}
