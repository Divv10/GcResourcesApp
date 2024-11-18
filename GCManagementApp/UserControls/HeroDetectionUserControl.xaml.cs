using GCManagementApp.Models;
using GCManagementApp.Helpers;
using GCManagementApp.Operations;
using GCManagementApp.Static;
using GCManagementApp.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GCManagementApp.Enums;
using System.Drawing;
using MahApps.Metro.Controls;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Emgu.CV;
using Serilog;
using MaterialDesignThemes.Wpf;
using TesseractOCR.Renderers;
using Org.BouncyCastle.Utilities;
using System.IO;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using GCManagementApp.Exceptions;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for ProcessAttachUserControl.xaml
    /// </summary>
    public partial class HeroDetectionUserControl : UserControl, INotifyPropertyChanged
    {
        public ICommand StartCommand { get; }
        public ICommand SingleScanCommand { get; }

        public bool IsConnected => EmulatorConnectionInfo.IsConnected;

        private int _heroesCount;
        public int HeroesCount
        {
            get => _heroesCount;
            set
            {
                SetProperty(ref _heroesCount, value);
                OnPropertyChanged(nameof(HeroesCountFormatted));
            }
        }

        private bool _saveScreenshots;
        public bool SaveScreenshots
        {
            get => _saveScreenshots;
            set => SetProperty(ref _saveScreenshots, value);
        }

        public List<Hero> HeroesCollection { get; } = ProfileGrowth.Heroes.Select(h => h.Hero).OrderBy(h => h.DisplayName).ToList();

        private Hero _selectedHero;
        public Hero SelectedHero
        {
            get => _selectedHero;
            set => SetProperty(ref _selectedHero, value);
        }

        public string HeroesCountFormatted
        {
            get
            {
                if (HeroesCount <= 8)
                    return $"Thats {HeroesCount} heroes from first row";
                var fullRows = HeroesCount / 8;
                var lastRow = HeroesCount % 8;

                if (lastRow == 0)
                    return $"Thats {fullRows} full rows";
                return $"Thats {fullRows} full rows and {lastRow} heroes from last row";
            }
        }

        public List<HeroScanResult> ScanResults { get; set; } = new List<HeroScanResult>();

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set 
            { 
                SetProperty(ref _isRunning, value); 
                OnPropertyChanged(nameof(StartTestContent));

                if (value)
                {
                    
                }
                else
                {
                    
                }
            }
        }

        private CancellationTokenSource _cancellationToken;


        public string StartTestContent => IsRunning ? "Stop" : "Start consecutive scan";

        public HeroDetectionUserControl()
        {
            InitializeComponent();
            DataContext = this;

            SaveScreenshots = false;
            StartCommand = new RelayCommand(Start);
            SingleScanCommand = new RelayCommand(SingleScan);
        }

        private void Start(object param)
        {
            if (IsRunning)
            {
                _cancellationToken.Cancel();
                IsRunning = false;
            }
            else
            {
                _cancellationToken = new CancellationTokenSource();
                IsRunning = true;
                ConsecutiveScan();
            }
        }

        private async void ConsecutiveScan()
        {
            try
            {
                var alg = (HeroMatchAlgorithmEnum)Properties.Settings.Default.Algorithm;

                if (!IsConnected)
                    return;

                var wndsCount = EmulatorConnectionInfo.RegionWindowList.Count;
                for (int w = wndsCount - 1; w >= 0; w--)
                {
                    try
                    {
                        EmulatorConnectionInfo.RegionWindowList[w].Close();
                    }
                    catch { }
                }

                if (HeroesCount > Hero.GetHeroesCollection.Count)
                {
                    var msg = MessageBox.Show("You have entered more heroes than there are in the game. Results may be wrong. Are You sure?", "", MessageBoxButton.YesNo);
                    if (msg != MessageBoxResult.Yes)
                        return;
                }

                Log.Logger.Verbose($"===========================================================");
                Log.Logger.Verbose($"Starting recognition. Heroes count: {HeroesCount}");
                ScanResults = new List<HeroScanResult>();
                var currentHeroArea = ImageRegions.FirstHeroArea;
                var j = 0;
                for (int i = 0; i < HeroesCount; i++)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        throw new ScanningCanceledException();

                    Log.Logger.Verbose($"... Taking screenshot...");
                    var img = (await ImageOperations.GetBitmapSourceFromScreen()).ToBitmap();
                    var heroTypeImg = img.CropImage(currentHeroArea);
                    var heroTypeLocation = GetHeroType(heroTypeImg);
                    if (!heroTypeLocation.IsEmpty)
                    {
                        Log.Logger.Verbose($"... Hero type found...");
                        //var nameArea = TextRegions.HeroNameArea;
                        var nameArea = ImageRegions.HeroNameArea;
                        nameArea.Offset(currentHeroArea.Location);
                        nameArea.Offset(heroTypeLocation.Location);

                        Log.Logger.Verbose($"... Checking hero name...");
                        //var hero = GetHeroFromName(img, nameArea);
                        var hero = alg == HeroMatchAlgorithmEnum.Quick ? GetHeroFromNameWithIR(img, nameArea) : GetHeroFromNamePrecise(img, nameArea);
                        if (hero != null)
                        {
                            Debug.WriteLine(hero.DisplayName);
                            Log.Verbose($"------------------------------------------");
                            Log.Verbose($"Hero: {hero.DisplayName}");
                            var clickLocation = heroTypeLocation.Location;
                            clickLocation.Offset(currentHeroArea.Location);
                            clickLocation.Offset(ClickRegions.HeroPortraitOffset);
                            await AdbOperations.Click(clickLocation);
                            await EmguImageOperations.FindImageInRepeat(ImageResources.HomeButton, ImageRegions.HomeArea, 0.88);                            

                            await Task.Delay(500);

                            if (_cancellationToken.IsCancellationRequested)
                                throw new ScanningCanceledException();
                            Log.Verbose("... Before Scan Hero");
                            await ScanHero(hero);
                            Log.Verbose("... After Scan Hero");
                        }
                        else
                        {
                            Log.Verbose("Can't match the hero name");
                            Debug.WriteLine("Can't match the hero name");
                        }
                    }
                    else
                    {
                        //heroTypeImg.To`ource().DisplayImage();
                        Log.Verbose("Can't match the hero type"); 
                        Debug.WriteLine("Can't match the hero type");
                    }

                    Log.Logger.Verbose($"... Iterating to next hero...");
                    currentHeroArea.Offset(ImageRegions.HorizontalGap);

                    j++;
                    if (j != 0 && j % 8 == 0 && i + 1 < HeroesCount)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                            throw new ScanningCanceledException();

                        Log.Logger.Verbose($"... Swiping to next row...");
                        Debug.WriteLine($"j={j} ; swiping");
                        await Swipe();
                        await Task.Delay(1000);
                        currentHeroArea = ImageRegions.FirstHeroArea;
                        j = 0;
                    }
                }

                Log.Logger.Verbose($"=======================================");
                Log.Logger.Verbose($"Scanning completed!");

                if (ScanResults.Count == 0) 
                {
                    MessageBox.Show("No heroes were scanned properly.");
                }
                else
                {
                    DataSyncSummaryWindow dssw = new DataSyncSummaryWindow()
                    {
                        ScanResults = ScanResults,
                    };
                    dssw.Show();
                    dssw.Activate();
                }
            }
            catch (ScanningCanceledException)
            {
                IsRunning = false;
                MessageBox.Show("Scanning has been manually stopped");
            }
            catch (Exception e)
            {
                IsRunning = false;
                MessageBox.Show(e.Message);
            }
        }

        private async void SingleScan(object param)
        {
            try
            {
                if (SelectedHero == null)
                    return;

                ScanResults = new List<HeroScanResult>();
                await ScanHero(SelectedHero);

                if (ScanResults.Count == 0)
                {
                    MessageBox.Show("Scanning yielded no results");
                }
                else
                {
                    DataSyncSummaryWindow dssw = new DataSyncSummaryWindow()
                    {
                        ScanResults = ScanResults,
                    };
                    dssw.Show();
                    dssw.Activate();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async Task ScanHero(Hero hero)
        {
            var heroDetailsImg = (await ImageOperations.GetBitmapSourceFromScreen()).ToBitmap();
            if (SaveScreenshots)
            {
                try
                {
                    var directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Logs\\ScanImages";
                    var path = $"{directory}\\{DateTime.Now.Ticks}.png";
                    Directory.CreateDirectory(directory);
                    heroDetailsImg.Save(path, ImageFormat.Png);
                }
                catch { }
            }
            var progress = GetHeroProgress(heroDetailsImg);

            GetBp(heroDetailsImg, progress);
            GetEwLevel(heroDetailsImg, progress);
            GetArtiInfo(heroDetailsImg, progress);
            GetRingInfo(heroDetailsImg, progress);
            GetNecklaceInfo(heroDetailsImg, progress);
            GetEarringInfo(heroDetailsImg, progress);
            await GetPetLevel(progress);

            progress.Hero = hero;
            ScanResults.Add(progress);
        }

        private async Task Swipe()
        {
            Log.Logger.Verbose($"Swiping...");
            var midPoint = new System.Drawing.Point(1080 / 2, 720 / 2);
            var swipeToPoint = midPoint;
            swipeToPoint.Offset(new System.Drawing.Point(0, ImageRegions.VerticalGap));
            await AdbOperations.Swipe(swipeToPoint, midPoint, 1500, EmulatorConnectionInfo.UseBatchCmdToSwipe);
        }

        private Hero GetHeroFromNameWithOCR(Bitmap img, Rectangle nameArea)
        {
            for (int i = 0; i < 3; i++)
            {
                nameArea.Offset(i, i);
                var heroNameImg = img.CropImage(nameArea);
                var heroName = TesseractOperations.ReadText(heroNameImg.ToByteArray(ImageFormat.Png)).Trim();

                var heroType = HeroType.SR;
                if (heroName.EndsWith("(T)"))
                {
                    heroType = HeroType.T;
                    heroName = heroName.Replace("(T)", "");
                }
                if (heroName.EndsWith("(S)"))
                {
                    heroType = HeroType.S;
                    heroName = heroName.Replace("(S)", "");
                }
                if (Enum.TryParse<HeroEnum>(heroName, true, out var heroNameEnum))
                {
                    return Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == heroNameEnum && h.HeroType == heroType);
                }
            }

            return null;
        }

        private Hero GetHeroFromNameWithIR(Bitmap img, Rectangle nameArea)
        {
            var nameImg = img.CropImage(nameArea);
            //nameImg.ToImageSource().DisplayImage();
            var path = $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : "_kr")}\\HeroNames";
            foreach (var heroIcon in Directory.GetFiles(path, "*.png"))
            {
                if (EmguImageOperations.FindImageWithMask(heroIcon, nameImg, EmulatorConnectionInfo.Confidence))
                {
                    var heroName = Path.GetFileName(heroIcon).Split(".")[0];
                    var heroType = HeroType.SR;
                    if (heroName.EndsWith("T"))
                    {
                        heroType = HeroType.T;
                        heroName = heroName.Substring(0, heroName.Length - 1);
                    }
                    if (heroName.EndsWith("S"))
                    {
                        heroType = HeroType.S;
                        heroName = heroName.Substring(0, heroName.Length - 1);
                    }
                    if (Enum.TryParse<HeroEnum>(heroName, true, out var heroNameEnum))
                    {
                        return Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == heroNameEnum && h.HeroType == heroType);
                    }
                }
            }

            return null;
        }

        private Hero GetHeroFromNamePrecise(Bitmap img, Rectangle nameArea)
        {
            Dictionary<string, double> values = new Dictionary<string, double>();

            var nameImg = img.CropImage(nameArea);
            //nameImg.ToImageSource().DisplayImage();
            var path = $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : "_kr")}\\HeroNames";
            foreach (var heroIcon in Directory.GetFiles(path, "*.png"))
            {
                var score = EmguImageOperations.FindImageWithMaskScore(heroIcon, nameImg);
                values.Add(heroIcon, score);
            }

            Log.Verbose("Highest scores:");
            Log.Verbose(string.Join(Environment.NewLine, values.OrderByDescending(x => x.Value).Take(5).Select(x => $"{x.Key}: {x.Value}")));

            var heroName = Path.GetFileName(values.OrderByDescending(x => x.Value).FirstOrDefault().Key).Split(".")[0];
            var heroType = HeroType.SR;
            if (heroName.EndsWith("T"))
            {
                heroType = HeroType.T;
                heroName = heroName.Substring(0, heroName.Length - 1);
            }
            if (heroName.EndsWith("S"))
            {
                heroType = HeroType.S;
                heroName = heroName.Substring(0, heroName.Length - 1);
            }
            if (Enum.TryParse<HeroEnum>(heroName, true, out var heroNameEnum))
            {
                return Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == heroNameEnum && h.HeroType == heroType);
            }
            

            return null;
        }

        private Rectangle GetHeroType(Bitmap img)
        {
            var sr = EmguImageOperations.FindImageRegion(ImageResources.UnitSR, img, 0.8);
            if (!sr.IsEmpty)
                return sr;
            var t = EmguImageOperations.FindImageRegion(ImageResources.UnitT, img, 0.8);
            if (!t.IsEmpty)
                return t;

            return Rectangle.Empty;
        }

        private async Task GetPetLevel(HeroScanResult result)
        {
            await AdbOperations.Click(ClickRegions.PetsButton);
            await Task.Delay(1100);
            var petScreenImg = (await ImageOperations.GetBitmapSourceFromScreen()).ToBitmap();

            for (int i = 0; i < 3; i++)
            {
                var area = TextRegions.PetLevelArea;
                area.Offset(i, i);
                var petLevelImg = petScreenImg.CropImage(area);
                var lvlString = TesseractOperations.ReadText(petLevelImg.ToByteArray(ImageFormat.Png))?.Trim();
                if (lvlString?.ToLower()?.Contains("max") == true)
                {
                    result.Pet = (int)StaticValues.MaxPetLevel;
                    break;
                }
                else if (int.TryParse(lvlString?.GetNumbers(), out var lvl) && lvl > 0 && lvl <= StaticValues.MaxPetLevel)
                {
                    result.Pet = lvl;
                    break;
                }
            }

            //if (result.Pet == null)             
            //    petLevelImg.ToImageSource().DisplayImage();            

            Log.Verbose($"Pet: {result.Pet}");
            Log.Verbose($"------------------------------------------");

            await AdbOperations.Click(ClickRegions.GoBackButton);
            await Task.Delay(500);
        }

        private void GetBp(Bitmap img, HeroScanResult result)
        {
            var bpImg = img.CropImage(TextRegions.BpArea);
            for (int i = 0; i < 3; i++)
            {
                var bpString = TesseractOperations.ReadText(bpImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                if (int.TryParse(bpString, out var bp))
                {
                    result.Bp = bp;
                    break;
                }
            }
            Log.Verbose($"BP: {result.Bp}");
        }

        private void GetEwLevel(Bitmap img, HeroScanResult result)
        {
            var ewImg = img.CropImage(TextRegions.EWArea);
            for (int i = 0; i < 3; i++)
            {
                var ewString = TesseractOperations.ReadText(ewImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                if (int.TryParse(ewString, out var str))
                {
                    result.EwLevel = str;
                    break;
                }
            }
            Log.Verbose($"EW: +{result.EwLevel}");
        }

        private void GetArtiInfo(Bitmap img, HeroScanResult result)
        {
            var artiTierImg = img.CropImage(ImageRegions.ArtiTierArea);
            if (!EmguImageOperations.FindImageRegion(ImageResources.ArtiT4, artiTierImg, 0.8).IsEmpty)
                result.ArtiTier = 4;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.ArtiT3, artiTierImg, 0.8).IsEmpty)
                result.ArtiTier = 3;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.ArtiT2, artiTierImg, 0.8).IsEmpty)
                result.ArtiTier = 2;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.ArtiT1, artiTierImg, 0.8).IsEmpty)
                result.ArtiTier = 1;
            else
                result.ArtiTier = 0; // No arti

            if (result.ArtiTier > 0)
            {
                var artiImg = img.CropImage(TextRegions.ArtiLevelArea);
                for (int i = 0; i < 3; i++)
                {
                    var artiString = TesseractOperations.ReadText(artiImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                    if (int.TryParse(artiString, out var str))
                    {
                        result.ArtiLevel = str;
                        break;
                    }
                }

                var artiTypeImg = img.CropImage(ImageRegions.ArtiTypeArea);
                if (!EmguImageOperations.FindImageRegion(ImageResources.ArtiFrozen, artiTypeImg, 0.8).IsEmpty)
                    result.ArtiType = 3;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.ArtiCurse, artiTypeImg, 0.8).IsEmpty)
                    result.ArtiType = 2;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.ArtiBurning, artiTypeImg, 0.8).IsEmpty)
                    result.ArtiType = 1;
                else
                    result.ArtiType = 0; 
            }
                    

            Log.Verbose($"Arti: +{result.ArtiLevel}");
            Log.Verbose($"Arti: T{result.ArtiTier}");
            Log.Verbose($"Arti type: {(ArtifactType)(result.ArtiType ?? 0)}");
        }

        private void GetRingInfo(Bitmap img, HeroScanResult result)
        {
            var ringTierImg = img.CropImage(ImageRegions.RingTierArea);
            
            if (!EmguImageOperations.FindImageRegion(ImageResources.AccT3, ringTierImg, 0.8).IsEmpty)
                result.RingTier = 3;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.AccT2, ringTierImg, 0.8).IsEmpty)
                result.RingTier = 2;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.AccT1, ringTierImg, 0.8).IsEmpty)
                result.RingTier = 1;
            else
                result.RingTier = 0; // No arti

            if (result.RingTier > 0)
            {
                var ringImg = img.CropImage(TextRegions.RingLevelArea);
                for (int i = 0; i < 3; i++)
                {
                    var upgString = TesseractOperations.ReadText(ringImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                    if (int.TryParse(upgString, out var str))
                    {
                        result.RingLevel = str;
                        break;
                    }
                }

                var ringTypeImg = img.CropImage(ImageRegions.RingTypeArea);
                if (!EmguImageOperations.FindImageRegion(ImageResources.AccOrange, ringTypeImg, 0.8).IsEmpty)
                    result.RingType = 3;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.AccBlue, ringTypeImg, 0.8).IsEmpty)
                    result.RingType = 2;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.AccPurple, ringTypeImg, 0.8).IsEmpty)
                    result.RingType = 1;
                else
                    result.RingType = 0;
            }


            Log.Verbose($"Ring: +{result.RingLevel}");
            Log.Verbose($"Ring: T{result.RingTier}");
            Log.Verbose($"Ring type: {(AccessorySetEnum)(result.RingType ?? 0)}");
        }

        private void GetNecklaceInfo(Bitmap img, HeroScanResult result)
        {
            var accTierImg = img.CropImage(ImageRegions.NecklaceTierArea);
            
            if (!EmguImageOperations.FindImageRegion(ImageResources.AccT3, accTierImg, 0.8).IsEmpty)
                result.NecklaceTier = 3;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.AccT2, accTierImg, 0.8).IsEmpty)
                result.NecklaceTier = 2;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.AccT1, accTierImg, 0.8).IsEmpty)
                result.NecklaceTier = 1;
            else
                result.NecklaceTier = 0; // No arti

            if (result.NecklaceTier > 0)
            {
                var accImg = img.CropImage(TextRegions.NecklaceLevelArea);
                for (int i = 0; i < 3; i++)
                {
                    var upgString = TesseractOperations.ReadText(accImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                    if (int.TryParse(upgString, out var str))
                    {
                        result.NecklaceLevel = str;
                        break;
                    }
                }

                var accTypeImg = img.CropImage(ImageRegions.NecklaceTypeArea);
                if (!EmguImageOperations.FindImageRegion(ImageResources.AccOrange, accTypeImg, 0.8).IsEmpty)
                    result.NecklaceType = 3;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.AccBlue, accTypeImg, 0.8).IsEmpty)
                    result.NecklaceType = 2;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.AccPurple, accTypeImg, 0.8).IsEmpty)
                    result.NecklaceType = 1;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.AccPurple2, accTypeImg, 0.8).IsEmpty)
                    result.NecklaceType = 1;
                else
                    result.NecklaceType = 0;
            }


            Log.Verbose($"Necklace: +{result.NecklaceLevel}");
            Log.Verbose($"Necklace: T{result.NecklaceTier}");
            Log.Verbose($"Necklace type: {(AccessorySetEnum)(result.NecklaceType ?? 0)}");
        }

        private void GetEarringInfo(Bitmap img, HeroScanResult result)
        {
            var accTierImg = img.CropImage(ImageRegions.EarringTierArea);
            
            if (!EmguImageOperations.FindImageRegion(ImageResources.AccT3, accTierImg, 0.8).IsEmpty)
                result.EarringTier = 3;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.AccT2, accTierImg, 0.8).IsEmpty)
                result.EarringTier = 2;
            else if (!EmguImageOperations.FindImageRegion(ImageResources.AccT1, accTierImg, 0.8).IsEmpty)
                result.EarringTier = 1;
            else
                result.EarringTier = 0; // No arti

            if (result.EarringTier > 0)
            {
                var accImg = img.CropImage(TextRegions.EarringLevelArea);
                for (int i = 0; i < 3; i++)
                {
                    var upgString = TesseractOperations.ReadText(accImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                    if (int.TryParse(upgString, out var str))
                    {
                        result.EarringLevel = str;
                        break;
                    }
                }

                var accTypeImg = img.CropImage(ImageRegions.EarringTypeArea);
                if (!EmguImageOperations.FindImageRegion(ImageResources.AccOrange, accTypeImg, 0.8).IsEmpty)
                    result.EarringType = 3;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.AccBlue, accTypeImg, 0.8).IsEmpty)
                    result.EarringType = 2;
                else if (!EmguImageOperations.FindImageRegion(ImageResources.AccPurple, accTypeImg, 0.8).IsEmpty)
                    result.EarringType = 1;
                else
                    result.EarringType = 0;
            }


            Log.Verbose($"Earring: +{result.EarringLevel}");
            Log.Verbose($"Earring: T{result.EarringTier}");
            Log.Verbose($"Earring type: {(AccessorySetEnum)(result.EarringType ?? 0)}");
        }

        private HeroScanResult GetHeroProgress(Bitmap heroDetailsImg)
        {
            var result = new HeroScanResult() { IsChecked = true };

            var levelImg = heroDetailsImg.CropImage(TextRegions.LevelArea);
            var siImg = heroDetailsImg.CropImage(TextRegions.SIArea);
            var chaserImg = heroDetailsImg.CropImage(TextRegions.ChaserArea);
            var transImg = heroDetailsImg.CropImage(TextRegions.TranscendenceArea);

            int level;
            for (int i=0; i<3; i++)
            {
                var levelString = TesseractOperations.ReadText(levelImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                if (int.TryParse(levelString, out level) && level > 0 && level <= StaticValues.MaxLevel)
                {
                    result.Level = level;
                    break;
                }
            }
            Log.Verbose($"Level: {result.Level}");
            int si;
            for (int i = 0; i < 3; i++)
            {
                var siString = TesseractOperations.ReadText(siImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                if (int.TryParse(siString, out si) && si > 0 && si <= StaticValues.MaxSiLevel)
                {
                    result.Si = si;
                    break;
                }
            }
            Log.Verbose($"SI: {result.Si}");
            int chaser;
            for (int i = 0; i < 3; i++)
            {
                var chaserString = TesseractOperations.ReadText(chaserImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                if (int.TryParse(chaserString, out chaser) && chaser > 0 && chaser <= StaticValues.MaxClLevel)
                {
                    result.Chaser = chaser;
                    break;
                }
            }
            Log.Verbose($"CL: {result.Chaser}");
            int trans;
            for (int i = 0; i < 3; i++)
            {
                var transString = TesseractOperations.ReadText(transImg.ToByteArray(ImageFormat.Png))?.Trim()?.GetNumbers();
                if (int.TryParse(transString, out trans) && trans >= 0 && trans <= StaticValues.MaxTranscendenceLevel)
                {
                    result.Trans = trans;
                    break;
                }
            }
            Log.Verbose($"T: {result.Trans}");

            return result;
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
            OnPropertyChanged(propName);
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
