using Giny.Core.DesignPattern;
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
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
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
                EffectPokefus pokefusEffect = item.GetFirstEffect<EffectPokefus>();
                MonsterRecord monsterRecord = MonsterRecord.GetMonsterRecord(pokefusEffect.MonsterId);
                character.AddFollower(monsterRecord.Look);
            }
        }
        public void OnPlayerResultApplied(FightPlayerResult result)
        {
            foreach (var monster in result.Fighter.EnemyTeam.GetFighters<MonsterFighter>(false))
            {
                CharacterItemRecord item = CreatePokefusItem(result.Character.Id, monster.Record, monster.Monster.Grade.GradeId);

                result.Loot.AddItem(item.GId, item.Quantity);
                result.Character.Inventory.AddItem(item);
            }
        }
        public void OnFighterJoined(Fighter fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            CharacterFighter characterFighter = (CharacterFighter)fighter;

            foreach (var pokefusItem in GetPokefusItems(characterFighter.Character.Inventory))
            {
                EffectPokefus effect = pokefusItem.GetFirstEffect<EffectPokefus>();
                MonsterRecord monsterRecord = MonsterRecord.GetMonsterRecord(effect.MonsterId);

                PokefusFighter pokefusFighter = new PokefusFighter(characterFighter.Team, characterFighter.RoleplayCell, monsterRecord, monsterRecord.GetGrade(effect.GradeId));

                fighter.Team.AddFighter(pokefusFighter);
            }
        }

        private IEnumerable<CharacterItemRecord> GetPokefusItems(Inventory inventory)
        {
            return inventory.GetEquipedItems().Where(x => IsPokefusItem(x));
        }
        private bool IsPokefusItem(CharacterItemRecord item)
        {
            return item.HasEffect<EffectPokefus>();
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

            item.RemoveEffects();

            item.AddEffect(new EffectPokefus((short)monster.Id, monster.Name, monsterGradeId));
            item.AddEffect(new EffectPokefusLevel(0));
            item.AddEffect(new EffectInteger(EffectsEnum.Effect_Followed, (int)monster.Id));

            return item;
        }


    }
}
