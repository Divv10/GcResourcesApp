using GCManagementApp.Models;
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

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for ProcessAttachUserControl.xaml
    /// </summary>
    public partial class ProcessSelectUserControl : UserControl, INotifyPropertyChanged
    {
        public ICommand ConnectCommand { get; }

        private string _emulatorIp;
        public string EmulatorIp
        {
            get => _emulatorIp;
            set => SetProperty(ref _emulatorIp, value);
        }

        private int _emulatorPort;
        public int EmulatorPort
        {
            get => _emulatorPort;
            set => SetProperty(ref _emulatorPort, value);
        }

        public bool IsConnected => EmulatorConnectionInfo.IsConnected;

        public string ConnectText => IsConnected ? "Disconnect" : "Connect";

        public ProcessSelectUserControl()
        {
            InitializeComponent();
            DataContext = this;
            EmulatorIp = Properties.Settings.Default.EmulatorIp;
            EmulatorPort = Properties.Settings.Default.EmulatorPort;

            ConnectCommand = new RelayCommand(Connect);
        }

        private async void Connect(object param)
        {
            if (!IsConnected)
            {
                try
                {
                    var instanceIpAddress = new System.Net.DnsEndPoint(EmulatorIp, EmulatorPort);

                    var bsProcess = new BlueStackProcess()
                    {
                        Host = EmulatorIp,
                        Port = EmulatorPort,
                        InstanceIpAddress = instanceIpAddress,
                    };

                    await AdbOperations.Connect(bsProcess);
                    Properties.Settings.Default.EmulatorIp = EmulatorIp;
                    Properties.Settings.Default.EmulatorPort = EmulatorPort;
                    Properties.Settings.Default.Save();
                    EmulatorConnectionInfo.IsConnected = true;
                    OnPropertyChanged(nameof(IsConnected));
                    OnPropertyChanged(nameof(ConnectText));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                await AdbOperations.Disconnect();
                EmulatorConnectionInfo.IsConnected = false;
                OnPropertyChanged(nameof(IsConnected));
                OnPropertyChanged(nameof(ConnectText));
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
