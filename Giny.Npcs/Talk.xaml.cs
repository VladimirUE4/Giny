using Giny.IO.D2O;
using Giny.IO.D2OClasses;
using Giny.World.Managers.Generic;
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

        private void RepliesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            replyCanvas.Visibility = Visibility.Visible;

            NpcReply reply = (NpcReply)replies.SelectedItem;
            actions.SelectedItem = reply.Record.ActionIdentifier;

            param1.Text = reply.Record.Param1;
            param2.Text = reply.Record.Param2;
            param3.Text = reply.Record.Param3;
            criterias.Text = reply.Record.Criteria;
        }

        private void ActionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            param1.Text = string.Empty;
            param2.Text = string.Empty;
            param3.Text = string.Empty;
            criterias.Text = string.Empty;
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
