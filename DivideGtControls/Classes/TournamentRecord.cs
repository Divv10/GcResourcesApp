using DivideGtCommons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DivideGT
{
    public class TournamentRecord : NotifyPropertyChanged
    {
        private DateTime _tournamentDate;
        public DateTime TournamentDate
        {
            get => _tournamentDate;
            set => SetProperty(ref _tournamentDate, value);
        }

        private RoundEnum _round;
        public RoundEnum Round
        {
            get => _round;
            set => SetProperty(ref _round, value);
        }

        private GuildEnum _guild;
        public GuildEnum Guild
        {
            get => _guild;
            set => SetProperty(ref _guild, value);
        }

        private string _playerName;
        public string PlayerName
        {
            get => _playerName;
            set => SetProperty(ref _playerName, value);
        }

        private int _position;
        public int Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private Defense _defenseInfo;
        public Defense DefenseInfo
        {
            get => _defenseInfo;
            set => SetProperty(ref _defenseInfo, value);
        }

        private Defense _attackDefenseInfo;
        public Defense AttackDefenseInfo
        {
            get => _attackDefenseInfo;
            set => SetProperty(ref _attackDefenseInfo, value);
        }

        private Attack _attackInfo;
        public Attack AttackInfo
        {
            get => _attackInfo;
            set => SetProperty(ref _attackInfo, value);
        }

        private string _attackEnemyName;
        public string AttackEnemyName
        {
            get => _attackEnemyName;
            set => SetProperty(ref _attackEnemyName, value);
        }

        private bool _isError;
        public bool IsError
        {
            get => _isError;
            set => SetProperty(ref _isError, value);
        }

        private string _errorMsg;
        public string ErrorMsg
        {
            get => _errorMsg;
            set => SetProperty(ref _errorMsg, value);
        }

        private string _fullString;
        public string FullString
        {
            get => _fullString;
            set => SetProperty(ref _fullString, value);
        }

        private ICommand _showFullInfo;
        public ICommand ShowFullInfo
        {
            get
            {
                if (_showFullInfo == null)
                {
                    _showFullInfo = new RelayCommand(
                        (p) =>
                        {
                            FullInfoView fiv = new FullInfoView(Position, PlayerName, Guild, AppStatic.TournamentDate, AppStatic.TournamentProvider?.GetCollection()?.GetFullDetails(Guild, PlayerName, AppStatic.TournamentDate));
                            fiv.Show();
                        }, () => true);
                }
                return _showFullInfo;
            }
        }

        public TournamentRecordEnum RecordType { get; set; }

        public override string ToString()
        {            
            string[] record = new string[]
            {
                TournamentDate.ToString("yyyy-MM-dd"),
                ((int)Round).ToString(),
                ((int)Guild).ToString(),
                PlayerName?.Trim(),
                Position.ToString(),

                (DefenseInfo?.Defense1 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Defense2 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Defense3 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Defense4 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Defense5 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Defense6 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Defense7 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Defense8 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Hidden1 ?? 1).ToString(),
                (DefenseInfo?.Hidden2 ?? 2).ToString(),
                ((int)(DefenseInfo?.Stars ?? 0)).ToString(),
                (DefenseInfo?.Bp ?? 0).ToString(),

                (AttackInfo?.Attack1 ?? HeroEnum.Unknown).ToString(),
                (AttackInfo?.Attack2 ?? HeroEnum.Unknown).ToString(),
                (AttackInfo?.Attack3 ?? HeroEnum.Unknown).ToString(),
                (AttackInfo?.Attack4 ?? HeroEnum.Unknown).ToString(),
                (AttackInfo?.Bp ?? 0).ToString(),

                (AttackDefenseInfo?.Defense1 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Defense2 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Defense3 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Defense4 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Defense5 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Defense6 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Defense7 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Defense8 ?? HeroEnum.Unknown).ToString(),
                (AttackDefenseInfo?.Hidden1 ?? 1).ToString(),
                (AttackDefenseInfo?.Hidden2 ?? 2).ToString(),
                ((int)(AttackDefenseInfo?.Stars ?? 0)).ToString(),
                (AttackDefenseInfo?.Bp ?? 0).ToString(),

                " ", //Attack enemy name

                (DefenseInfo?.Pet1 ?? HeroEnum.Unknown).ToString(),
                (DefenseInfo?.Pet2 ?? HeroEnum.Unknown).ToString(),
                (AttackInfo?.Pet ?? HeroEnum.Unknown).ToString(),

            };

            return string.Join(",", record);
        }
    }
}
