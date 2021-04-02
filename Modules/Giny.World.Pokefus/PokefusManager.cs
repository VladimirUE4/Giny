using Giny.Core.DesignPattern;
using Giny.Core.Time;
using Giny.Pokefus.Effects;
using Giny.Pokefus.Fight.Fighters;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Results;
using Giny.World.Managers.Items;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus
{
    public class PokefusManager : Singleton<PokefusManager>
    {
        private const short RegularItemId = 15710;

        private const short MiniBossItemId = 15161;

        private const short BossItemId = 15393;

        private ItemRecord m_regularItemRecord;
        private ItemRecord m_miniBossItemRecord;
        private ItemRecord m_bossItemRecord;

        public const short MaxPokefusLevel = 200;

        public void Initialize()
        {
            m_regularItemRecord = ItemRecord.GetItem(RegularItemId);
            m_miniBossItemRecord = ItemRecord.GetItem(MiniBossItemId);
            m_bossItemRecord = ItemRecord.GetItem(BossItemId);
        }

        public void OnHumanOptionsCreated(Character character)
        {
            foreach (var item in GetPokefusItems(character.Inventory))
            {
                EffectPokefus pokefusEffect = item.Effects.Get<EffectPokefus>();
                MonsterRecord monsterRecord = MonsterRecord.GetMonsterRecord(pokefusEffect.MonsterId);
                character.AddFollower(monsterRecord.Look);
            }
        }
        public void OnPlayerResultApplied(FightPlayerResult result)
        {
            AsyncRandom random = new AsyncRandom();

            if (result.Fight.Winners == result.Fighter.Team)
            {
                foreach (var monster in result.Fighter.EnemyTeam.GetFighters<MonsterFighter>(false))
                {
                    var chance = (random.Next(0, 100) + random.NextDouble());
                    var dropRate = GetDropRate(monster, result.Fighter);

                    if (!(dropRate >= chance))
                        continue;

                    CharacterItemRecord item = CreatePokefusItem(result.Character.Id, monster.Record, monster.Monster.Grade.GradeId);
                    result.Loot.AddItem(item.GId, item.Quantity);
                    result.Character.Inventory.AddItem(item);
                }
            }
        }
        private double GetDropRate(MonsterFighter monster, CharacterFighter fighter)
        {
            double probability = 0d;


            if (monster.Level >= 1)
            {
                probability = 0.1;
            }
            if (monster.Level >= 50)
            {
                probability = 0.05;
            }
            if (monster.Level >= 100)
            {
                probability = 0.04;
            }
            if (monster.Level >= 150)
            {
                probability = 0.03;
            }
            if (monster.Level >= 200)
            {
                probability = 0.02;
            }
            if (monster.Record.IsBoss)
            {
                probability = 0.01;
            }

            probability += (fighter.Level / 200) / 20;


            probability += (fighter.Stats.Prospecting.TotalInContext() / 5000);

            var percentage = Math.Round(probability * 100d, 2);

            fighter.Character.Reply("Chance de drop pour " + monster.Name + ": " + percentage + "%", Color.CornflowerBlue);

            return percentage;
        }
        public void OnFighterJoined(Fighter fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            CharacterFighter characterFighter = (CharacterFighter)fighter;

            foreach (var pokefusItem in GetPokefusItems(characterFighter.Character.Inventory))
            {
                EffectPokefus effect = pokefusItem.Effects.Get<EffectPokefus>();
                MonsterRecord monsterRecord = MonsterRecord.GetMonsterRecord(effect.MonsterId);

                CellRecord cell = fighter.Team.GetPlacementCell();

                PokefusFighter pokefusFighter = new PokefusFighter(characterFighter, monsterRecord, null, effect.GradeId, cell);

                fighter.Team.AddFighter(pokefusFighter);
            }
        }

        private IEnumerable<CharacterItemRecord> GetPokefusItems(Inventory inventory)
        {
            return inventory.GetEquipedItems().Where(x => IsPokefusItem(x));
        }
        private bool IsPokefusItem(CharacterItemRecord item)
        {
            return item.Effects.Exists<EffectPokefus>();
        }
        public CharacterItemRecord CreatePokefusItem(long characterId, MonsterRecord monster, byte monsterGradeId)
        {
            ItemRecord itemRecord;

            if (monster.IsMiniBoss)
            {
                itemRecord = m_miniBossItemRecord;
            }
            else if (monster.IsBoss)
            {
                itemRecord = m_bossItemRecord;
            }
            else
            {
                itemRecord = m_regularItemRecord;
            }

            CharacterItemRecord item = ItemManager.Instance.CreateCharacterItem(itemRecord, characterId, 1);

            item.Effects.Clear();

            item.Effects.Add(new EffectPokefus((short)monster.Id, monster.Name, monsterGradeId));
            item.Effects.Add(new EffectPokefusLevel(0));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Followed, (int)monster.Id));

            return item;
        }


    }
}
