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
    /// Interaction logic for GoogleDriveWindow.xaml
    /// </summary>
    public partial class GoogleDriveWindow : Window, INotifyPropertyChanged
    {
        private static readonly string[] Scopes = new[] { DriveService.Scope.DriveAppdata };
        private static readonly string ClientId = "id";
        private static readonly string ClientSecret = "secret";

        public ICommand LoginToDriveCommand { get; }
        public ICommand DownloadFromDriveCommand { get; }
        public ICommand UploadToDriveCommand { get; }

        private GoogleLoginStatus _loginStatus;
        public GoogleLoginStatus LoginStatus
        {
            get => _loginStatus;
            set
            {
                SetProperty(ref _loginStatus, value);
                ProfileGrowth.Profile.GoogleLoginStatus = value;
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private UserCredential _userCredential 
        {
            get => ProfileGrowth.Profile.GoogleUserCredentials;
            set => ProfileGrowth.Profile.GoogleUserCredentials = value;
        }
        private DriveService _driveService 
        {
            get => ProfileGrowth.Profile.GoogleDriveService;
            set => ProfileGrowth.Profile.GoogleDriveService = value;
        }

        public GoogleDriveWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoginStatus = ProfileGrowth.Profile.GoogleLoginStatus;
            Username = ProfileGrowth.Profile.GoogleDisplayName;
            if (_userCredential == null || _driveService == null)
            {
                LoginStatus = GoogleLoginStatus.NotLogged;
            }

            LoginToDriveCommand = new RelayCommand(o => LoginToDrive());
            DownloadFromDriveCommand = new RelayCommand(o => DownloadFromDrive(), () => LoginStatus == GoogleLoginStatus.Logged);
            UploadToDriveCommand = new RelayCommand(o => UploadToDrive(), () => LoginStatus == GoogleLoginStatus.Logged);            
        }

        private async void LoginToDrive()
        {
            LoginStatus = GoogleLoginStatus.Logging;
            try
            {
                await Run();
                LoginStatus = GoogleLoginStatus.Logged;
            }
            catch 
            {
                LoginStatus = GoogleLoginStatus.Error;
            }
        }

        private async Task Run()
        {
            ClientSecrets secrets = new ClientSecrets()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));
            CancellationToken ct = cts.Token;

            _userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, Scopes, "user", ct);

            // Create the service.
            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _userCredential,
                ApplicationName = "GcManagementApp",
            });

            var request = _driveService.About.Get();
            request.Fields = "user";
            var about = request.Execute();

            Username = about.User.DisplayName;
            ProfileGrowth.Profile.GoogleDisplayName = Username;
        }

        private async void DownloadFromDrive()
        {
            LoginStatus = GoogleLoginStatus.Logging;

            try
            {
                await DownloadFromDriveImpl();
                WindowsHelper.ShowMessageDialog(Properties.Resources.ProgressDownloaded, this);

                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
            catch 
            {
                WindowsHelper.ShowMessageDialog(Properties.Resources.ErrorDownloading, this);
            }
            finally
            {
                LoginStatus = GoogleLoginStatus.Logged;
                OnPropertyChanged();
            }
        }

        private async Task DownloadFromDriveImpl()
        {
            await _userCredential.RefreshTokenAsync(CancellationToken.None);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(15));
            CancellationToken ct = cts.Token;

            var request = _driveService.Files.List();
            request.Spaces = "appDataFolder";
            request.Fields = "nextPageToken, files(id, name)";
            var result = await request.ExecuteAsync(ct);
            if (result.Files == null || result.Files.Count == 0)
            {
                WindowsHelper.ShowMessageDialog(Properties.Resources.NoProgressFound, this);
                return;
            }

            var progressId = result.Files.FirstOrDefault();

            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(15));
            ct = cts.Token;

            var downloadRequest = _driveService.Files.Get(progressId.Id);
            Profile profile;
            using (var stream = new MemoryStream())
            {
                await downloadRequest.DownloadAsync(stream, ct);
                stream.Position = 0;

                try
                {
                    profile = Profile.LoadFromStreamJson(stream);
                }
                catch (JsonReaderException ex)
                {
                    cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromSeconds(15));
                    ct = cts.Token;

                    using (var xmlStream = new MemoryStream())
                    {
                        await downloadRequest.DownloadAsync(xmlStream, ct);
                        xmlStream.Position = 0;
                        profile = Profile.LoadFromStreamXml(xmlStream);
                    }
                }
            }

            ProfileGrowth.CreateBackup();

            ProfileGrowth.Profile.HeroPlans = profile.HeroPlans;
            ProfileGrowth.Profile.HeroesOwned = profile.HeroesOwned;
            ProfileGrowth.Profile.MaterialsInventory = profile.MaterialsInventory;
            ProfileGrowth.Profile.Settings = profile.Settings;
            ProfileGrowth.Profile.EwPlans = profile.EwPlans;

            ProfileGrowth.InitializeAfterDownload();
            await ProfileGrowth.ForceSaveToFile();
        }

        private async void UploadToDrive()
        {
            LoginStatus = GoogleLoginStatus.Logging;

            try
            {
                await UploadToDriveImpl();
            }
            catch
            {
                WindowsHelper.ShowMessageDialog(Properties.Resources.ErrorUploading, this);
            }
            finally
            {
                LoginStatus = GoogleLoginStatus.Logged;
                OnPropertyChanged();
            }
        }


        private async Task UploadToDriveImpl()
        {
            await _userCredential.RefreshTokenAsync(CancellationToken.None);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(15));
            CancellationToken ct = cts.Token;

            var request = _driveService.Files.List();
            request.Spaces = "appDataFolder";
            request.Fields = "nextPageToken, files(id, name)";
            var result = await request.ExecuteAsync(ct);
            foreach (var file in result.Files)
            {
                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(15));
                ct = cts.Token;

                var deleteRequest = _driveService.Files.Delete(file.Id);
                await deleteRequest.ExecuteAsync(ct);
            }

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "ProfileGrowth.xml",
                Parents = new List<string>()
                    {
                        "appDataFolder"
                    }
            };

            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(15));
            ct = cts.Token;

            FilesResource.CreateMediaUpload uploadRequest;
            var profileXml = ProfileGrowth.Profile.GetProfileJson();
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(profileXml)))
            {
                uploadRequest = _driveService.Files.Create(fileMetadata, stream, "application/xml");
                uploadRequest.Fields = "id";
                await uploadRequest.UploadAsync(ct);
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

    public enum GoogleLoginStatus
    {
        NotLogged,
        Logging,
        Logged,
        Error,
    }
}
