using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GCManagementApp.TemplateSelectors
{
    public class MassEditTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Transcendence { get; set; }
        public DataTemplate Level { get; set; }
        public DataTemplate SoulImprint { get; set; }
        public DataTemplate Chaser { get; set; }
        public DataTemplate Pet { get; set; }
        public DataTemplate Necklace { get; set; }
        public DataTemplate Earring { get; set; }
        public DataTemplate Ring { get; set; }
        public DataTemplate EW { get; set; }
        public DataTemplate Artifact { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var enumValue = (MassEditTypeEnum)item;

            switch (enumValue)
            {
                default: return Level;
                case MassEditTypeEnum.Transcendence: return Transcendence;
                case MassEditTypeEnum.SoulImprint: return SoulImprint;
                case MassEditTypeEnum.Chaser: return Chaser;
                case MassEditTypeEnum.Pet: return Pet;
                case MassEditTypeEnum.AccessoryNecklace: return Necklace;
                case MassEditTypeEnum.AccessoryRing: return Ring;
                case MassEditTypeEnum.AccessoryEarring: return Earring;
                case MassEditTypeEnum.ExclusiveWeapon: return EW;
                case MassEditTypeEnum.Artifact: return Artifact;
            }
        }
    }
}
