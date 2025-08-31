using ControlzEx.Theming;
using GCManagementApp.Enums;
using GCManagementApp.Models;
using GCManagementApp.Static;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
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

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for TierListView.xaml
    /// </summary>
    public partial class TierListView : UserControl, INotifyPropertyChanged
    {
        public ICommand CopyToClipboardCommand { get; }

        public string[] SiLabels { get; set; } = new string[] { Properties.Resources.SI15, String.Format(Properties.Resources.SIX, "10+"), String.Format(Properties.Resources.SIX, "5+"), String.Format(Properties.Resources.SIX, "0+"), Properties.Resources.Locked, Properties.Resources.NotOwned };

        public string[] ClLabels { get; set; } = new string[] { Properties.Resources.CL25, String.Format(Properties.Resources.CLX, "20+"), String.Format(Properties.Resources.CLX, "20"), String.Format(Properties.Resources.CLX, "0+"), String.Format(Properties.Resources.CLX, "0"), Properties.Resources.NotOwned};
       
        public string[] DLabels { get; set; } = new string[] { Properties.Resources.D10, String.Format(Properties.Resources.DX, "8+"), String.Format(Properties.Resources.DX, "5+"), String.Format(Properties.Resources.DX, "3+"), String.Format(Properties.Resources.DX, "0+"), String.Format(Properties.Resources.DX, "0"), Properties.Resources.NotOwned};

        public string[] TransLabels { get; set; } = new string[] { "T6", "T5", "T4", "T3", "T2", "T1", "T0", Properties.Resources.NotOwned };

        public string[] AccessoryLabels { get; set; } = new string[] { Properties.Resources.AllT4, Properties.Resources.AnyT4, Properties.Resources.AllT3, Properties.Resources.AnyT3, Properties.Resources.AllT2, Properties.Resources.AnyT2, Properties.Resources.AllT1, Properties.Resources.AnyT1, Properties.Resources.NotOwned };

        public string[] EWLabels { get; set; } = new string[] { "+10 - +9" ,"+8", "+5 - +7", "+4", "+0 - +3", Properties.Resources.NotOwned};

        public string[] ScLabels { get; set; } = new string[] { Properties.Resources.Heroes };

        private ObservableCollection<HeroGrowth>[] _siCollections;
        public ObservableCollection<HeroGrowth>[] SiCollections
        {
            get => _siCollections;
            set => SetProperty(ref _siCollections, value);
        }

        private ObservableCollection<HeroGrowth>[] _clCollections;
        public ObservableCollection<HeroGrowth>[] ClCollections
        {
            get => _clCollections;
            set => SetProperty(ref _clCollections, value);
        }

        private ObservableCollection<HeroGrowth>[] _dCollections;
        public ObservableCollection<HeroGrowth>[] DCollections
        {
            get => _dCollections;
            set => SetProperty(ref _dCollections, value);
        }

        private ObservableCollection<HeroGrowth>[] _transCollections;
        public ObservableCollection<HeroGrowth>[] TransCollections
        {
            get => _transCollections;
            set => SetProperty(ref _transCollections, value);
        }

        private ObservableCollection<HeroGrowth>[] _accessoryCollections;
        public ObservableCollection<HeroGrowth>[] AccessoryCollections
        {
            get => _accessoryCollections;
            set => SetProperty(ref _accessoryCollections, value);
        }

        private ObservableCollection<HeroGrowth>[] _ewCollections;
        public ObservableCollection<HeroGrowth>[] EWCollections
        {
            get => _ewCollections;
            set => SetProperty(ref _ewCollections, value);
        }

        private ObservableCollection<HeroGrowth>[] _scCollections;
        public ObservableCollection<HeroGrowth>[] ScCollections
        {
            get => _scCollections;
            set => SetProperty(ref _scCollections, value);
        }

        public TierListView()
        {
            InitializeComponent();
            DataContext = this;
            RefreshCollections();

            CopyToClipboardCommand = new RelayCommand(CopyToClipboard);

            SiList.IsVisibleChanged += IsVisibleChangedDep;
            ClList.IsVisibleChanged += IsVisibleChangedDep;
            DList.IsVisibleChanged += IsVisibleChangedDep;
            AccList.IsVisibleChanged += IsVisibleChangedDep;
            EwList.IsVisibleChanged += IsVisibleChangedDep;
        }

        private void RefreshCollections()
        {
            var heroes = ProfileGrowth.Heroes;

            var si15 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.SiLevel == 15).OrderBy(x => x.DisplayName));
            var si10p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.SiLevel < 15 && x.SiLevel >= 10).OrderByDescending(x => x.SiLevel).ThenByDescending(x => x.IsCoreOpen).ThenBy(x => x.DisplayName));
            var si5p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.SiLevel < 10 && x.SiLevel >= 5).OrderByDescending(x => x.SiLevel).ThenByDescending(x => x.IsCoreOpen).ThenBy(x => x.DisplayName));
            var si0p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.SiLevel < 5 && x.SiLevel >= 0).OrderByDescending(x => x.SiLevel).ThenByDescending(x => x.IsCoreOpen).ThenBy(x => x.DisplayName));
            var siNO = new ObservableCollection<HeroGrowth>(heroes.Where(x => !x.IsOwned).OrderBy(x => x.DisplayName));

            SiCollections = new ObservableCollection<HeroGrowth>[] {si15, si10p, si5p, si0p, siNO, null};

            var cl25 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.ChaserLevel == 25).OrderBy(x => x.DisplayName));
            var cl20p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.ChaserLevel < 25 && x.ChaserLevel > 20).OrderByDescending(x => x.ChaserLevel).ThenBy(x => x.DisplayName));
            var cl20 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.ChaserLevel == 20).OrderByDescending(x => x.ChaserLevel).ThenBy(x => x.DisplayName));
            var cl0p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.ChaserLevel < 20 && x.ChaserLevel > 0).OrderByDescending(x => x.ChaserLevel).ThenBy(x => x.DisplayName));
            var cl0 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.ChaserLevel == 0).OrderBy(x => x.DisplayName));
            var clNO = new ObservableCollection<HeroGrowth>(heroes.Where(x => !x.IsOwned).OrderBy(x => x.DisplayName));

            ClCollections = new ObservableCollection<HeroGrowth>[] { cl25, cl20p, cl20, cl0p, cl0, clNO };

            var trans6 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.TranscendenceLevel == 6).OrderByDescending(x => x.DisplayName));
            var trans5 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.TranscendenceLevel == 5).OrderByDescending(x => x.DisplayName));
            var trans4 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.TranscendenceLevel == 4).OrderByDescending(x => x.DisplayName));
            var trans3 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.TranscendenceLevel == 3).OrderByDescending(x => x.DisplayName));
            var trans2 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.TranscendenceLevel == 2).OrderByDescending(x => x.DisplayName));
            var trans1 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.TranscendenceLevel == 1).OrderByDescending(x => x.DisplayName));
            var trans0 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.TranscendenceLevel == 0).OrderBy(x => x.DisplayName));
            var transNO = new ObservableCollection<HeroGrowth>(heroes.Where(x => !x.IsOwned).OrderBy(x => x.DisplayName));

            TransCollections = new ObservableCollection<HeroGrowth>[] { trans6, trans5, trans4, trans3, trans2, trans1, trans0, transNO};

            var d10 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.DescentLevel == 10).OrderByDescending(x => x.DisplayName));
            var d8p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.DescentLevel < 10 && x.DescentLevel >= 8).OrderByDescending(x => x.DescentLevel).ThenBy(x => x.DisplayName));
            var d5p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.DescentLevel < 8 && x.DescentLevel >= 5).OrderByDescending(x => x.DescentLevel).ThenBy(x => x.DisplayName));
            var d3p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.DescentLevel < 5 && x.DescentLevel >= 3).OrderByDescending(x => x.DescentLevel).ThenBy(x => x.DisplayName));
            var d0p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.DescentLevel < 3 && x.DescentLevel > 0).OrderByDescending(x => x.DescentLevel).ThenBy(x => x.DisplayName));
            var d0 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.IsOwned && x.DescentLevel == 0).OrderByDescending(x => x.DisplayName));
            var dNO = new ObservableCollection<HeroGrowth>(heroes.Where(x => !x.IsOwned).OrderBy(x => x.DisplayName));

            DCollections = new ObservableCollection<HeroGrowth>[] { d10, d8p, d5p, d3p, d0p, d0, dNO};

            var allT4 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AllSameTier(x, AccessoryTierEnum.T4)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var anyT4 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AnyTier(x, AccessoryTierEnum.T4)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var allT3 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AllSameTier(x, AccessoryTierEnum.T3)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var anyT3 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AnyTier(x, AccessoryTierEnum.T3)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var allT2 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AllSameTier(x, AccessoryTierEnum.T2)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var anyT2 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AnyTier(x, AccessoryTierEnum.T2)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var allT1 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AllSameTier(x, AccessoryTierEnum.T1)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var anyT1 = new ObservableCollection<HeroGrowth>(heroes.Where(x => AnyTier(x, AccessoryTierEnum.T1)).OrderByDescending(x => x.TotalAccessoryUpgradeSum).ThenBy(x => x.DisplayName));
            var rest = new ObservableCollection<HeroGrowth>(heroes.Where(x => !x.IsOwned).OrderBy(x => x.DisplayName));

            AccessoryCollections = new ObservableCollection<HeroGrowth>[] { allT4, anyT4, allT3, anyT3, allT2, anyT2, allT1, anyT1, rest };

            var ew8p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.Equipment?.IsExclusiveWeaponOwned == true && x.Equipment?.ExclusiveWeaponUpgrade > 8).OrderByDescending(x => x.Equipment.ExclusiveWeaponUpgrade).ThenBy(x => x.DisplayName));
            var ew8 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.Equipment?.IsExclusiveWeaponOwned == true && x.Equipment?.ExclusiveWeaponUpgrade == 8).OrderBy(x => x.DisplayName));
            var ew4p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.Equipment?.IsExclusiveWeaponOwned == true && x.Equipment?.ExclusiveWeaponUpgrade > 4 && x.Equipment?.ExclusiveWeaponUpgrade < 8).OrderByDescending(x => x.Equipment?.ExclusiveWeaponUpgrade).ThenBy(x => x.DisplayName));
            var ew4 = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.Equipment?.IsExclusiveWeaponOwned == true && x.Equipment?.ExclusiveWeaponUpgrade == 4).OrderBy(x => x.DisplayName));
            var ew0p = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.Equipment?.IsExclusiveWeaponOwned == true && x.Equipment?.ExclusiveWeaponUpgrade >= 0 && x.Equipment?.ExclusiveWeaponUpgrade < 4).OrderByDescending(x => x.Equipment?.ExclusiveWeaponUpgrade).ThenBy(x => x.DisplayName));
            var ewNO = new ObservableCollection<HeroGrowth>(heroes.Where(x => x.Equipment?.IsExclusiveWeaponOwned == false).OrderBy(x => x.DisplayName));

            EWCollections = new ObservableCollection<HeroGrowth>[] { ew8p, ew8, ew4p, ew4, ew0p, ewNO };

            var sc = new ObservableCollection<HeroGrowth>(heroes.OrderByDescending(x => x.IsOwned).ThenByDescending(x => x.SiLevel).ThenByDescending(x => x.IsCoreOpen).ThenByDescending(x => x.TranscendenceLevel).ThenByDescending(x => x.ChaserLevel).ThenByDescending(x => x.Level).ThenBy(x => x.DisplayName));

            ScCollections = new ObservableCollection<HeroGrowth>[] { sc };
        }

        private bool AllTier(HeroGrowth heroGrowth)
        {
            return heroGrowth.IsOwned && heroGrowth.Ring.AccessoryTier == heroGrowth.Necklace.AccessoryTier && heroGrowth.Necklace.AccessoryTier == heroGrowth.Earrings.AccessoryTier;
        }

        private bool AllSameTier(HeroGrowth heroGrowth, AccessoryTierEnum tier)
        {
            return AllTier(heroGrowth) && heroGrowth.Ring.AccessoryTier == tier;
        }

        private bool AnyTier(HeroGrowth heroGrowth, AccessoryTierEnum tier)
        {
            return heroGrowth.IsOwned && !AllTier(heroGrowth) && Math.Max((int)heroGrowth.Ring.AccessoryTier, Math.Max((int)heroGrowth.Necklace.AccessoryTier, (int)heroGrowth.Earrings.AccessoryTier)) == (int)tier;
        }

        private void CopyToClipboard(object param)
        {
            if (param is UserControl tlc)
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)tlc.ActualWidth, (int)tlc.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(tlc);

                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(rtb));
                MemoryStream stream = new MemoryStream();
                png.Save(stream);

                BitmapImage image = new BitmapImage();
                stream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();

                Clipboard.SetImage(image);
            }
        }

        private void IsVisibleChangedDep(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RefreshCollections();
            }
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
