﻿using Giny.Core.DesignPattern;
using Giny.Protocol.Custom.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Marks
{
    public class MarksManager : Singleton<MarksManager>
    {
        public Color GetMarkColorFromSpellId(SpellEnum spellId)
        {
            switch (spellId)
            {
                /* Piège Répulsif
                 * Piège de Dérive
                 * Fosse commune */
                case SpellEnum.MassGrave14314:
                case SpellEnum.RepellingTrap12914:
                case SpellEnum.DriftTrap12942:
                    return Color.FromArgb(10849205);

                /* Piège a Fragmentation
                 * Piège Sournois */
                case SpellEnum.FragmentationTrap12941:
                case SpellEnum.TrickyTrap12906:
                    return Color.FromArgb(12128795);
                /*
                 * Piège de masse
                 * Piège Scélérat */
                case SpellEnum.MassTrap12920:
                case SpellEnum.SickratTrap:
                    return Color.FromArgb(5911580);

                /*
                 * Piège Fangeux
                 * Calamité */
                case SpellEnum.MiryTrap12916:
                case SpellEnum.Calamity12950:
                    return Color.FromArgb(4228004);
                /*
                 * Piège Mortel
                 * Piège Funeste */
                case SpellEnum.LethalTrap12921:
                case SpellEnum.MalevolentTrap12948:
                    return Color.FromArgb(0);

                /*
                 * Piège Insidieux
                 */
                case SpellEnum.InsidiousTrap12918:
                    return Color.FromArgb(0);

                /*
                 * Piège d'Immobilisation
                 */
                case SpellEnum.ParalysingTrap12910:
                    return Color.FromArgb(2258204);
                    
                /*
                 * Runes Huppermage 
                 */
                case SpellEnum.EarthRune:
                    return Color.Brown;
                case SpellEnum.FireRune13687:
                    return Color.Red;
                case SpellEnum.WaterRune:
                    return Color.Blue;
                case SpellEnum.AirRune:
                    return Color.Green;
            }

            return Color.CornflowerBlue;
        }
    }
}
