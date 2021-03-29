using Giny.IO.D2O;
using Giny.IO.D2OClasses;
using Giny.ORM;
using Giny.ORM.Interfaces;
using Giny.World.Managers.Generic;
using Giny.World.Records.Npcs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Giny.Npcs
{
    /// <summary>
    /// Logique d'interaction pour Talk.xaml
    /// </summary>
    public partial class Talk : UserControl
    {
        private NpcActionRecord ActionRecord
        {
            get;
            set;
        }
        private NpcSpawnRecord SpawnRecord
        {
            get;
            set;
        }
        private bool ReplyLoaded
        {
            get;
            set;
        }
        public Talk(NpcSpawnRecord spawn, NpcActionRecord record)
        {
            this.ActionRecord = record;
            this.SpawnRecord = spawn;

            InitializeComponent();

            DisplayMessages();
            DisplayReplies();

            replyCanvas.Visibility = Visibility.Hidden;

            foreach (var action in Enum.GetValues(typeof(GenericActionEnum)))
            {
                actions.Items.Add(action);
            }
        }


        public void DisplayMessages()
        {
            var messageId = int.Parse(ActionRecord.Param1);

            var npcMessage = D2OManager.GetObject<NpcMessage>("NpcMessages.d2o", messageId);
            string text = Loader.D2IFile.GetText((int)npcMessage.MessageId);

            messageText.AppendText(text);
        }
        public void DisplayReplies()
        {
            var messageId = int.Parse(ActionRecord.Param1);

            Npc npc = D2OManager.GetObject<Npc>("Npcs.d2o", SpawnRecord.TemplateId);

            foreach (var replyRecord in NpcReplyRecord.GetNpcReplies((short)messageId))
            {
                var test = npc.dialogReplies.FirstOrDefault(x => x[0] == replyRecord.ReplyId);

                var text = Loader.D2IFile.GetText(test[1]);

                replies.Items.Add(new NpcReply(text, replyRecord));
            }
        }

        public void AddReply(NpcD2OReply reply)
        {
            NpcReplyRecord replyRecord = new NpcReplyRecord()
            {
                ReplyId = reply.ReplyId,
                ActionIdentifier = GenericActionEnum.None,
                Id = 0,
                MessageId = int.Parse(ActionRecord.Param1),
            };

            replyRecord.AddInstantElement();

            replies.Items.Add(new NpcReply(reply.Text, replyRecord));
        }

        private void RepliesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReplyLoaded = false;

            replyCanvas.Visibility = Visibility.Visible;

            NpcReply reply = (NpcReply)replies.SelectedItem;

            if (reply != null)
            {
                actions.SelectedItem = reply.Record.ActionIdentifier;
                param1.Text = reply.Record.Param1;
                param2.Text = reply.Record.Param2;
                param3.Text = reply.Record.Param3;
                criterias.Text = reply.Record.Criteria;
                ReplyLoaded = true;
            }
            else
            {
                replyCanvas.Visibility = Visibility.Hidden;
            }
        }

        private void ActionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            param1.Text = string.Empty;
            param2.Text = string.Empty;
            param3.Text = string.Empty;
            criterias.Text = string.Empty;
            UpdateReply();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var replyDialog = new AddReply(this, SpawnRecord.TemplateId);
            replyDialog.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((NpcReply)replies.SelectedItem).Record.RemoveInstantElement();
            replies.Items.Remove(replies.SelectedItem);

        }

        private void UpdateReply()
        {
            if (!ReplyLoaded)
            {
                return;
            }
            var reply = (NpcReply)replies.SelectedItem;

            reply.Record.Param1 = param1.Text;
            reply.Record.Param2 = param2.Text;
            reply.Record.Param3 = param3.Text;
            reply.Record.Criteria = criterias.Text;
            reply.Record.ActionIdentifier = (GenericActionEnum)actions.SelectedItem;
            reply.Record.UpdateInstantElement();
        }
        private void param1_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateReply();
        }

        private void param2_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateReply();
        }

        private void param3_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateReply();
        }

        private void criterias_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateReply();
        }
    }
    public class NpcReply
    {
        public string Text
        {
            get;
            set;
        }
        public NpcReplyRecord Record
        {
            get;
            set;
        }

        public NpcReply(string text, NpcReplyRecord record)
        {
            this.Text = text;
            this.Record = record;
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
