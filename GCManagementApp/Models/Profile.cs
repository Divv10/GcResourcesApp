using GCManagementApp.Helpers;
using GCManagementApp.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GCManagementApp.Models
{
    [Serializable]
    public class Profile : NotifyPropertyChanged
    {
        private const string xmlProfileFilePath = "Profile\\profile.xml";
        private const string xmlProfileFileBackupPath = "Profile\\profile_backup.xml";

        private static string jsonProfileDefaultFilePath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Profiles\\Default.json";
        private static string jsonProfileFileBackupPath { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Profiles\\Default_backup.json";

        public string CurrentProfileJsonFilePath => ProfileManager.SelectedProfile.Path;

        [XmlIgnore]
        [JsonIgnore]
        public GoogleLoginStatus GoogleLoginStatus { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public UserCredential? GoogleUserCredentials { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DriveService? GoogleDriveService { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public string GoogleDisplayName { get; set; }

        public List<ProfileHeroGrowth> HeroesOwned { get; set; }

        public Inventory MaterialsInventory { get; set; }

        public Settings Settings { get; set; }

        public List<HeroPlan> HeroPlans { get; set; }

        public List<EwPlan> EwPlans { get; set; }

        private ClassGearSlots _tankGearSlots;
        public ClassGearSlots TankGearSlots
        {
            get => _tankGearSlots;
            set => SetProperty(ref _tankGearSlots, value);
        }

        private ClassGearSlots _assaultGearSlots;
        public ClassGearSlots AssaultGearSlots
        {
            get => _assaultGearSlots;
            set => SetProperty(ref _assaultGearSlots, value);
        }

        private ClassGearSlots _mageGearSlots;
        public ClassGearSlots MageGearSlots
        {
            get => _mageGearSlots;
            set => SetProperty(ref _mageGearSlots, value);
        }

        private ClassGearSlots _rangerGearSlots;
        public ClassGearSlots RangerGearSlots
        {
            get => _rangerGearSlots;
            set => SetProperty(ref _rangerGearSlots, value);
        }

        private ClassGearSlots _healerGearSlots;
        public ClassGearSlots HealerGearSlots
        {
            get => _healerGearSlots;
            set => SetProperty(ref _healerGearSlots, value);
        }

        private List<List<Hero>> _vulcanusTeams;
        public List<List<Hero>> VulcanusTeams
        {
            get => _vulcanusTeams;
            set => SetProperty(ref _vulcanusTeams, value);
        }

        private bool[] _vulcanusSweeps;
        public bool[] VulcanusSweeps
        {
            get => _vulcanusSweeps;
            set => SetProperty(ref _vulcanusSweeps, value);
        }

        private List<SerializableContentTeam> _contentTeams;
        public List<SerializableContentTeam> ContentTeams
        {
            get => _contentTeams;
            set => SetProperty(ref _contentTeams, value);
        }

        private string _contentTeamName;
        public string ContentTeamName
        {
            get => _contentTeamName;
            set => SetProperty(ref _contentTeamName, value);
        }

        private string _tierListCreditsText;
        public string TierListCreditsText
        {
            get => _tierListCreditsText;
            set => SetProperty(ref _tierListCreditsText, value);
        }

        private List<TierListTeam> _tierListTeams;
        public List<TierListTeam> TierListTeams
        {
            get => _tierListTeams;
            set => SetProperty(ref _tierListTeams, value);
        }

        #region xml

        [Obsolete("Use Json instead", true)]
        public static Profile LoadFromXml()
        {
            if (!File.Exists(xmlProfileFilePath))
                return new Profile();
            return XmlGenericSerializer<Profile>.Deserialize(xmlProfileFilePath);
        }

        public static Profile LoadFromXml(string path)
        {
            return XmlGenericSerializer<Profile>.Deserialize(path);
        }

        [Obsolete("Use Json instead", true)]
        public string GetProfileXml()
        {
            return XmlGenericSerializer<Profile>.Serialize(this);
        }

        [Obsolete("Use Json instead")]
        public static Profile LoadFromStreamXml(Stream stream)
        {
            return XmlGenericSerializer<Profile>.DeserializeFromStream(stream);
        }

        private static bool _isSavePending = false;

        [Obsolete("Use Json instead", true)]
        public async void SaveToXml()
        {
            if (_isSavePending)
                return;

            _isSavePending = true;
            Debug.WriteLine("---------------------- Saving");
            await Task.Delay(3000);
            SaveToXmlImpl();
            _isSavePending = false;
        }

        [Obsolete("Use Json instead", true)]
        public void ForceSaveToXml()
        {
            SaveToXmlImpl();
        }

        private async void SaveToXmlImpl()
        {
            try
            {
                string xml = XmlGenericSerializer<Profile>.Serialize(this);
                FileInfo file = new System.IO.FileInfo(xmlProfileFilePath);
                file.Directory?.Create();
                await File.WriteAllTextAsync(xmlProfileFilePath, xml);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving to xml file.");
            }
        }

        #endregion

        public void CreateBackup()
        {
            if (File.Exists(CurrentProfileJsonFilePath))
            {
                FileInfo Sourcefile = new FileInfo(CurrentProfileJsonFilePath);
                Sourcefile.CopyTo(jsonProfileFileBackupPath, true);
            }
        }        

        public static Profile LoadFromJson(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return new Profile();
            return JsonConvert.DeserializeObject<Profile>(File.ReadAllText(jsonPath)) ?? new Profile();
        }

        public string GetProfileJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Profile LoadFromStreamJson(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<Profile>(jsonTextReader) ?? new Profile();
            }
        }

        public async void SaveToJson()
        {
            if (_isSavePending)
                return;

            _isSavePending = true;
            Debug.WriteLine("---------------------- Saving");
            await Task.Delay(3000);
            await SaveToJsonImpl();
            _isSavePending = false;
        }

        public async Task ForceSaveToJson()
        {
            await SaveToJsonImpl();
        }

        private async Task SaveToJsonImpl()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                FileInfo file = new System.IO.FileInfo(CurrentProfileJsonFilePath);
                file.Directory?.Create();
                await File.WriteAllTextAsync(CurrentProfileJsonFilePath, json);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving to json file.");
            }
        }

        public async Task SaveToJson(string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                FileInfo file = new System.IO.FileInfo(path);
                file.Directory?.Create();
                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving to json file.");
            }
        }

        public static async Task CheckJsonFile()
        {
            if (File.Exists(jsonProfileDefaultFilePath))
            {
                return;
            }

            if (File.Exists(xmlProfileFilePath))
            {
                var profile = XmlGenericSerializer<Profile>.Deserialize(xmlProfileFilePath);
                await profile.ForceSaveToJson();
            }
        }
    }
}
