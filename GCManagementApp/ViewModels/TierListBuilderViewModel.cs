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
    internal class TierListBuilderViewModel : NotifyPropertyChanged, IDropTarget
    {
        public static SolidColorBrush[] TiersColors { get; } = new[]
        {
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fbb4ae")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b3cde3")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ccebc5")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#decbe4")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fed9a6")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffcc")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e5d8bd")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f2f2f2")),
        };

        public ICommand SaveCommand { get; }
        public ICommand AddNewTeamCommand { get; }
        public ICommand RemoveAllCommand { get; }
        public ICommand SaveToClipboardCommand { get; }
        public ICommand RemoveTeamCommand { get; }
        public ICommand ClearRankCommand { get; }

        private ObservableCollection<TierListTeam> _tierListTeams;
        public ObservableCollection<TierListTeam> TierListTeams
        {
            get => _tierListTeams;
            set => SetProperty(ref _tierListTeams, value);
        }

        public ICollectionView HeroesView
        {
            get { return CollectionViewSource.GetDefaultView(Heroes); }
        }

        private ObservableCollection<Hero> _heroes = null!;
        public ObservableCollection<Hero> Heroes
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

        private string _creditsText;
        public string CreditsText
        {
            get => _creditsText;
            set => SetProperty(ref _creditsText, value);
        }

        public TierListBuilderViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            AddNewTeamCommand = new RelayCommand(AddNewTeam);
            RemoveAllCommand = new RelayCommand(RemoveAll);
            SaveToClipboardCommand = new RelayCommand(SaveToClipboard);
            RemoveTeamCommand = new RelayCommand(RemoveTeam);
            ClearRankCommand = new RelayCommand(ClearRank);

            Heroes = new ObservableCollection<Hero>(ProfileGrowth.Heroes.Select(x => x.Hero));

            Initialize();

            HeroesView.SortDescriptions.Add(new SortDescription(nameof(Hero.DisplayName), ListSortDirection.Ascending));
        }

        private void Initialize()
        {
            TierListTeams = new ObservableCollection<TierListTeam>();

            var cts = ProfileGrowth.Profile.TierListTeams;
            foreach (var ct in cts)
            {
                var currentHeroes = new List<Hero>();
                foreach (var h in ct.Heroes)
                {
                    var hero = Heroes.FirstOrDefault(x => x.HeroType == h.HeroType && x.HeroName == h.HeroName);
                    Heroes.Remove(hero);
                    currentHeroes.Add(hero);
                }
                TierListTeams.Add(new TierListTeam() { Label = ct.Label, LabelColorHex = ct.LabelColorHex, LabelColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ct.LabelColorHex)), Heroes = new ObservableCollection<Hero>(currentHeroes) });
            }
            CreditsText = ProfileGrowth.Profile.TierListCreditsText;
        }

        private void Save(object param)
        {
            ProfileGrowth.Profile.TierListTeams = TierListTeams.ToList();
            ProfileGrowth.Profile.TierListCreditsText = CreditsText;
            ProfileGrowth.Profile.SaveToJson();
        }

        private void AddNewTeam(object param)
        {
            if (TierListTeams.Count() < 8)
            {
                TierListTeams.Add(new TierListTeam { Heroes = new ObservableCollection<Hero>(), Label = "S", LabelColor = TiersColors[TierListTeams.Count()], LabelColorHex = $"#{TiersColors[TierListTeams.Count()].Color.ToString().Substring(3)}" });
            }
        }

        private void RemoveAll(object param)
        {
            foreach(var ct in TierListTeams)
            {
                foreach(var h in ct.Heroes)                
                    Heroes.Add(h);                
            }
            TierListTeams.Clear();
        }

        private void SaveToClipboard(object param)
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
            if (param is TierListTeam ct)
            {
                TierListTeams.Remove(ct);
                foreach(var h in ct.Heroes)
                {
                    Heroes.Add(h);
                }
            }
        }

        private void ClearRank(object param)
        {
            if (param is TierListTeam ct)
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
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is Hero hero && dropInfo.TargetCollection is ObservableCollection<Hero> col)
            {
                col.Add(hero);
                if (dropInfo.DragInfo.SourceCollection is ObservableCollection<Hero> source)
                    source.Remove(hero);
                else
                    Heroes.Remove(hero);
            }
            else if (dropInfo.Data is Hero h && dropInfo.TargetCollection == HeroesView && dropInfo.TargetCollection != dropInfo.DragInfo.SourceCollection)
            {
                Heroes.Add(h);
                ((ObservableCollection<Hero>)dropInfo.DragInfo.SourceCollection).Remove(h);
            }
        }
    }
}
