using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DivideGT
{
    public class TournamentProviderGDrive : ITournamentProvider
    {

        public string url { get; set; }

        private TournamentCollection _collection;
        public TournamentCollection GetCollection()
        {
            if (_collection == null)
            {
                AppStatic.logger?.Info("Getting tournament data...");
                SplashScreen ss = new SplashScreen();

                ss.Show();

                _collection = new TournamentCollection();
                AppStatic.logger?.Info($"URL: {url}");
                var textFromFile = (new WebClient()).DownloadString(url);
                AppStatic.logger?.Info("Content loaded!");
                string[] array = textFromFile.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                AppStatic.logger?.Info($"Lines: {array.Length}");
                foreach (var line in array)
                {
                    if (line.StartsWith(";"))
                        continue;
                    _collection.AddRecord(line.GetTournamentRecord());
                }
                AppStatic.logger?.Info("Parsing data done");

                ss.Close();                
            }
            return _collection;
        }

        public TournamentProviderGDrive()
        {

        }

        public List<TournamentDate> GetTournaments()
        {
            return GetCollection().GetAvailableTournaments();
        }
    }
}
