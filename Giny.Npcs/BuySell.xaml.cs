using Giny.World.Records.Items;
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
    /// Logique d'interaction pour BuySell.xaml
    /// </summary>
    public partial class BuySell : UserControl
    {
        private NpcActionRecord Action
        {
            get;
            set;
        }

        public BuySell(NpcActionRecord action)
        {
            this.Action = action;

            InitializeComponent();
            DisplayItems();

        }

        private void DisplayItems()
        {
            items.Items.Clear();

            foreach (var itemStr in Action.Param1.Split(','))
            {
                short itemId = short.Parse(itemStr);

                ItemRecord item = ItemRecord.GetItem(itemId);

                items.Items.Add(item);
            }
        }
    }
}
