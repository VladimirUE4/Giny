﻿using Giny.IO.D2O;
using Giny.IO.D2OClasses;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
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

            foreach (var itemType in Enum.GetValues(typeof(ItemTypeEnum)))
            {
                itemTypes.Items.Add(itemType);
            }

        }

        private void DisplayItems()
        {
            items.Items.Clear();

            if (Action.Param1 != string.Empty)
            {
                foreach (var itemStr in Action.Param1.Split(','))
                {
                    short itemId = short.Parse(itemStr);

                    Item item = D2OManager.GetObject<Item>("Items.d2o", itemId);
                    string name = Loader.D2IFile.GetText((int)item.NameId);
                    items.Items.Add(new ExchangeItem(itemId, name));
                }
            }

            tokenId.Text = Action.Param2;
        }

        private void itemId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (itemId.Text == string.Empty)
            {
                return;
            }

            int id = int.Parse(itemId.Text);

            if (D2OManager.ObjectExists("Items.d2o", id))
            {
                var item = D2OManager.GetObject<Item>("Items.d2o", id);
                itemName.Text = Loader.D2IFile.GetText((int)item.NameId);
            }
            else
            {
                itemName.Text = string.Empty;
            }

        }

        private void UpdateItems()
        {
            StringBuilder sb = new StringBuilder();

            int index = 0;

            foreach (ExchangeItem item in items.Items)
            {
                sb.Append(item.Id);

                if (index != items.Items.Count - 1)
                {
                    sb.Append(",");
                }
                index++;
            }

            Action.Param1 = sb.ToString();
            Action.Param2 = tokenId.Text;

            Action.UpdateInstantElement();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (ExchangeItem)items.SelectedItem;
            items.Items.Remove(item);

            UpdateItems();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(itemId.Text);

            if (D2OManager.ObjectExists("Items.d2o", id))
            {
                var item = D2OManager.GetObject<Item>("Items.d2o", id);
                var text = Loader.D2IFile.GetText((int)item.NameId);

                ExchangeItem eitem = new ExchangeItem(item.id, text);
                items.Items.Add(eitem);

                UpdateItems();

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var type = (ItemTypeEnum)itemTypes.SelectedItem;

            var objects = D2OManager.GetObjects("Items.d2o");

            List<Item> added = new List<Item>();

            foreach (Item item in objects)
            {
                if (item.TypeId == (uint)type)
                {
                    added.Add(item);
                }
            }

            foreach (var d2oItem in added)
            {
                var text = Loader.D2IFile.GetText((int)d2oItem.NameId);
                ExchangeItem eitem = new ExchangeItem(d2oItem.id, text);
                items.Items.Add(eitem);
            }

            UpdateItems();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            items.Items.Clear();
            UpdateItems();
        }

        private void tokenId_TextChanged(object sender, TextChangedEventArgs e)
        {
            Action.Param2 = tokenId.Text;
            Action.UpdateInstantElement();
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
