using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DivideGT;
using DivideGtCommons.Enums;
using LiveCharts;
using LiveCharts.Wpf;

namespace DivideGtCommons.Controls
{
    /// <summary>
    /// Interaction logic for AttackSummary.xaml
    /// </summary>
    public partial class AttackSummary : UserControl
    {
        public static Brush[] colors = new[]
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEC007")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#607D8A")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2195F2")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F34336"))
            };

        public PieChart Pie1 { get { return pie1; } }
        public PieChart Pie2 { get { return pie2; } }

        public SeriesCollection MostUsedSeries { get; set; }
        public SeriesCollection FailedSeries { get; set; }
        public List<Comp> MostUsedComps { get; set; }
        public List<Comp> FailedComps { get; set; }
        public List<PlayersFails> FailedAttacks { get; set; }
        public string GuildName { get; set; }
        public int TotalAttacks { get; set; }
        public int Attacks3Stars { get; set; }
        public int Attacks2Stars { get; set; }
        public int Attacks1Stars { get; set; }
        public int Attacks0Stars { get; set; }

        public AttackSummary(List<TournamentRecord> records)
        {
            InitializeComponent();
            DataContext = this;

            if (records == null || records.Count == 0)
                return;

            GuildName = records.FirstOrDefault().Guild.ToString();
            var attacksRecords = records.Where(r => (r.RecordType == TournamentRecordEnum.Attack || r.RecordType == TournamentRecordEnum.Both) && r.AttackDefenseInfo?.Stars != StarsEnum.NotAttacked);
            TotalAttacks = attacksRecords.Count();
            Attacks3Stars = attacksRecords.Where(r => r.AttackDefenseInfo.Stars == StarsEnum.ThreeStar).Count();
            Attacks2Stars = attacksRecords.Where(r => r.AttackDefenseInfo.Stars == StarsEnum.TwoStar).Count();
            Attacks1Stars = attacksRecords.Where(r => r.AttackDefenseInfo.Stars == StarsEnum.OneStar).Count();
            Attacks0Stars = attacksRecords.Where(r => r.AttackDefenseInfo.Stars == StarsEnum.ZeroStar).Count();

            var failedAttacks = attacksRecords.Where(r => r.AttackDefenseInfo.Stars != StarsEnum.ThreeStar);            
               
            var compsDict = new Dictionary<string, int>();
            var failedCompsDict = new Dictionary<string, int>();
            var playerFails = new Dictionary<string, int>();
            foreach (var a in attacksRecords)
            {
                string[] heroes = new[] { a.AttackInfo.Attack1.ToString(), a.AttackInfo.Attack2.ToString(), a.AttackInfo.Attack3.ToString(), a.AttackInfo.Attack4.ToString() };
                string comp = string.Join(", ", heroes.OrderBy(x => x));                
                
                if (compsDict.ContainsKey(comp))
                {
                    compsDict[comp]++;
                }
                else
                {
                    compsDict.Add(comp, 1);
                }
               
                if (a.AttackDefenseInfo.Stars != StarsEnum.ThreeStar)
                {
                    if (playerFails.ContainsKey(a.PlayerName))
                    {
                        playerFails[a.PlayerName]++;
                    }
                    else
                    {
                        playerFails.Add(a.PlayerName, 1);
                    }

                    if (failedCompsDict.ContainsKey(comp))
                    {
                        failedCompsDict[comp]++;
                    }
                    else
                    {
                        failedCompsDict.Add(comp, 1);
                    }
                }
            }

            FailedAttacks = playerFails.Select(x => new PlayersFails() { Name = x.Key, Count = x.Value }).OrderByDescending(x => x.Count).ToList();

            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P0})", chartPoint.Y, chartPoint.Participation);

            var topThreeComps = compsDict.OrderByDescending(x => x.Value).Take(3);
            var i = 0;
            MostUsedComps = new List<Comp>();
            MostUsedSeries = new SeriesCollection();

            foreach (var c in topThreeComps)
            {
                var compArr = c.Key.Split(',');

                MostUsedComps.Add(new Comp() { Color = colors[i], IsAttackVisible = Visibility.Visible, IsOtherVisible = Visibility.Hidden, AttackComp = new Attack() 
                {
                    Attack1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[0].Trim(), true), 
                    Attack2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[1].Trim(), true),
                    Attack3 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[2].Trim(), true), 
                    Attack4 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[3].Trim(), true) 
                }});

                MostUsedSeries.Add(new PieSeries { Values = new ChartValues<int> { c.Value }, DataLabels = true, LabelPoint = labelPoint, Fill = colors[i] });
                i++;
            }
            if (topThreeComps.Sum(x => x.Value) < TotalAttacks)
            {
                MostUsedComps.Add(new Comp() { Color = colors[3], IsAttackVisible = Visibility.Hidden, IsOtherVisible = Visibility.Visible, AttackComp = null });
                MostUsedSeries.Add(new PieSeries { Values = new ChartValues<int> { TotalAttacks - topThreeComps.Sum(x => x.Value) }, DataLabels = true, LabelPoint = labelPoint, Fill = colors[i] });
            }

            var topThreeFailedComps = failedCompsDict.OrderByDescending(x => x.Value).Take(3);
            i = 0;
            FailedComps = new List<Comp>();
            FailedSeries = new SeriesCollection();

            foreach (var c in topThreeFailedComps)
            {
                var compArr = c.Key.Split(',');

                FailedComps.Add(new Comp()
                {
                    Color = colors[i],
                    IsAttackVisible = Visibility.Visible,
                    IsOtherVisible = Visibility.Hidden,
                    AttackComp = new Attack()
                    {
                        Attack1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[0].Trim(), true),
                        Attack2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[1].Trim(), true),
                        Attack3 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[2].Trim(), true),
                        Attack4 = (HeroEnum)Enum.Parse(typeof(HeroEnum), compArr[3].Trim(), true)
                    }
                });

                FailedSeries.Add(new PieSeries { Values = new ChartValues<int> { c.Value }, DataLabels = true, LabelPoint = labelPoint, Fill = colors[i] });
                i++;
            }
            if (topThreeFailedComps.Sum(x => x.Value) < (TotalAttacks - Attacks3Stars))
            {
                FailedComps.Add(new Comp() { Color = colors[3], IsAttackVisible = Visibility.Hidden, IsOtherVisible = Visibility.Visible, AttackComp = null });
                FailedSeries.Add(new PieSeries { Values = new ChartValues<int> { TotalAttacks - topThreeComps.Sum(x => x.Value) }, DataLabels = true, LabelPoint = labelPoint, Fill = colors[i] });
            }

            //MostUsedSeries = new SeriesCollection
            //{
            //    new PieSeries { Values = new ChartValues<double> { 2 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[0]},
            //    new PieSeries { Values = new ChartValues<double> { 4 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[1]},
            //    new PieSeries { Values = new ChartValues<double> { 1 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[2]},
            //    new PieSeries { Values = new ChartValues<double> { 3 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[3]}
            //};
            //FailedSeries = new SeriesCollection
            //{
            //    new PieSeries { Values = new ChartValues<double> { 2 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[0]},
            //    new PieSeries { Values = new ChartValues<double> { 4 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[1]},
            //    new PieSeries { Values = new ChartValues<double> { 1 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[2]},
            //    new PieSeries { Values = new ChartValues<double> { 3 }, DataLabels = true,LabelPoint = labelPoint, Fill=colors[3]}
            //};

            //MostUsedComps = new List<Comp>()
            //{
            //    new Comp() {Color = colors[0], IsAttackVisible = Visibility.Visible, IsOtherVisible = Visibility.Hidden, AttackComp = new Attack() {Attack1 = HeroEnum.Amy, Attack2 = HeroEnum.Arme, Attack3 = HeroEnum.Edel, Attack4 = HeroEnum.Elesis } },
            //    new Comp() {Color = colors[1], IsAttackVisible = Visibility.Visible, IsOtherVisible = Visibility.Hidden, AttackComp = new Attack() {Attack1 = HeroEnum.Amy, Attack2 = HeroEnum.Arme, Attack3 = HeroEnum.Edel, Attack4 = HeroEnum.Elesis } },
            //    new Comp() {Color = colors[2], IsAttackVisible = Visibility.Visible, IsOtherVisible = Visibility.Hidden, AttackComp = new Attack() {Attack1 = HeroEnum.Amy, Attack2 = HeroEnum.Arme, Attack3 = HeroEnum.Edel, Attack4 = HeroEnum.Elesis } },
            //    new Comp() {Color = colors[3], IsAttackVisible = Visibility.Hidden, IsOtherVisible = Visibility.Visible, AttackComp = null },
            //};

            //FailedComps = new List<Comp>()
            //{
            //    new Comp() {Color = colors[0], IsAttackVisible = Visibility.Visible, IsOtherVisible = Visibility.Hidden, AttackComp = new Attack() {Attack1 = HeroEnum.Amy, Attack2 = HeroEnum.Arme, Attack3 = HeroEnum.Edel, Attack4 = HeroEnum.Elesis } },
            //    new Comp() {Color = colors[1], IsAttackVisible = Visibility.Visible, IsOtherVisible = Visibility.Hidden, AttackComp = new Attack() {Attack1 = HeroEnum.Amy, Attack2 = HeroEnum.Arme, Attack3 = HeroEnum.Edel, Attack4 = HeroEnum.Elesis } },
            //    new Comp() {Color = colors[2], IsAttackVisible = Visibility.Visible, IsOtherVisible = Visibility.Hidden, AttackComp = new Attack() {Attack1 = HeroEnum.Amy, Attack2 = HeroEnum.Arme, Attack3 = HeroEnum.Edel, Attack4 = HeroEnum.Elesis } },
            //    new Comp() {Color = colors[3], IsAttackVisible = Visibility.Hidden, IsOtherVisible = Visibility.Visible, AttackComp = null },
            //};

            pie1.DisableAnimations = true;
            pie2.DisableAnimations = true;
            pie1.Series = MostUsedSeries;
            pie2.Series = FailedSeries;
        }        
    }



    public class Comp
    {
        public Brush Color { get; set; }
        public Attack AttackComp { get; set; }
        public Visibility IsAttackVisible { get; set; }
        public Visibility IsOtherVisible { get; set; }
    }

    public class PlayersFails
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }
}
