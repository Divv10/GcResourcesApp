using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using GCManagementApp.Static;

namespace GCManagementApp.Windows {
	/// <summary>
	/// Interaction logic for MessageDialog.xaml
	/// </summary>
	public partial class DataSyncWindow : Window, INotifyPropertyChanged {
		public ICommand OpenHelpCommand { get; set; }

		private Window _mainWindow;
		public Window MainWindow {
			get => _mainWindow;
			set => SetProperty(ref _mainWindow, value);
			}

		private Action _closeAction;
		public Action CloseAction {
			get => _closeAction;
			set => SetProperty(ref _closeAction, value);
			}

		public DataSyncWindow( Window mainWindow ) {
			InitializeComponent();
			DataContext = this;

			OpenHelpCommand = new RelayCommand(OpenHelp);

			Closing += OnWindowClosing;
			Loaded += OnWindowLoaded;
			MainWindow = mainWindow;
			this.Topmost = true;
			}

		private void OnWindowLoaded( object sender, EventArgs e ) {
			Loaded -= OnWindowLoaded;
			if ( MainWindow != null ) {
				MainWindow.WindowState = WindowState.Minimized;
				}
			}

		private void OnWindowClosing( object sender, CancelEventArgs e ) {
			Closing -= OnWindowClosing;
			if ( CloseAction != null ) {
				CloseAction();
				}
			if ( MainWindow != null ) {
				MainWindow.WindowState = WindowState.Maximized;
				}

			var wndsCount = EmulatorConnectionInfo.RegionWindowList.Count;
			for ( int w = wndsCount - 1; w >= 0; w-- ) {
				try {
					EmulatorConnectionInfo.RegionWindowList[w].Close();
					}
				catch { }
				}
			}

		private void OpenHelp( object param ) {
			Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/1RAKXqxO965z5ZP39vVxMPz7ytqPJHVWcp6CTIXjCt1c", UseShellExecute = true });
			}

		#region PC

		public event PropertyChangedEventHandler PropertyChanged = null!;

		protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null! ) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

		protected virtual void OnPropertyChanged<T>( Expression<Func<T>> raiser ) {
			var propName = ((MemberExpression) raiser?.Body!)?.Member.Name;
			OnPropertyChanged(propName!);
			}

		protected bool SetProperty<T>( ref T field, T value, [CallerMemberName] string name = null! ) {
			if ( EqualityComparer<T>.Default.Equals(field, value) )
				return false;
			field = value;
			OnPropertyChanged(name);
			return true;
			}

		#endregion
		}
	}
