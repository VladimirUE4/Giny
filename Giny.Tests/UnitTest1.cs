using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.IPC.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Experiences;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Stats;
using Giny.World.Network;
using Giny.World.Records.Breeds;
using Giny.World.Records.Characters;
using Giny.World.Records.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Giny.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Test()
        {
            DatabaseManager.Instance.Initialize(Assembly.GetAssembly(typeof(CharacterRecord)), "127.0.0.1", "giny_world", "root", "");
            DatabaseManager.Instance.LoadTable<ExperienceRecord>();
            DatabaseManager.Instance.LoadTable<BreedRecord>();
            DatabaseManager.Instance.LoadTable<MapRecord>();
            MapsManager.Instance.CreateInstances();
            ExperienceManager.Instance.Initialize();
        }
        [TestMethod]
        public void TestMethod1()
        {

            WorldClient client = Mock.Of<WorldClient>();
            client.Account = new Account(1, "lolo", "rkoerk", 1, ServerRoleEnum.Administrator);

            CharacterRecord record = new CharacterRecord();
            record.BreedId = 1;
            record.Shortcuts = new List<World.Managers.Shortcuts.CharacterShortcut>();
            record.Look = EntityLookManager.Instance.Parse("{1003}");

            Character character = new Character(client, record);
            character.Map = MapRecord.GetMap(0);

            Character target = new Character(client, record);

            CellRecord cell = Mock.Of<CellRecord>();

            Fight fight = FightManager.Instance.CreateFightDual(character, target, cell);

            Fighter fighter1 = character.CreateFighter(fight.BlueTeam);

            Fighter fighter2 = character.CreateFighter(fight.BlueTeam);

            fight.RedTeam.AddFighter(fighter2);
            fight.BlueTeam.AddFighter(fighter1);

            fight.StartPlacement();

            bool result = fighter1.CastSpell(1, 0);

            Assert.IsTrue(result);

        }
    }
}
