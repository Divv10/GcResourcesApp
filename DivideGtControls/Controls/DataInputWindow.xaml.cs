using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DivideGT
{
    /// <summary>
    /// Interaction logic for DataInputWindow.xaml
    /// </summary>
    public partial class DataInputWindow : Window, INotifyPropertyChanged
    {
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return null;
        }

        public List<int> HiddenList { get; set; }

        public List<StarsDropDown> StarsList { get; set; } 

        private ObservableCollection<TournamentRecord> _records;
        public ObservableCollection<TournamentRecord> Records
        {
            get => _records;
            set => SetProperty(ref _records, value);
        }

        private TournamentRecord _selectedRecord;
        public TournamentRecord SelectedRecord
        {
            get => _selectedRecord;
            set => SetProperty(ref _selectedRecord, value);
        }

        private GuildEnum _guild;
        public GuildEnum Guild
        {
            get => _guild;
            set => SetProperty(ref _guild, value);
        }

        private RoundEnum _round;
        public RoundEnum Round
        {
            get => _round;
            set => SetProperty(ref _round, value);
        }

        private int? _position;
        public int? Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private string _playerName;
        public string PlayerName
        {
            get => _playerName;
            set => SetProperty(ref _playerName, value);
        }

        private DateTime _finalsDay;
        public DateTime FinalsDate
        {
            get => _finalsDay;
            set => SetProperty(ref _finalsDay, value);
        }

        private HeroEnum _defense1;
        public HeroEnum Defense1
        {
            get => _defense1;
            set => SetProperty(ref _defense1, value);
        }

        private HeroEnum _defense2;
        public HeroEnum Defense2
        {
            get => _defense2;
            set => SetProperty(ref _defense2, value);
        }

        private HeroEnum _defense3;
        public HeroEnum Defense3
        {
            get => _defense3;
            set => SetProperty(ref _defense3, value);
        }

        private HeroEnum _defense4;
        public HeroEnum Defense4
        {
            get => _defense4;
            set => SetProperty(ref _defense4, value);
        }

        private HeroEnum _defense5;
        public HeroEnum Defense5
        {
            get => _defense5;
            set => SetProperty(ref _defense5, value);
        }

        private HeroEnum _defense6;
        public HeroEnum Defense6
        {
            get => _defense6;
            set => SetProperty(ref _defense6, value);
        }

        private HeroEnum _defense7;
        public HeroEnum Defense7
        {
            get => _defense7;
            set => SetProperty(ref _defense7, value);
        }

        private HeroEnum _defense8;
        public HeroEnum Defense8
        {
            get => _defense8;
            set => SetProperty(ref _defense8, value);
        }

        private int _hidden1;
        public int Hidden1
        {
            get => _hidden1;
            set => SetProperty(ref _hidden1, value);
        }

        private int _hidden2;
        public int Hidden2
        {
            get => _hidden2;
            set => SetProperty(ref _hidden2, value);
        }

        private int _bp;
        public int Bp
        {
            get => _bp;
            set => SetProperty(ref _bp, value);
        }

        private StarsDropDown _stars;
        public StarsDropDown Stars
        {
            get => _stars;
            set => SetProperty(ref _stars, value);
        }

        private HeroEnum _attack1;
        public HeroEnum Attack1
        {
            get => _attack1;
            set => SetProperty(ref _attack1, value);
        }

        private HeroEnum _attack2;
        public HeroEnum Attack2
        {
            get => _attack2;
            set => SetProperty(ref _attack2, value);
        }

        private HeroEnum _attack3;
        public HeroEnum Attack3
        {
            get => _attack3;
            set => SetProperty(ref _attack3, value);
        }

        private HeroEnum _attack4;
        public HeroEnum Attack4
        {
            get => _attack4;
            set => SetProperty(ref _attack4, value);
        }

        private HeroEnum _defenseAttack1;
        public HeroEnum DefenseAttack1
        {
            get => _defenseAttack1;
            set => SetProperty(ref _defenseAttack1, value);
        }

        private HeroEnum _defenseAttack2;
        public HeroEnum DefenseAttack2
        {
            get => _defenseAttack2;
            set => SetProperty(ref _defenseAttack2, value);
        }

        private HeroEnum _defenseAttack3;
        public HeroEnum DefenseAttack3
        {
            get => _defenseAttack3;
            set => SetProperty(ref _defenseAttack3, value);
        }

        private HeroEnum _defenseAttack4;
        public HeroEnum DefenseAttack4
        {
            get => _defenseAttack4;
            set => SetProperty(ref _defenseAttack4, value);
        }

        private HeroEnum _defenseAttack5;
        public HeroEnum DefenseAttack5
        {
            get => _defenseAttack5;
            set => SetProperty(ref _defenseAttack5, value);
        }

        private HeroEnum _defenseAttack6;
        public HeroEnum DefenseAttack6
        {
            get => _defenseAttack6;
            set => SetProperty(ref _defenseAttack6, value);
        }

        private HeroEnum _defenseAttack7;
        public HeroEnum DefenseAttack7
        {
            get => _defenseAttack7;
            set => SetProperty(ref _defenseAttack7, value);
        }

        private HeroEnum _defenseAttack8;
        public HeroEnum DefenseAttack8
        {
            get => _defenseAttack8;
            set => SetProperty(ref _defenseAttack8, value);
        }

        private int _hiddenAttack1;
        public int HiddenAttack1
        {
            get => _hiddenAttack1;
            set => SetProperty(ref _hiddenAttack1, value);
        }

        private int _hiddenAttack2;
        public int HiddenAttack2
        {
            get => _hiddenAttack2;
            set => SetProperty(ref _hiddenAttack2, value);
        }

        private StarsDropDown _attackStars;
        public StarsDropDown AttackStars
        {
            get => _attackStars;
            set => SetProperty(ref _attackStars, value);
        }

        private ICommand _addRecordCommand;
        public ICommand AddRecordCommand
        {
            get
            {
                if (_addRecordCommand == null)
                {
                    _addRecordCommand = new RelayCommand(AddRecord, () => !string.IsNullOrEmpty(PlayerName) && Position.HasValue && !Records.Any(x => x.Position == Position.Value));
                }
                return _addRecordCommand;
            }
        }

        private ICommand _modifySelectedRecordCommand;
        public ICommand ModifySelectedRecordCommand
        {
            get
            {
                if (_modifySelectedRecordCommand == null)
                {
                    _modifySelectedRecordCommand = new RelayCommand(ModifySelectedRecord, () => !string.IsNullOrEmpty(PlayerName) && Position.HasValue && SelectedRecord != null);
                }
                return _modifySelectedRecordCommand;
            }
        }

        private ICommand _removeSelectedRecordCommand;
        public ICommand RemoveSelectedRecordCommand
        {
            get
            {
                if (_removeSelectedRecordCommand == null)
                {
                    _removeSelectedRecordCommand = new RelayCommand(DeleteSelected, () => SelectedRecord != null);
                }
                return _removeSelectedRecordCommand;
            }
        }

        private ICommand _editSelectedRecordCommand;
        public ICommand EditSelectedRecordCommand
        {
            get
            {
                if (_editSelectedRecordCommand == null)
                {
                    _editSelectedRecordCommand = new RelayCommand(EditSelected, () => SelectedRecord != null);
                }
                return _editSelectedRecordCommand;
            }
        }

        private ICommand _saveRecordsCommand;
        public ICommand SaveRecordsCommand
        {
            get
            {
                if (_saveRecordsCommand == null)
                {
                    _saveRecordsCommand = new RelayCommand(SaveRecords, () => Records != null);
                }
                return _saveRecordsCommand;
            }
        }

        private ICommand _loadRecordsCommand;
        public ICommand LoadRecordsCommand
        {
            get
            {
                if (_loadRecordsCommand == null)
                {
                    _loadRecordsCommand = new RelayCommand(LoadRecordsFromFile, () => Records != null);
                }
                return _loadRecordsCommand;
            }
        }

        public DataInputWindow()
        {
            HiddenList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            StarsList = new List<StarsDropDown>()
            {
                new StarsDropDown() {Stars = StarsEnum.ZeroStar , Label = "0"},
                new StarsDropDown() {Stars = StarsEnum.OneStar , Label = "1"},
                new StarsDropDown() {Stars = StarsEnum.TwoStar , Label = "2"},
                new StarsDropDown() {Stars = StarsEnum.ThreeStar , Label = "3"},
                new StarsDropDown() {Stars = StarsEnum.NotAttacked , Label = "No attack"},
            };
            Records = new ObservableCollection<TournamentRecord>();
            FinalsDate = DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? DateTime.Today : DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);
            DataContext = this;
            ClearDataPartially();
            InitializeComponent();
        }

        private void AddRecord(object param)
        {
            if (!Position.HasValue || string.IsNullOrWhiteSpace(PlayerName))
            {
                MessageBox.Show("Position and / or Name cannot be empty");
                return;
            }
            if (Records.Any(x => x.Position == Position.Value))
            {
                MessageBox.Show($"Record with position {Position.Value} already addded");
                return;
            }

            var record = new TournamentRecord()
            {
                Position = Position.Value,
                PlayerName = PlayerName,
                TournamentDate = FinalsDate.Date,
                Guild = Guild,
                Round = Round,
                DefenseInfo = new Defense()
                {
                    Defense1 = Defense1,
                    Defense2 = Defense2,
                    Defense3 = Defense3,
                    Defense4 = Defense4,
                    Defense5 = Defense5,
                    Defense6 = Defense6,
                    Defense7 = Defense7,
                    Defense8 = Defense8,
                    Hidden1 = Hidden1,
                    Hidden2 = Hidden2,
                    Bp = Bp,
                    Stars = Stars.Stars,
                },
                AttackInfo = new Attack()
                {
                    Attack1 = Attack1,
                    Attack2 = Attack2,
                    Attack3 = Attack3,
                    Attack4 = Attack4,
                },
                AttackDefenseInfo = new Defense()
                {
                    Defense1 = DefenseAttack1,
                    Defense2 = DefenseAttack2,
                    Defense3 = DefenseAttack3,
                    Defense4 = DefenseAttack4,
                    Defense5 = DefenseAttack5,
                    Defense6 = DefenseAttack6,
                    Defense7 = DefenseAttack7,
                    Defense8 = DefenseAttack8,
                    Hidden1 = HiddenAttack1,
                    Hidden2 = HiddenAttack2,
                    Stars = AttackStars.Stars,
                },
            };
            Records.Add(record);
            ClearDataPartially();
            SelectedRecord = null;
            Records = new ObservableCollection<TournamentRecord>(Records.OrderBy(x => x.Position).ToList());
        }

        private void ModifySelectedRecord(object param)
        {
            if (!Position.HasValue || string.IsNullOrWhiteSpace(PlayerName))
            {
                MessageBox.Show("Position and / or Name cannot be empty");
                return;
            }
            if (Records.Where(x => x != SelectedRecord).Any(x => x.Position == Position.Value))
            {
                MessageBox.Show($"Record with position {Position.Value} already addded");
                return;
            }
            if (SelectedRecord == null)
            {
                MessageBox.Show($"Select record to modify");
                return;
            }

            var record = SelectedRecord;

            record.Position = Position.Value;
            record.PlayerName = PlayerName;
            record.TournamentDate = FinalsDate.Date;
            record.Guild = Guild;
            record.Round = Round;
            record.DefenseInfo = new Defense()
            {
                Defense1 = Defense1,
                Defense2 = Defense2,
                Defense3 = Defense3,
                Defense4 = Defense4,
                Defense5 = Defense5,
                Defense6 = Defense6,
                Defense7 = Defense7,
                Defense8 = Defense8,
                Hidden1 = Hidden1,
                Hidden2 = Hidden2,
                Bp = Bp,
                Stars = Stars.Stars,
            };
            record.AttackInfo = new Attack()
            {
                Attack1 = Attack1,
                Attack2 = Attack2,
                Attack3 = Attack3,
                Attack4 = Attack4,
            };
            record.AttackDefenseInfo = new Defense()
            {
                Defense1 = DefenseAttack1,
                Defense2 = DefenseAttack2,
                Defense3 = DefenseAttack3,
                Defense4 = DefenseAttack4,
                Defense5 = DefenseAttack5,
                Defense6 = DefenseAttack6,
                Defense7 = DefenseAttack7,
                Defense8 = DefenseAttack8,
                Hidden1 = HiddenAttack1,
                Hidden2 = HiddenAttack2,
                Stars = AttackStars.Stars,
            };

            ClearDataPartially();
            SelectedRecord = null;
        }

        private void ClearDataPartially()
        {
            Position = null;
            PlayerName = null;

            Defense1 = HeroEnum.Unknown;
            Defense2 = HeroEnum.Unknown;
            Defense3 = HeroEnum.Unknown;
            Defense4 = HeroEnum.Unknown;
            Defense5 = HeroEnum.Unknown;
            Defense6 = HeroEnum.Unknown;
            Defense7 = HeroEnum.Unknown;
            Defense8 = HeroEnum.Unknown;
            Hidden1 = 1;
            Hidden2 = 2;
            Bp = 0;
            Stars = StarsList.FirstOrDefault(x => x.Stars == StarsEnum.NotAttacked);

            Attack1 = HeroEnum.Unknown;
            Attack2 = HeroEnum.Unknown;
            Attack3 = HeroEnum.Unknown;
            Attack4 = HeroEnum.Unknown;

            DefenseAttack1 = HeroEnum.Unknown;
            DefenseAttack2 = HeroEnum.Unknown;
            DefenseAttack3 = HeroEnum.Unknown;
            DefenseAttack4 = HeroEnum.Unknown;
            DefenseAttack5 = HeroEnum.Unknown;
            DefenseAttack6 = HeroEnum.Unknown;
            DefenseAttack7 = HeroEnum.Unknown;
            DefenseAttack8 = HeroEnum.Unknown;
            HiddenAttack1 = 1;
            HiddenAttack2 = 2;
            AttackStars = StarsList.FirstOrDefault(x => x.Stars == StarsEnum.NotAttacked);

        }

        private void EditSelected(object param)
        {
            if (SelectedRecord == null)
                return;

            Position = SelectedRecord.Position;
            PlayerName = SelectedRecord.PlayerName;
            Guild = SelectedRecord.Guild;
            Round = SelectedRecord.Round;
            FinalsDate = SelectedRecord.TournamentDate;

            Defense1 = SelectedRecord.DefenseInfo.Defense1;
            Defense2 = SelectedRecord.DefenseInfo.Defense2;
            Defense3 = SelectedRecord.DefenseInfo.Defense3;
            Defense4 = SelectedRecord.DefenseInfo.Defense4;
            Defense5 = SelectedRecord.DefenseInfo.Defense5;
            Defense6 = SelectedRecord.DefenseInfo.Defense6;
            Defense7 = SelectedRecord.DefenseInfo.Defense7;
            Defense8 = SelectedRecord.DefenseInfo.Defense8;
            Hidden1 = SelectedRecord.DefenseInfo.Hidden1;
            Hidden2 = SelectedRecord.DefenseInfo.Hidden2;
            Stars = StarsList.FirstOrDefault(x => x.Stars == SelectedRecord.DefenseInfo.Stars);
            Bp = SelectedRecord.DefenseInfo.Bp;

            Attack1 = SelectedRecord.AttackInfo.Attack1;
            Attack2 = SelectedRecord.AttackInfo.Attack2;
            Attack3 = SelectedRecord.AttackInfo.Attack3;
            Attack4 = SelectedRecord.AttackInfo.Attack4;

            DefenseAttack1 = SelectedRecord.AttackDefenseInfo.Defense1;
            DefenseAttack2 = SelectedRecord.AttackDefenseInfo.Defense2;
            DefenseAttack3 = SelectedRecord.AttackDefenseInfo.Defense3;
            DefenseAttack4 = SelectedRecord.AttackDefenseInfo.Defense4;
            DefenseAttack5 = SelectedRecord.AttackDefenseInfo.Defense5;
            DefenseAttack6 = SelectedRecord.AttackDefenseInfo.Defense6;
            DefenseAttack7 = SelectedRecord.AttackDefenseInfo.Defense7;
            DefenseAttack8 = SelectedRecord.AttackDefenseInfo.Defense8;
            HiddenAttack1 = SelectedRecord.AttackDefenseInfo.Hidden1;
            HiddenAttack2 = SelectedRecord.AttackDefenseInfo.Hidden2;
            AttackStars = StarsList.FirstOrDefault(x => x.Stars == SelectedRecord.AttackDefenseInfo.Stars);

        }

        private void DeleteSelected(object param)
        {
            if (SelectedRecord != null)
                Records.Remove(SelectedRecord);
        }

        private void SaveRecords(object param)
        {
            if (Records == null)
                return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save records";
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog1.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog1.FileName, $"; {Guild.ToString()} {Round.ToString()} {FinalsDate.ToString("d")} {Environment.NewLine}");
                File.AppendAllLines(saveFileDialog1.FileName, Records.Select(x => x.ToString()));
            };
        }

        private void LoadRecordsFromFile(object param)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.DefaultExt = "txt";
            ofd.Title = "Load records";
            if (ofd.ShowDialog() == true)
            {
                Records = new ObservableCollection<TournamentRecord>();
                var lines = File.ReadAllLines(ofd.FileName); 
                foreach (var line in lines)
                {
                    if (line.StartsWith(";") || string.IsNullOrWhiteSpace(line))
                        continue;
                    Records.Add(line.GetTournamentRecord());
                }
            }

            Records = new ObservableCollection<TournamentRecord>(Records.OrderBy(x => x.Position).ToList());
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
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
