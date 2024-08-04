using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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

namespace GCManagementApp.Windows
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class DataSyncSummaryWindow : Window, INotifyPropertyChanged
    {
        public ICommand CloseCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        private List<HeroScanResult> _scanResults;
        public List<HeroScanResult> ScanResults
        {
            get => _scanResults;
            set => SetProperty(ref _scanResults, value);
        }

        public DataSyncSummaryWindow()
        {
            InitializeComponent();
            DataContext = this;

            CloseCommand = new RelayCommand(Close);
            UpdateCommand = new RelayCommand(Update);
        }

        private void Close(object param)
        {
            this.Close();
        } 

        private void Update(object param) 
        {
            foreach (var result in ScanResults)
            {
                if (!result.IsChecked)
                    continue;

                var hero = ProfileGrowth.Heroes.FirstOrDefault(x => x.Hero.HeroName == result.Hero.HeroName && x.Hero.HeroType == result.Hero.HeroType);
                if (hero != null)
                {
                    hero.IsOwned = true;
                    if (result.Si != null)
                        hero.SiLevel = result.Si.Value;
                    if (result.Chaser != null)
                        hero.ChaserLevel = result.Chaser.Value;
                    if (result.Level != null)
                        hero.Level = result.Level.Value;
                    if (result.Trans != null)
                        hero.TranscendenceLevel = result.Trans.Value;
                    if (result.Pet != null)
                        hero.PetLevel = result.Pet.Value;
                    if (result.Bp != null)
                        hero.BP = result.Bp.Value;
                    if (result.EwLevel != null)
                    {
                        hero.Equipment.IsExclusiveWeaponOwned = true;
                        hero.Equipment.ExclusiveWeaponUpgrade = result.EwLevel.Value;
                    }
                    if (result.ArtiTier > 0)
                    {
                        hero.Equipment.ArtifactTier = (ArtifactTier)result.ArtiTier;
                        if (result.ArtiLevel != null)
                            hero.Equipment.ArtifactUpgrade = result.ArtiLevel.Value;
                        if (result.ArtiType != null)
                            hero.Equipment.ArtifactType = (ArtifactType)result.ArtiType;
                    }
                    if (result.RingTier > 0)
                    {
                        hero.Ring.AccessoryTier = (AccessoryTierEnum)(result.RingTier - 1);
                        if (result.RingLevel != null)
                            hero.Ring.AccessoryUpgradeLevel = result.RingLevel.Value;
                        if (result.RingType != null)
                            hero.Ring.AccessorySet = (AccessorySetEnum)result.RingType;
                    }
                    if (result.NecklaceTier > 0)
                    {
                        hero.Necklace.AccessoryTier = (AccessoryTierEnum)(result.NecklaceTier - 1);
                        if (result.NecklaceLevel != null)
                            hero.Necklace.AccessoryUpgradeLevel = result.NecklaceLevel.Value;
                        if (result.NecklaceType != null)
                            hero.Necklace.AccessorySet = (AccessorySetEnum)result.NecklaceType;
                    }
                    if (result.EarringTier > 0)
                    {
                        hero.Earrings.AccessoryTier = (AccessoryTierEnum)(result.EarringTier - 1);
                        if (result.EarringLevel != null)
                            hero.Earrings.AccessoryUpgradeLevel = result.EarringLevel.Value;
                        if (result.EarringType != null)
                            hero.Earrings.AccessorySet = (AccessorySetEnum)result.EarringType;
                    }
                }
                
            }

            ProfileGrowth.SaveToFile();
            ProfileManager.ForceUpdate();
            
            this.Close();
        }

        #region PC

        public event PropertyChangedEventHandler PropertyChanged = null!;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> raiser)
        {
            var propName = ((MemberExpression)raiser?.Body!)?.Member.Name;
            OnPropertyChanged(propName!);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        #endregion
    }
}
