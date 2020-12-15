using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void MapIdTextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayNpcs(int.Parse(mapId.Text));
        }
        private void DisplayNpcs(int mapId)
        {
            npcs.Items.Clear();

            foreach (var npc in NpcSpawnRecord.GetNpcsOnMap(mapId))
            {
                npcs.Items.Add(npc);
            }
        }

        private void NpcSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayActions(CurrentNpc);
            templateId.Text = CurrentNpc.TemplateId.ToString();
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

            CurrentNpc.UpdateInstantElement();

            foreach (var action in CurrentNpc.Actions)
            {
                action.UpdateInstantElement();
            }
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
            }
        }
    }
}
