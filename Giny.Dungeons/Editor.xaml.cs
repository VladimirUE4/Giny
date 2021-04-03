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

        private DungeonRecord SelectedDungeon
        {
            get
            {
                return (DungeonRecord)dungeons.SelectedItem;
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


            foreach (var dungeon in DungeonRecord.GetDungeonRecords())
            {
                dungeons.Items.Add(dungeon);
            }

            mapsCanvas.Visibility = Visibility.Hidden;
            monsterCanvas.Visibility = Visibility.Hidden;

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void dungeons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDungeon != null)
            {
                mapsCanvas.Visibility = Visibility.Visible;
                monsterCanvas.Visibility = Visibility.Hidden;

                maps.Items.Clear();

                foreach (var mapId in SelectedDungeon.Rooms.Keys)
                {
                    maps.Items.Add(mapId);
                }

                entrance.Text = SelectedDungeon.EntranceMapId.ToString();
                exit.Text = SelectedDungeon.ExitMapId.ToString();
            }
        }

        private void maps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (maps.SelectedItem != null)
            {
                long mapId = (long)maps.SelectedItem;

                monsterCanvas.Visibility = Visibility.Visible;

                monsters.Items.Clear();

                foreach (var monster in SelectedDungeon.Rooms[mapId].MonsterIds)
                {
                    monsters.Items.Add(MonsterRecord.GetMonsterRecord(monster));
                }

                MapPositionRecord positionRecord = MapPositionRecord.GetMapPosition(mapId);

                mapName.Content = positionRecord.Name;
            }
        }

        private void monsters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (monsters.SelectedItem != null)
            {
                MonsterRecord monsterId = (MonsterRecord)monsters.SelectedItem;

            }
        }

        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var results = MonsterRecord.GetMonsterRecords().Where(x => x.Name.ToLower().Contains(searchText.Text.ToLower()) || x.Id.ToString() == searchText.Text);

            searchMonsters.Items.Clear();

            foreach (var result in results)
            {
                searchMonsters.Items.Add(result);
            }
        }

        private void addDungeon_Click(object sender, RoutedEventArgs e)
        {
            DungeonRecord record = new DungeonRecord();

            record.Id = TableManager.Instance.PopId<DungeonRecord>();

            record.Name = djName.Text;
            record.Rooms = new Dictionary<long, MonsterRoom>();

            record.EntranceMapId = 0;
            record.ExitMapId = 0;

            record.AddInstantElement();

            dungeons.Items.Add(record);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            long id = long.Parse(mapId.Text);

            if (SelectedDungeon.Rooms.ContainsKey(id))
            {
                return;
            }
            SelectedDungeon.Rooms.Add(id, new MonsterRoom(RespawnDelay));
            SelectedDungeon.UpdateInstantElement();
            maps.Items.Add(id);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (maps.SelectedItem != null)
            {
                long mapId = (long)maps.SelectedItem;
                SelectedDungeon.Rooms.Remove(mapId);
                maps.Items.Remove(maps.SelectedItem);
                SelectedDungeon.UpdateInstantElement();
            }
        
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SearchedMonster != null)
            {
                long mapId = (long)maps.SelectedItem;
                SelectedDungeon.Rooms[mapId].MonsterIds.Add((short)SearchedMonster.Id);
                SelectedDungeon.UpdateInstantElement();
                monsters.Items.Add(SearchedMonster);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (SelectedDungeon != null)
            {
                SelectedDungeon.RemoveInstantElement();
                dungeons.Items.Remove(SelectedDungeon);
            }
        }

        private void entrance_LostFocus(object sender, RoutedEventArgs e)
        {
            SelectedDungeon.EntranceMapId = long.Parse(entrance.Text);
            SelectedDungeon.UpdateInstantElement();
        }

        private void exit_LostFocus(object sender, RoutedEventArgs e)
        {
            SelectedDungeon.ExitMapId = long.Parse(exit.Text);
            SelectedDungeon.UpdateInstantElement();
        }
    }
}
