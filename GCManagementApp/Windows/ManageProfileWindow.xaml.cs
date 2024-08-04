using GCManagementApp.Helpers;
using GCManagementApp.Models;
using Microsoft.Win32;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GCManagementApp.Windows
{
    /// <summary>
    /// Interaction logic for ManageProfileWindow.xaml
    /// </summary>
    public partial class ManageProfileWindow : Window, INotifyPropertyChanged
    {
        public ICommand RenameCurrentProfileCommand { get; set; }
        public ICommand AcceptRenameCurrentProfileCommand { get; set; }
        public ICommand DiscardRenameCurrentProfileCommand { get; set; }
        public ICommand AddNewProfileCommand { get; set; }
        public ICommand DeleteSelectedProfileCommand { get; set; }
        public ICommand SetDefaultProfileCommand { get; set; }
        public ICommand RenameSelectedProfileCommand { get; set; }
        public ICommand AcceptRenameSelectedProfileCommand { get; set; }
        public ICommand DiscardRenameSelectedProfileCommand { get; set; }
        public ICommand ImportProfileCommand { get; set; }
        public ICommand SwitchProfileCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ICommand ExportProfileCommand { get; set; }

        private string _currentProfileRenameText;
        public string CurrentProfileRenameText
        {
            get => _currentProfileRenameText;
            set => SetProperty(ref _currentProfileRenameText, value);
        }

        private bool _isCurrentProfileRenaming;
        public bool IsCurrentProfileRenaming
        {
            get => _isCurrentProfileRenaming;
            set => SetProperty(ref _isCurrentProfileRenaming, value);
        }

        private ObservableCollection<ProfileFile> _profilesList;
        public ObservableCollection<ProfileFile> ProfilesList
        {
            get => _profilesList;
            set => SetProperty(ref _profilesList, value);
        }

        private ProfileFile _selectedProfile;
        public ProfileFile SelectedProfile
        {
            get => _selectedProfile;
            set => SetProperty(ref _selectedProfile, value);
        }

        private ProfileFile _currentProfile;
        public ProfileFile CurrentProfile
        {
            get => _currentProfile;
            set => SetProperty(ref _currentProfile, value);
        }

        private string _selectedProfileRenameText;
        public string SelectedProfileRenameText
        {
            get => _selectedProfileRenameText;
            set => SetProperty(ref _selectedProfileRenameText, value);
        }

        private bool _isSelectedProfileRenaming;
        public bool IsSelectedProfileRenaming
        {
            get => _isSelectedProfileRenaming;
            set => SetProperty(ref _isSelectedProfileRenaming, value);
        }

        public ManageProfileWindow()
        {
            InitializeComponent();
            DataContext = this;

            ProfilesList = ProfileManager.Profiles;
            CurrentProfile = ProfileManager.SelectedProfile;

            RenameCurrentProfileCommand = new RelayCommand(RenameCurrentProfile);
            AcceptRenameCurrentProfileCommand = new RelayCommand(AcceptRenameCurrentProfile, () => !string.IsNullOrWhiteSpace(CurrentProfileRenameText));
            DiscardRenameCurrentProfileCommand = new RelayCommand(DiscardRenameCurrentProfile);
            AddNewProfileCommand = new RelayCommand(AddNewProfile);
            DeleteSelectedProfileCommand = new RelayCommand(DeleteSelectedProfile, () => SelectedProfile != null && SelectedProfile?.Id != CurrentProfile?.Id);
            SetDefaultProfileCommand = new RelayCommand(SetDefaultProfile, () => SelectedProfile != null);
            RenameSelectedProfileCommand = new RelayCommand(RenameSelectedProfile, () => SelectedProfile != null);
            AcceptRenameSelectedProfileCommand = new RelayCommand(AcceptRenameSelectedProfile, () => !string.IsNullOrWhiteSpace(SelectedProfileRenameText));
            DiscardRenameSelectedProfileCommand = new RelayCommand(DiscardRenameSelectedProfile);
            ImportProfileCommand = new RelayCommand(ImportProfile, () => SelectedProfile != null && SelectedProfile?.Id != CurrentProfile?.Id);
            SwitchProfileCommand = new RelayCommand(SwitchProfile, () => SelectedProfile != null && SelectedProfile?.Id != CurrentProfile?.Id);
            ExportProfileCommand = new RelayCommand(ExportProfile);
            HelpCommand = new RelayCommand(p =>
            {
                Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/1FmHbjoEmuxzWGgGDhnf68sHzoTcfJ-moU_9Hgh_nsxI/edit#heading=h.1xhmjnounojy", UseShellExecute = true });
            });
        }

        private void ExportProfile(object param)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Profile Json file (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    System.IO.File.Copy(CurrentProfile.Path, saveFileDialog.FileName, true);
                }
            }
            catch (Exception ex)
            {
                WindowsHelper.ShowMessageDialog(Properties.Resources.ErrorOccuredWhileExporting + Environment.NewLine + ex.Message, App.Current.MainWindow);
            }
        }

        private void RenameCurrentProfile(object param)
        {
            CurrentProfileRenameText = CurrentProfile.Name;
            IsCurrentProfileRenaming = true;
        }

        private void AcceptRenameCurrentProfile(object param)
        {
            CurrentProfile.Name = CurrentProfileRenameText;
            var profile = ProfilesList.FirstOrDefault(x => x.Id == CurrentProfile.Id);
            if (profile != null)
            {
                profile.Name = CurrentProfileRenameText;
            }
            Task.Run(() => ProfileManager.SaveProfiles().Wait());
            IsCurrentProfileRenaming = false;
        }

        private void DiscardRenameCurrentProfile(object param)
        {
            IsCurrentProfileRenaming = false;
        }

        private void AddNewProfile(object param)
        {
            var newGuid = Guid.NewGuid();
            var newProfile = new ProfileFile()
            {
                Id = newGuid,
                IsDefault = !ProfilesList.Any(x => x.IsDefault),
                Name = GetNewProfileName(),
                Path = System.IO.Path.Combine(ProfileManager.ProfilesFolderPath, $"Profile_{newGuid}.json"),
            };

            var p = new Profile();
            Task.Run(() => p.SaveToJson(newProfile.Path).Wait());

            ProfilesList.Add(newProfile);
            SelectedProfile = newProfile;
            Task.Run(() => ProfileManager.SaveProfiles().Wait());
        }

        private void DeleteSelectedProfile(object param)
        {
            ProfilesList.Remove(SelectedProfile);
            SelectedProfile = ProfilesList.FirstOrDefault();
            Task.Run(() => ProfileManager.SaveProfiles().Wait());
        }

        private void SetDefaultProfile(object param)
        {
            var defProfile = ProfilesList.FirstOrDefault(x => x.IsDefault);
            if (defProfile != null)
            {
                defProfile.IsDefault = false;
            }
            SelectedProfile.IsDefault = true;

            Task.Run(() => ProfileManager.SaveProfiles().Wait());
        }

        private void RenameSelectedProfile(object param)
        {
            SelectedProfileRenameText = SelectedProfile.Name;
            IsSelectedProfileRenaming = true;
        }

        private void AcceptRenameSelectedProfile(object param)
        {
            SelectedProfile.Name = SelectedProfileRenameText;
            var profile = ProfilesList.FirstOrDefault(x => x.Id == SelectedProfile.Id);
            if (profile != null)
            {
                profile.Name = SelectedProfileRenameText;
            }
            Task.Run(() => ProfileManager.SaveProfiles().Wait());
            IsSelectedProfileRenaming = false;
        }

        private void DiscardRenameSelectedProfile(object param)
        {
            IsSelectedProfileRenaming = false;
        }

        private void ImportProfile(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Profile importedProfile;
                    if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".xml")
                    {
                        importedProfile = Profile.LoadFromXml(openFileDialog.FileName);
                    }
                    else if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".json")
                    {
                        importedProfile = Profile.LoadFromJson(openFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.InvalidFileSelected);
                        return;
                    }

                    Task.Run(() => importedProfile.SaveToJson(SelectedProfile.Path).Wait());
                }
                catch (Exception e)
                {
                    Log.Logger.Error(string.Format(Properties.Resources.ErrorImportingProfileFromX, openFileDialog.FileName), e);
                }
            }                
        }

        private void SwitchProfile(object param)
        {
            ProfileManager.SwitchProfile(SelectedProfile);
            Close();
        }

        private string GetNewProfileName()
        {
            int i = 1;
            string profileName = Properties.Resources.NewProfile;
            while (ProfilesList.Any(x => x.Name == profileName))
            {
                profileName = $"{profileName}{i}";
                i++;
            }

            return profileName;
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
