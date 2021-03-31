using Giny.IO.D2O;
using Giny.IO.D2OClasses;
using Giny.ORM;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using Giny.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Giny.Dungeons
{
    /// <summary>
    /// Logique d'interaction pour Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public const float RespawnDelay = 10f;

        private DungeonMapRecord CurrentDungeonMap
        {
            get
            {
                return (DungeonMapRecord)dungeonMaps.SelectedItem;
            }
        }
        private MonsterRecord SearchedMonster
        {
            get
            {
                return (MonsterRecord)searchMonsters.SelectedItem;
            }
        }
        public Editor()
        {
            InitializeComponent();

            foreach (var map in DungeonMapRecord.GetDungeonMaps())
            {
                dungeonMaps.Items.Add(map);
            }

            monsterCanvas.Visibility = Visibility.Hidden;
        }

        private void dungeonMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            monsters.Items.Clear();

            if (CurrentDungeonMap == null)
            {
                monsterCanvas.Visibility = Visibility.Hidden;
                return;
            }


            foreach (short monsterId in CurrentDungeonMap.Monsters)
            {
                var monster = MonsterRecord.GetMonsterRecord(monsterId);
                monsters.Items.Add(monster);
            }

            monsterCanvas.Visibility = Visibility.Visible;
        }



        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchMonsters.Items.Clear();

            string search = searchText.Text;

            var results = MonsterRecord.GetMonsterRecords().Where(x => x.Name.ToLower().Contains(search.ToLower()) || x.Id.ToString() == search.ToString());

            foreach (var result in results)
            {
                searchMonsters.Items.Add(result);
            }

        }

        private void searchMonsters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sourceMap.Text == string.Empty || targetMap.Text == string.Empty)
            {
                return;
            }

            DungeonMapRecord dungeonMapRecord = new DungeonMapRecord()
            {
                Id = long.Parse(sourceMap.Text),
                Monsters = new List<short>(),
                NextMapId = long.Parse(targetMap.Text),
                RespawnDelay = RespawnDelay,
            };
            dungeonMapRecord.AddInstantElement();

            dungeonMaps.Items.Add(dungeonMapRecord);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SearchedMonster != null)
            {
                monsters.Items.Add(SearchedMonster);
                UpdateMonsterList();
            }


        }
        private void UpdateMonsterList()
        {
            List<short> result = new List<short>();

            foreach (MonsterRecord monster in monsters.Items)
            {
                result.Add((short)monster.Id);
            }

            CurrentDungeonMap.Monsters = result;
            CurrentDungeonMap.UpdateInstantElement();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MonsterRecord selectedMonster = (MonsterRecord)monsters.SelectedItem;

            if (selectedMonster != null)
            {
                CurrentDungeonMap.Monsters.Remove((short)selectedMonster.Id);
                monsters.Items.Remove(selectedMonster);

                UpdateMonsterList();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (CurrentDungeonMap != null)
            {
                CurrentDungeonMap.RemoveInstantElement();
                dungeonMaps.Items.Remove(CurrentDungeonMap);
            }
        }

        private void ReplaceSelectedClick(object sender, RoutedEventArgs e)
        {
            MonsterRecord selectedMonster = (MonsterRecord)monsters.SelectedItem;

            if (selectedMonster != null && SearchedMonster != null)
            {
                var indice = monsters.SelectedIndex;
                monsters.Items.Remove(selectedMonster);
                monsters.Items.Insert(indice, SearchedMonster);

                UpdateMonsterList();
            }
        }
    }
}
