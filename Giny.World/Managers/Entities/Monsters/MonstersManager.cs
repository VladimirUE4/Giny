using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.Core.Time;
using Giny.World.Managers.Entities;
using Giny.World.Managers.Entities.Monsters;
using Giny.World.Managers.Maps.Instances;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Monsters
{
    public class MonstersManager : Singleton<MonstersManager>
    {
        public const sbyte MAX_MONSTER_GROUP_PER_MAP = 5;

        public const int SPAWNING_INTERVAL = 5;

        public const byte MAX_MONSTER_PER_GROUP = 9;

        public const int MonsterSpawningPoolInterval = 5;

        public void SpawnMonsterGroups(MapRecord map, AsyncRandom random)
        {
            if (map.Subarea.Monsters.Length > 0)
            {
                for (sbyte groupId = 0; groupId < random.Next(1, MAX_MONSTER_GROUP_PER_MAP + 1); groupId++)
                {
                    SpawnMonsterGroup(map, random);
                }
            }
        }
        public void SpawnMonsterGroup(MapRecord map, AsyncRandom random)
        {
            AddGeneratedMonsterGroup(map.Instance, map.Subarea.Monsters, random);
        }
        public void AddGeneratedMonsterGroup(MapInstance instance, MonsterSpawnRecord[] spawns, AsyncRandom random)
        {
            MonsterGroup group = new MonsterGroup(instance.Record, instance.FindMonsterGroupCell().Id);
            group.Direction = Entity.RandomDirection4D(random);

            int monsterCount = random.Next(1, MAX_MONSTER_PER_GROUP + 1);

            int num2 = 0;

            var shuffled = spawns.Shuffle().ToArray();

            foreach (var monsterSpawn in shuffled)
            {
                double num = random.NextDouble(0, 1);

                if (num2 == monsterCount)
                {
                    break;
                }
                if (monsterSpawn.Probability > num)
                {
                    MonsterRecord template = MonsterRecord.GetMonsterRecord(monsterSpawn.MonsterId);
                    Monster monster = new Monster(template, group.GetCell());
                    group.AddMonster(monster);
                    num2++;
                }
            }

            if (group.MonsterCount == 0)
            {
                MonsterRecord template = MonsterRecord.GetMonsterRecord(shuffled.Random().MonsterId);
                Monster monster = new Monster(template, group.GetCell());
                group.AddMonster(monster);
            }

            instance.AddEntity(group);
        }

        public void SpawnDungeonGroup(MapRecord map)
        {
            if (map.MonsterRoom.MonsterIds.Count == 0)
            {
                return;
            }

            ModularMonsterGroup group = new ModularMonsterGroup(map, map.Instance.FindMonsterGroupCell().Id);

            foreach (var monsterId in map.MonsterRoom.MonsterIds)
            {
                MonsterRecord template = MonsterRecord.GetMonsterRecord(monsterId);
                Monster monster = new Monster(template, group.GetCell());
                group.AddMonster(monster);
            }

            map.Instance.AddEntity(group);
        }

        public void AddFixedMonsterGroup(MapInstance instance, short cellId, MonsterRecord[] monsterRecords)
        {
            MonsterGroup group = new MonsterGroup(instance.Record, cellId);

            foreach (var template in monsterRecords)
            {
                Monster monster = new Monster(template, group.GetCell());
                group.AddMonster(monster);
            }

            instance.AddEntity(group);

        }
        public void AddFixedMonsterGroup(MapInstance instance, short cellId, MonsterRecord[] monsterRecords, byte[] grades)
        {
            if (monsterRecords.Length != grades.Length)
            {
                throw new Exception("Record and grades array must have same lenght.");
            }
            MonsterGroup group = new MonsterGroup(instance.Record, cellId);

            for (int i = 0; i < monsterRecords.Length; i++)
            {
                Monster monster = new Monster(monsterRecords[i], group.GetCell(), grades[i]);
                group.AddMonster(monster);
            }


            instance.AddEntity(group);

        }
        public bool GroupExist(MapInstance instance, MonsterRecord[] monsterRecords)
        {
            foreach (var group in instance.GetEntities<MonsterGroup>())
            {
                if (group.GetMonsters().All(x => monsterRecords.Contains(x.Record)))
                    return true;
            }
            return false;
        }
        public void RemoveGroup(MapInstance instance, MonsterRecord[] monsterRecords)
        {
            foreach (var group in instance.GetEntities<MonsterGroup>())
            {
                if (group.GetMonsters().All(x => monsterRecords.Contains(x.Record)))
                {
                    instance.RemoveEntity(group.Id);
                    break;
                }
            }
        }
    }
}
