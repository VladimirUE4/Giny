using Giny.Protocol.Enums;
using Giny.World.Managers.Fights.Effects;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Giny.SpellExplorer
{
    /// <summary>
    /// Logique d'interaction pour NotHandledEffects.xaml
    /// </summary>
    public partial class NotHandledEffects : Window
    {
        Dictionary<EffectsEnum, List<SpellRecord>> Unhandled = new Dictionary<EffectsEnum, List<SpellRecord>>();

        private CancellationTokenSource CancelSource
        {
            get;
            set;
        }
        public NotHandledEffects()
        {

            InitializeComponent();

            CancelSource = new CancellationTokenSource();

            effects.SelectionChanged += Effects_SelectionChanged;
            spells.SelectionChanged += Spells_SelectionChanged;
            Task.Run(() =>
            {
                Load();
            });

        }

        private void Spells_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (spells.SelectedItem != null)
            {
                CastSpell castSpell = new CastSpell((SpellRecord)spells.SelectedItem);
                castSpell.Show();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            CancelSource.Cancel();
            base.OnClosed(e);
        }
        private void Load()
        {

            var spells = SpellRecord.GetSpellRecords();
            int i = 0;
            int n = spells.Count();

            foreach (var spell in spells)
            {
                foreach (var level in SpellLevelRecord.GetSpellLevels(spell.Id))
                {
                    foreach (var effect in level.Effects)
                    {
                        if (SpellEffectManager.Instance.Exists(effect.EffectEnum))
                        {
                            continue;
                        }
                        if (!Unhandled.ContainsKey(effect.EffectEnum))
                        {
                            Unhandled.Add(effect.EffectEnum, new List<SpellRecord>());

                            this.Dispatcher.Invoke(() =>
                            {
                                effects.Items.Add(effect.EffectEnum.ToString());
                                count.Content = "Count : " + Unhandled.Count;
                            });
                        }

                        Unhandled[effect.EffectEnum].Add(spell);

                    }
                }
                i++;
                
                if (CancelSource.IsCancellationRequested)
                {
                    break;
                }
                this.Dispatcher.Invoke(() =>
                {
                    progress.Value = (i / (double)n) * 100d;
                });

            }
        }

        private void Effects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EffectsEnum effect = 0;

            if (Enum.TryParse<EffectsEnum>(effects.SelectedItem.ToString(), out effect))
            {

                spells.Items.Clear();

                foreach (var spell in Unhandled[effect])
                {
                    spells.Items.Add(spell);
                }
            }
            else
            {
                MessageBox.Show("Unknown Effects Enum " + effects.SelectedItem.ToString());
            }
        }
    }
}
