using Giny.IO.D2O;
using Giny.IO.D2OClasses;
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

                Item item = D2OManager.GetObject<Item>("Items.d2o", itemId);
                string name = Loader.D2IFile.GetText((int)item.NameId);
                items.Items.Add(new ExchangeItem(itemId, name));
            }

            tokenId.Text = Action.Param2;
        }

    }
    public class ExchangeItem
    {
        private string Name
        {
            get;
            set;
        }
        public int Id
        {
            get;
            set;
        }

        public ExchangeItem(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public override string ToString()
        {
            return "(" + Id + ") " + Name;
        }
    }
}
