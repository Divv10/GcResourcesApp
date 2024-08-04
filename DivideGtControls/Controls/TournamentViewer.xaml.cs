using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace DivideGT
{
    /// <summary>
    /// Interaction logic for TournamentViewer.xaml
    /// </summary>
    public partial class TournamentViewer : UserControl, INotifyPropertyChanged
    {
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return null;
        }

        private List<TournamentRecord> _tournamentRecords;
        public List<TournamentRecord> TournamentRecords
        {
            get => _tournamentRecords;
            set
            {
                SetProperty(ref _tournamentRecords, value);
                OnPropertyChanged("TournamentRecordsByRound");
            }
        }

        public Visibility IsRound64 => currentRound == RoundEnum.RoundOf64 ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsRound32 => currentRound == RoundEnum.RoundOf32 ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsRound16 => currentRound == RoundEnum.RoundOf16 ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsRound8 => currentRound == RoundEnum.RoundOf8 ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsRound4 => currentRound == RoundEnum.RoundOf4 ? Visibility.Visible : Visibility.Hidden;
        public Visibility IsRoundFinal => currentRound == RoundEnum.Final ? Visibility.Visible : Visibility.Hidden;

        private ICommand _switchRoundCommand;
        public ICommand SwitchRoundCommand
        {
            get
            {
                if (_switchRoundCommand == null)
                {
                    _switchRoundCommand = new RelayCommand(switchRound, () => true);
                }
                return _switchRoundCommand;
            }
        }

        private ICommand _switchNextRoundCommand;
        public ICommand SwitchNextRoundCommand
        {
            get
            {
                if (_switchNextRoundCommand == null)
                {
                    _switchNextRoundCommand = new RelayCommand(switchNextRound, () => true);
                }
                return _switchNextRoundCommand;
            }
        }

        private ICommand _switchPrevRoundCommand;
        public ICommand SwitchPrevRoundCommand
        {
            get
            {
                if (_switchPrevRoundCommand == null)
                {
                    _switchPrevRoundCommand = new RelayCommand(switchPrevRound, () => true);
                }
                return _switchPrevRoundCommand;
            }
        }

        private RoundEnum currentRound { get; set; }

        public List<TournamentRecord> TournamentRecordsByRound => TournamentRecords.Where(x => x.Round == currentRound).ToList();

        public TournamentViewer(List<TournamentRecord> records)
        {
            TournamentRecords = records.OrderBy(x => x.Position).ToList();
            DataContext = this;
            InitializeComponent();
        }

        private void switchRound(object param)
        {
            currentRound = (RoundEnum)(int.Parse(param.ToString()));
            updateRounds();
        }

        private void switchPrevRound(object param)
        {
            switch (currentRound)
            {
                case RoundEnum.Final:
                    currentRound = RoundEnum.RoundOf4;
                    break;
                case RoundEnum.RoundOf4:
                    currentRound = RoundEnum.RoundOf8;
                    break;
                case RoundEnum.RoundOf8:
                    currentRound = RoundEnum.RoundOf16;
                    break;
                case RoundEnum.RoundOf16:
                    currentRound = RoundEnum.RoundOf32;
                    break;
                case RoundEnum.RoundOf32:
                    currentRound = RoundEnum.RoundOf64;
                    break;
                case RoundEnum.RoundOf64:
                    currentRound = RoundEnum.Final;
                    break;
            }
            updateRounds();
        }

        private void switchNextRound(object param)
        {
            switch (currentRound)
            {
                case RoundEnum.Final:
                    currentRound = RoundEnum.RoundOf64;
                    break;
                case RoundEnum.RoundOf4:
                    currentRound = RoundEnum.Final;
                    break;
                case RoundEnum.RoundOf8:
                    currentRound = RoundEnum.RoundOf4;
                    break;
                case RoundEnum.RoundOf16:
                    currentRound = RoundEnum.RoundOf8;
                    break;
                case RoundEnum.RoundOf32:
                    currentRound = RoundEnum.RoundOf16;
                    break;
                case RoundEnum.RoundOf64:
                    currentRound = RoundEnum.RoundOf32;
                    break;
            }
            updateRounds();
        }

        private void updateRounds()
        {
            OnPropertyChanged("IsRound64");
            OnPropertyChanged("IsRound32");
            OnPropertyChanged("IsRound16");
            OnPropertyChanged("IsRound8");
            OnPropertyChanged("IsRound4");
            OnPropertyChanged("IsRoundFinal");
            OnPropertyChanged("TournamentRecordsByRound");
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
