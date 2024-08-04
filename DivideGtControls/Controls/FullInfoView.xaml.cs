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
using System.Windows.Shapes;

namespace DivideGT
{
    /// <summary>
    /// Interaction logic for FullInfoView.xaml
    /// </summary>
    public partial class FullInfoView : Window, INotifyPropertyChanged
    {
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return null;
        }

        private int _position;
        public int Position
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

        private string _guild;
        public string Guild
        {
            get => _guild;
            set => SetProperty(ref _guild, value);
        }

        private string _tournament;
        public string Tournament
        {
            get => _tournament;
            set => SetProperty(ref _tournament, value);
        }

        private List<TournamentRecord> _records;
        public List<TournamentRecord> Records
        {
            get => _records;
            set => SetProperty(ref _records, value);
        }

        public FullInfoView(int position, string playerName, GuildEnum guild, TournamentDate tournamentDate, List<TournamentRecord> records)
        {
            Position = position;
            PlayerName = playerName;
            Guild = guild.ToString();
            Tournament = tournamentDate.DisplayName;
            Records = records.OrderBy(x => (int)x.Round).ToList();
            DataContext = this;
            InitializeComponent();
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
