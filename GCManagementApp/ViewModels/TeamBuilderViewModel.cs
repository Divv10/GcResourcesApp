using GCManagementApp.Enums;
using GCManagementApp.Models;
using GCManagementApp.Static;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Practices.Prism;

namespace GCManagementApp.ViewModels
{
    internal class TeamBuilderViewModel : NotifyPropertyChanged, IDropTarget
    {
        public ICommand SaveCommand { get; }
        public ICommand AddNewTeamCommand { get; }
        public ICommand RemoveAllCommand { get; }
        public ICommand SaveToClipboardCommand { get; }
        public ICommand RemoveTeamCommand { get; }
        public ICommand ClearRankCommand { get; }

        public ContentAttributeEnum[] Elements { get; } = Enum.GetValues<ContentAttributeEnum>();

        private ObservableCollection<ContentTeam> _contentTeams;
        public ObservableCollection<ContentTeam> ContentTeams
        {
            get => _contentTeams;
            set => SetProperty(ref _contentTeams, value);
        }

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

        private bool _isPrintMode;
        public bool IsPrintMode
        {
            get => _isPrintMode;
            set => SetProperty(ref _isPrintMode, value);
        }

        private string _contentText;
        public string ContentText
        {
            get => _contentText;
            set => SetProperty(ref _contentText, value);
        }

        public TeamBuilderViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            AddNewTeamCommand = new RelayCommand(AddNewTeam);
            RemoveAllCommand = new RelayCommand(RemoveAll);
            SaveToClipboardCommand = new RelayCommand(SaveToClipboard);
            RemoveTeamCommand = new RelayCommand(RemoveTeam);
            ClearRankCommand = new RelayCommand(ClearRank);

            Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);

            Initialize();

            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.SiLevel), ListSortDirection.Descending));
            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.ChaserLevel), ListSortDirection.Descending));
            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
        }

        private void Initialize()
        {
            ContentTeams = new ObservableCollection<ContentTeam>();

            var cts = ProfileGrowth.Profile.ContentTeams;
            foreach (var ct in cts)
            {
                var currentHeroes = new List<HeroGrowth>();
                foreach (var h in ct.Heroes)
                {
                    var hero = Heroes.FirstOrDefault(x => x.Hero.HeroType == h.HeroType && x.Hero.HeroName == h.HeroName);
                    Heroes.Remove(hero);
                    currentHeroes.Add(hero);
                }
                ContentTeams.Add(new ContentTeam() { Element = ct.Element, Heroes = new ObservableCollection<HeroGrowth>(currentHeroes) });
            }
            ContentText = ProfileGrowth.Profile.ContentTeamName;
        }

        private void Save(object param)
        {
            ProfileGrowth.Profile.ContentTeams = ContentTeams.Select(x => new SerializableContentTeam() { Element = x.Element, Heroes = x.Heroes.Select(y => y.Hero).ToList()}).ToList();
            ProfileGrowth.Profile.ContentTeamName = ContentText;
            ProfileGrowth.Profile.SaveToJson();
        }

        private void AddNewTeam(object param)
        {
            if (ContentTeams.Count() < 4)
            {
                ContentTeams.Add(new ContentTeam());
            }
        }

        private void RemoveAll(object param)
        {
            foreach(var ct in ContentTeams)
            {
                foreach(var h in ct.Heroes)                
                    Heroes.Add(h);                
            }
            ContentTeams.Clear();
        }

        private async void SaveToClipboard(object param)
        {
            //VulcaTeamScroller.ScrollToHome();
            //await Task.Delay(1000);

            if (param is Grid ContentTeams)
            {
                IsPrintMode = true;

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)ContentTeams.ActualWidth, (int)ContentTeams.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(ContentTeams);

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

                IsPrintMode = false;
            }
        }

        private void RemoveTeam(object param)
        {
            if (param is ContentTeam ct)
            {
                ContentTeams.Remove(ct);
                foreach(var h in ct.Heroes)
                {
                    Heroes.Add(h);
                }
            }
        }

        private void ClearRank(object param)
        {
            if (param is ContentTeam ct)
            {                
                foreach (var h in ct.Heroes)
                {
                    Heroes.Add(h);
                }
                ct.Heroes.Clear();
            }

        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.TargetCollection == HeroesView)
            {
                dropInfo.Effects = DragDropEffects.Move;
                return;
            }
            if (dropInfo.TargetCollection.Cast<HeroGrowth>().Count() >= 4 && dropInfo.TargetCollection != dropInfo.DragInfo.SourceCollection)
            {
                dropInfo.Effects = DragDropEffects.None;
                return;
            }
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is HeroGrowth hero && dropInfo.TargetCollection is ObservableCollection<HeroGrowth> col)
            {
                col.Add(hero);
                if (dropInfo.DragInfo.SourceCollection is ObservableCollection<HeroGrowth> source)
                    source.Remove(hero);
                else
                    Heroes.Remove(hero);
            }
            else if (dropInfo.Data is HeroGrowth h && dropInfo.TargetCollection == HeroesView && dropInfo.TargetCollection != dropInfo.DragInfo.SourceCollection)
            {
                Heroes.Add(h);
                ((ObservableCollection<HeroGrowth>)dropInfo.DragInfo.SourceCollection).Remove(h);
            }
        }
    }
}
