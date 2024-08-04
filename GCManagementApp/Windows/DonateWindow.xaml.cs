using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System;
using System.Windows.Input;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Threading.Tasks;
using System.Threading;
using GCManagementApp.Static;
using System.Linq;
using System.IO;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using GCManagementApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace GCManagementApp.Windows
{
    /// <summary>
    /// Interaction logic for DonateWindow.xaml
    /// </summary>
    public partial class DonateWindow : Window, INotifyPropertyChanged
    {
        public ICommand OpenKofiCommand { get; }
       
        public DonateWindow()
        {
            InitializeComponent();
            DataContext = this;

            OpenKofiCommand = new RelayCommand(o => OpenKofi());         
        }

        private void OpenKofi()
        {
            Process.Start(new ProcessStartInfo { FileName = "https://ko-fi.com/divgc", UseShellExecute = true });
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
