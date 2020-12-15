using Giny.ORM;
using Giny.Protocol.Custom.Enums;
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

namespace Giny.Npcs
{
    /// <summary>
    /// Logique d'interaction pour Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public Editor()
        {
            InitializeComponent();
            SearchNpcs();
        }

        private NpcSpawnRecord CurrentNpc
        {
            get
            {
                return (NpcSpawnRecord)npcs.SelectedItem;
            }
        }
        private NpcActionRecord CurrentAction
        {
            get
            {
                return (NpcActionRecord)actions.SelectedItem;
            }
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchNpcs();
        }
        private void SearchNpcs()
        {
            if (searchText.Text != string.Empty)
            {
                var npcs = NpcSpawnRecord.GetNpcSpawns().Where(x => x.ToString().ToLower().Contains(searchText.Text));
                DisplayNpcs(npcs);
            }
            else
            {
                DisplayNpcs(NpcSpawnRecord.GetNpcSpawns());
            }
        }
        private void DisplayNpcs(IEnumerable<NpcSpawnRecord> records)
        {
            npcs.Items.Clear();

            foreach (var npc in records)
            {
                npcs.Items.Add(npc);
            }
        }

        private void NpcSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentNpc != null)
            {
                actions.Items.Clear();
                actionsContent.Content = null;
                DisplayActions(CurrentNpc);
                templateId.Text = CurrentNpc.TemplateId.ToString();
                mapId.Text = CurrentNpc.MapId.ToString();
                cellId.Text = CurrentNpc.CellId.ToString();

                direction.Items.Clear();

                npcName.Text = CurrentNpc.Template.Name;

                foreach (var value in Enum.GetValues(typeof(DirectionsEnum)))
                {
                    direction.Items.Add(value);
                }

                direction.SelectedItem = CurrentNpc.Direction;
            }
        }

        public void DisplayActions(NpcSpawnRecord record)
        {
            actions.Items.Clear();

            foreach (var action in record.Actions)
            {
                actions.Items.Add(action);
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            CurrentNpc.TemplateId = short.Parse(templateId.Text);
        }

        private void ActionsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentAction == null)
            {
                return;
            }

            switch (CurrentAction.Action)
            {
                case NpcActionsEnum.BUY_SELL:
                    actionsContent.Content = new BuySell(CurrentAction);
                    break;
                case NpcActionsEnum.TALK:
                    actionsContent.Content = new Talk(CurrentNpc, CurrentAction);
                    break;
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
