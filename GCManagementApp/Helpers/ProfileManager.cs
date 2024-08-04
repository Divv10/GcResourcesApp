using GCManagementApp.Models;
using GCManagementApp.Static;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Helpers
{
    public static class ProfileManager
    {
        private static string ProfilesFilePath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\ProfilesList.json";
        private static string DefaultProfileFilePath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Profiles\\Default.json";
        public static string ProfilesFolderPath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Profiles";

        public static ObservableCollection<ProfileFile> Profiles { get; set; } = new ObservableCollection<ProfileFile>();

        public static ProfileFile SelectedProfile { get; set; }

        public static event EventHandler ProfileChanged = delegate { };
        public static event EventHandler ProfileNameChanged = delegate { };

        public async static void Initialize()
        {
            if (!File.Exists(ProfilesFilePath))
            {
                Profiles = new ObservableCollection<ProfileFile>();

                Task.Run(() => Profile.CheckJsonFile()).Wait();

                Profiles = new ObservableCollection<ProfileFile>();

                ProfileGrowth.Profile = Profile.LoadFromJson(DefaultProfileFilePath);

                Profiles.Add(new ProfileFile()
                {
                    Name = "Default",
                    Id = Guid.NewGuid(),
                    Path = DefaultProfileFilePath,
                    IsDefault = true,
                });
                await SaveProfiles();
                SelectedProfile = Profiles.FirstOrDefault();

                ProfileNameChanged(null, EventArgs.Empty);
                return;
            }

            Profiles = JsonConvert.DeserializeObject<ObservableCollection<ProfileFile>>(File.ReadAllText(ProfilesFilePath)) ?? new ObservableCollection<ProfileFile>();
            SelectedProfile = Profiles.FirstOrDefault(x => x.IsDefault) ?? Profiles.FirstOrDefault();
            ProfileGrowth.Profile = Profile.LoadFromJson(SelectedProfile?.Path ?? DefaultProfileFilePath);
            ProfileNameChanged(null, EventArgs.Empty);
        }

        public static void SwitchProfile(ProfileFile profile)
        {
            if (profile == null)
                return;

            SelectedProfile = profile;
            ProfileGrowth.Profile = Profile.LoadFromJson(SelectedProfile.Path);
            ProfileGrowth.InitializeAfterDownload();

            ProfileChanged(null, EventArgs.Empty); 
            ProfileNameChanged(null, EventArgs.Empty);
        }

        public static void ForceUpdate()
        {
            ProfileChanged(null, EventArgs.Empty);
        }

        public async static Task SaveProfiles()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Profiles, Formatting.Indented);
                FileInfo file = new System.IO.FileInfo(ProfilesFilePath);
                file.Directory?.Create();
                await File.WriteAllTextAsync(ProfilesFilePath, json);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving to json file.");
            }
        }
    }
}
