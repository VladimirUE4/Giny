using Giny.Core.Extensions;
using Giny.Core.Misc;
using Giny.IO.D2O;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Giny.DatabaseSynchronizer
{
    class MiscBuilder
    {
        public static void Build(List<D2OReader> readers)
        {
            BuildOptionalFeatures(readers);
            return;

            BuildTextInformations(readers);

            BuildSpellEnum(readers);

            BuildJobType(readers);
            BuildSkillsType(readers);
            BuildInteractiveTypes(readers);
            BuildItemTypes(readers);
            BuildMonsterRaces(readers);
            BuildNpcActions(readers);
            BuildNpcsDialogs(readers);
        }
        private static void BuildOptionalFeatures(List<D2OReader> readers)
        {
            var opts = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "OptionalFeature")).EnumerateObjects().Cast<Giny.IO.D2OClasses.OptionalFeature>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Optional Featuers*");

            foreach (var opt in opts)
            {

                string keyword = opt.keyword.Replace(".", "").Replace("-", "").FirstCharToUpper();

                sb.AppendLine(keyword + "=" + opt.id + ",");
            }

            Notepad.Open(sb.ToString());
        }
        private static void BuildSpellEnum(List<D2OReader> readers)
        {
            var spells = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "Spell")).EnumerateObjects().Cast<Giny.IO.D2OClasses.Spell>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Spells*");

            Dictionary<string, int> values = new Dictionary<string, int>();

            foreach (var spell in spells)
            {
                string raw = Program.D2IFileEN.GetText((int)spell.nameId);

                string[] removed = new string[]
                {
                    "'","-","!","(",")","/","\"",".","?","²","$",
                    "*","%","[","]" ,":","–","+",","
                };
                foreach (var element in removed)
                {
                    raw = raw.Replace(element, "");
                }

                raw = Regex.Replace(raw, @"\s+", "");
                raw = Regex.Replace(raw, @"[\d-]", string.Empty);

                if (raw.Contains("UNKNOWN") || raw == string.Empty)
                {
                    raw = "Unknown" + spell.id.ToString();
                }

                if (values.ContainsKey(raw))
                {
                    raw = raw + spell.id;
                }

                values.Add(raw, spell.id);
            }


            foreach (var pair in values)
            {
                sb.AppendLine(pair.Key + "=" + pair.Value + ",");
            }



            Notepad.Open(sb.ToString());
        }
        private static void BuildTextInformations(List<D2OReader> readers)
        {
            var msgs = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "InfoMessage")).EnumerateObjects().Cast<Giny.IO.D2OClasses.InfoMessage>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Information Messages*");

            foreach (var msg in msgs)
            {
                sb.AppendLine(msg.textId + "->" + Program.D2IFileFR.GetText((int)msg.messageId));
            }

            Notepad.Open(sb.ToString());
        }
        private static void BuildJobType(List<D2OReader> readers)
        {
            var skills = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "Job")).EnumerateObjects().Cast<Giny.IO.D2OClasses.Job>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Job Types*");

            foreach (var skill in skills)
            {
                sb.AppendLine(Program.D2IFileEN.GetText((int)skill.NameId).ToUpper().Replace(" ", "_") + "=" + skill.id + ",");
            }

            Notepad.Open(sb.ToString());
        }
        private static void BuildSkillsType(List<D2OReader> readers)
        {
            var skills = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "Skill")).EnumerateObjects().Cast<Giny.IO.D2OClasses.Skill>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Interactive Skills*");

            foreach (var skill in skills)
            {
                sb.AppendLine(Program.D2IFileEN.GetText((int)skill.NameId).ToUpper().Replace(" ", "_") + skill.id + "=" + skill.id + ",");
            }

            Notepad.Open(sb.ToString());
        }
        private static void BuildInteractiveTypes(List<D2OReader> readers)
        {
            var interactives = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "Interactive")).EnumerateObjects().Cast<Giny.IO.D2OClasses.Interactive>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Interactive Types*");

            foreach (var interactive in interactives)
            {
                sb.AppendLine(Program.D2IFileFR.GetText((int)interactive.NameId).ToUpper().Replace(" ", "_") + "=" + interactive.id + ",");
            }

            Notepad.Open(sb.ToString());
        }
        private static void BuildNpcsDialogs(List<D2OReader> readers)
        {
            var npcs = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "Npc")).EnumerateObjects().Cast<Giny.IO.D2OClasses.Npc>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*Npc Messages*");
            foreach (var npc in npcs)
            {
                var l1 = ((IEnumerable)npc.DialogMessages).Cast<List<int>>().ToList();

                foreach (var value in l1)
                {
                    sb.AppendLine("");
                    sb.AppendLine("NPC_ID:" + npc.id + " MessageId : " + value[0] + ":" + Program.D2IFileFR.GetText(value[1]));
                }

            }

            var sb2 = new StringBuilder();
            sb2.AppendLine("*Npc Replies*");
            foreach (var npc in npcs)
            {
                var l1 = ((IEnumerable)npc.DialogReplies).Cast<List<int>>().ToList();

                foreach (var value in l1)
                {
                    sb2.AppendLine("");
                    sb2.AppendLine("NPC_ID:" + npc.id + " ReplyId : " + value[0] + " => " + Program.D2IFileFR.GetText(value[1]));
                }

            }

            Notepad.Open(sb.ToString());
            Notepad.Open(sb2.ToString());

        }
        private static void BuildItemTypes(List<D2OReader> readers)
        {
            var types = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "ItemType")).EnumerateObjects().Cast<Giny.IO.D2OClasses.ItemType>();

            StringBuilder sb = new StringBuilder();

            sb.Append("* Item Types *");

            foreach (var type in types)
            {
                var text = Program.D2IFileEN.GetText((int)type.nameId);
                sb.AppendLine(text.ToUpper().Replace(" ", "_") + "=" + type.id + ",");
            }

            Notepad.Open(sb.ToString());

        }
        private static void BuildMonsterRaces(List<D2OReader> readers)
        {
            var races = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "MonsterRace")).EnumerateObjects().Cast<Giny.IO.D2OClasses.MonsterRace>();

            StringBuilder sb = new StringBuilder();

            sb.Append("* Monster Races *");

            foreach (var race in races)
            {
                var text = Program.D2IFileEN.GetText((int)race.nameId);
                sb.AppendLine(text.ToUpper().Replace(" ", "_") + "=" + race.id + ",");
            }

            Notepad.Open(sb.ToString());

        }

        private static void BuildNpcActions(List<D2OReader> readers)
        {
            var actions = readers.FirstOrDefault(x => x.Classes.Any(w => w.Value.Name == "NpcAction")).EnumerateObjects().Cast<Giny.IO.D2OClasses.NpcAction>();

            StringBuilder sb = new StringBuilder();

            sb.Append("* NpcActions *");

            foreach (var action in actions)
            {
                var text = Program.D2IFileEN.GetText((int)action.nameId);

                if (text.StartsWith("[UNKNOWN"))
                {
                    text = "ITEM_TYPE_" + action.id.ToString();
                }
                sb.AppendLine(text.ToUpper().Replace(" ", "_") + "=" + action.realId + ",");
            }

            Notepad.Open(sb.ToString());

        }
    }
}
