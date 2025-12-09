using GCManagementApp.Models;
using GCManagementApp.Static;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism;
using PixelLab.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for VulcanusPlannerVertUserControl.xaml
    /// </summary>
    public partial class VulcanusPlannerVertUserControl : UserControl, INotifyPropertyChanged, IDropTarget
    {
        public ICommand ClearRankCommand { get; }
        public ICommand ClearAllCommand { get; }
        public ICommand SaveToClipboardCommand { get; }
        public ICommand SaveCommand { get; }

        public ICollectionView HeroesView
        {
            get { return CollectionViewSource.GetDefaultView(Heroes); }
        }

        private ObservableCollection<HeroGrowth> _heroes = null!;
        public ObservableCollection<HeroGrowth> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        private ObservableCollection<HeroGrowth>[] _vulcanusTeam;
        public ObservableCollection<HeroGrowth>[] VulcanusTeam
        {
            get => _vulcanusTeam;
            set => SetProperty(ref _vulcanusTeam, value);
        }

        private bool _isPrintMode;
        public bool IsPrintMode
        {
            get => _isPrintMode;
            set => SetProperty(ref _isPrintMode, value);
        }

        public VulcanusPlannerVertUserControl()
        {
            InitializeComponent();
            DataContext = this;

            IsPrintMode = true;
            ClearRankCommand = new RelayCommand(ClearRank);
            ClearAllCommand = new RelayCommand(ClearAll);
            SaveCommand = new RelayCommand(Save);
            SaveToClipboardCommand = new RelayCommand(SaveToClipboard);

            Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);
            VulcanusTeam = new ObservableCollection<HeroGrowth>[]
            {
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                new ObservableCollection<HeroGrowth>(),
                Heroes
            };

            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.SiLevel), ListSortDirection.Descending));
            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.ChaserLevel), ListSortDirection.Descending));
            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));

            Initialize();

            this.IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnSweepsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Save(null);
        }

        private void Initialize()
        {
            foreach (var coll in VulcanusTeam)
            {
                coll.Clear();
            }

            Heroes.AddRange(ProfileGrowth.Heroes);

            var vt = ProfileGrowth.Profile.VulcanusTeams;
            for (int i = 0; i < vt.Count(); i++)
            {
                foreach (var h in vt[i])
                {
                    var hero = Heroes.FirstOrDefault(x => x.Hero.HeroType == h.HeroType && x.Hero.HeroName == h.HeroName);
                    VulcanusTeam[i].Add(hero);
                    Heroes.Remove(hero);
                }
            }
        } 

        private void ClearRank(object rank)
        {
            if (int.TryParse(rank?.ToString(), out int r))
            {
                foreach (var hero in VulcanusTeam[r])
                {
                    Heroes.Add(hero);
                };
                VulcanusTeam[r].Clear();
            }
        }

        private void ClearAll(object param)
        {
            foreach (var t in VulcanusTeam)
            {
                t.Clear();
            }
            Heroes.Clear();
            Heroes.AddRange(ProfileGrowth.Heroes);
        }

        private async void SaveToClipboard(object param)
        {
            VulcaTeamScroller.ScrollToHome();
            await Task.Delay(1000);
            IsPrintMode = false;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)VulcaTeams.ActualWidth, (int)VulcaTeams.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(VulcaTeams);

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

            IsPrintMode = true;
        }
        

        private void Save(object param)
        {
            ProfileGrowth.Profile.VulcanusTeams = new List<List<Hero>>();
            for (int i = 0; i < VulcanusTeam.Count() - 1; i++)
            {
                ProfileGrowth.Profile.VulcanusTeams.Add(new List<Hero>(VulcanusTeam[i].Select(x => x.Hero)));
            }

            ProfileGrowth.Profile.SaveToJson();
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.TargetCollection.Cast<HeroGrowth>().Count() >= 4 && GetTeamIndex(dropInfo.TargetCollection) != 9 && dropInfo.TargetCollection != dropInfo.DragInfo.SourceCollection || dropInfo.DragInfo.SourceCollection == dropInfo.TargetCollection) 
            {
                dropInfo.Effects = DragDropEffects.None;
                return;
            }
            dropInfo.Effects = DragDropEffects.Move;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is HeroGrowth hero)
            {
                var index = GetTeamIndex(dropInfo.TargetCollection);
                VulcanusTeam[index].Insert(dropInfo.InsertIndex, hero);

                var sourceIndex = GetTeamIndex(dropInfo.DragInfo.SourceCollection);
                VulcanusTeam[sourceIndex].Remove(hero);

            }
        }

        private int GetTeamIndex(System.Collections.IEnumerable collection)
        {
            return collection is ListCollectionView ? VulcanusTeam.Count() - 1 : VulcanusTeam.IndexOf(x => x.AsEnumerable<HeroGrowth>() == collection.Cast<HeroGrowth>());
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Initialize();
            }
        }

        #region PC

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> raiser)
        {
            var propName = ((MemberExpression)raiser?.Body)?.Member.Name;
            OnPropertyChanged(propName!);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        #endregion
    }
}
