using Emgu.CV.XFeatures2D;
using GCManagementApp.Enums;
using GCManagementApp.Models;
using GCManagementApp.Static;
using PixelLab.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GCManagementApp.Helpers
{
	public static class BuildsHelper
	{
		public static Dictionary<Tuple<HeroEnum, HeroType>, List<ContentKey>> ContentKeys;

		public static Dictionary<Tuple<HeroEnum, HeroType>, List<RecommendedEquipment>> RecommendedEquipments = new Dictionary<Tuple<HeroEnum, HeroType>, List<RecommendedEquipment>>();

		public static Dictionary<Tuple<HeroEnum, HeroType>, HeroBuild> Builds;

		private static string lookupRegex = @"(?<=\"")(.*?)(?=\"")";

		public static void GetRecommendedBuilds()
		{
			//GetContentKeys();
			//GetEquipment();
			GetBuilds();
		}

		public static RecommendedBuild GetBuildForHero(Tuple<HeroEnum, HeroType> hero)
		{
			//ContentKeys.TryGetValue(hero, out var ck);
			//RecommendedEquipments.TryGetValue(hero, out var re);
			Builds.TryGetValue(hero, out var build);

			return new RecommendedBuild() { HeroBuild = build };
		}

		// private static void GetContentKeys()
		// {
		//    ContentKeys = new Dictionary<Tuple<HeroEnum, HeroType>, List<ContentKey>>();

		//    var gsh = new GoogleSheetsHelper(GoogleAuth.DatabaseSheetId, GoogleAuth.ContentKeysSheetName);
		//    var gsp = new GoogleSheetParameters() { RangeColumnStart = 2, RangeRowStart = 1, RangeColumnEnd = 8, RangeRowEnd = 100, FirstRowIsHeaders = false, SheetName = gsh.SheetName };
		//    var values = gsh.GetDataFromSheet(gsp);

		//    string lastName = null;
		//    foreach (IDictionary<string, object> contentKey in values)
		//    {
		//        var name = contentKey["Column0"] as string;
		//        var key = contentKey["Column6"] as string;

		//        if (string.IsNullOrWhiteSpace(key))
		//            continue;

		//        if (string.IsNullOrWhiteSpace(name))
		//            name = lastName;
		//        else
		//            lastName = name;

		//        var index = key.IndexOf("V1_");
		//        var ck = new ContentKey() { Name = name, Key = key.Substring(index + 3), Heroes = new List<string>() };

		//        var heroes = key.Split('_', StringSplitOptions.RemoveEmptyEntries).Take(4);
		//        foreach (var hero in heroes)
		//        {
		//            Tuple<HeroEnum, HeroType> contentHero = null;
		//            if (hero.Contains("(T)"))
		//            {
		//                var heroName = hero.Replace("(T)", "").Replace("MyungHwarin", "Hwarin");
		//                if (Enum.TryParse<HeroEnum>(heroName, true, out var h))
		//                {
		//                    contentHero = new Tuple<HeroEnum, HeroType>(h, HeroType.T);
		//                }
		//            }
		//            else if (hero.Contains("(S)"))
		//            {
		//                var heroName = hero.Replace("(S)", "").Replace("MyungHwarin", "Hwarin");
		//                if (Enum.TryParse<HeroEnum>(heroName, true, out var h))
		//                {
		//                    contentHero = new Tuple<HeroEnum, HeroType>(h, HeroType.S);
		//                }
		//            }
		//            else
		//            {
		//                if (Enum.TryParse<HeroEnum>(hero.Replace("MyungHwarin", "Hwarin"), true, out var h))
		//                {
		//                    contentHero = new Tuple<HeroEnum, HeroType>(h, HeroType.SR);
		//                }
		//            }

		//            if (contentHero != null )
		//            {
		//                ck.Heroes.Add($"{contentHero.Item1}{contentHero.Item2}");
		//                if (ContentKeys.TryGetValue(contentHero, out var keysList))
		//                {
		//                    keysList.Add(ck);
		//                }
		//                else
		//                {
		//                    ContentKeys.Add(contentHero, new List<ContentKey> { ck });
		//                }
		//            }
		//        }
		//    }
		//}

		//private static void GetEquipment()
		//{
		//    RecommendedEquipments = new Dictionary<Tuple<HeroEnum, HeroType>, List<RecommendedEquipment>>();

		//    var gsh = new GoogleSheetsHelper(GoogleAuth.DatabaseSheetId, GoogleAuth.EquipmentSheetName);
		//    var gsp = new GoogleSheetParameters() { RangeColumnStart = 1, RangeRowStart = 1, RangeColumnEnd = 25, RangeRowEnd = 145, FirstRowIsHeaders = false, SheetName = gsh.SheetName };
		//    var values = gsh.GetDataFromSheet(gsp, true);

		//    List<string[]> vals = new List<string[]>();
		//    foreach (IDictionary<string, object> eq in values)
		//    {
		//        var entry = new string[] { eq["Column0"] as string, eq["Column1"] as string, eq["Column2"] as string, eq["Column3"] as string };
		//        for (int i = 0; i < entry.Length; i++)
		//        {
		//            if (entry[i]?.IsNullOrWhiteSpace() == true)
		//            {
		//                entry[i] = vals.Last()[i];
		//            }
		//        }

		//        vals.Add(entry);

		//        var equipment = new RecommendedEquipment() { SubStat = entry[1], Description = entry[3] };
		//        switch (entry[0])
		//        {
		//            case "Blue + Red":
		//                equipment.SetColor = GearSet.RedBlue; break;
		//            case "Red":
		//                equipment.SetColor = GearSet.Red; break;
		//            case "Pink":
		//                equipment.SetColor = GearSet.Purple; break;
		//            case "Orange":
		//                equipment.SetColor = GearSet.Orange; break;
		//            case "Green":
		//                equipment.SetColor = GearSet.Green; break;
		//        }

		//        var heroes = entry[2].Split(',', StringSplitOptions.RemoveEmptyEntries);
		//        foreach (var h in heroes)
		//        {
		//            Tuple<HeroEnum, HeroType> contentHero = null;
		//            var name = h.Replace("[?]", "").Replace(" ", "").Trim();
		//            if (name.EndsWith("T"))
		//            {
		//                name = name.Replace("T", "");
		//                if (Enum.TryParse<HeroEnum>(name, true, out var heroEnum))
		//                {
		//                    contentHero = new Tuple<HeroEnum, HeroType>(heroEnum, HeroType.T);
		//                }
		//            }
		//            else
		//            {
		//                if (Enum.TryParse<HeroEnum>(name, true, out var heroEnum))
		//                {
		//                    contentHero = new Tuple<HeroEnum, HeroType>(heroEnum, HeroType.SR);
		//                }
		//            }

		//            if (contentHero != null)
		//            {
		//                if (RecommendedEquipments.TryGetValue(contentHero, out var equips))
		//                {
		//                    equips.Add(equipment);
		//                }
		//                else
		//                {
		//                    RecommendedEquipments.Add(contentHero, new List<RecommendedEquipment> { equipment });
		//                }
		//            }
		//        }
		//    }
		//}

		private static void GetBuilds()
		{
			Builds = new Dictionary<Tuple<HeroEnum, HeroType>, HeroBuild>();

			var gsh = new GoogleSheetsHelper(GoogleAuth.DatabaseSheetId, GoogleAuth.BuildsSheetName);
			var gsp = new GoogleSheetParameters() { RangeColumnStart = 1, RangeRowStart = 1, RangeColumnEnd = 20, RangeRowEnd = 100, FirstRowIsHeaders = false, SheetName = gsh.SheetName };
			var values = gsh.GetDataFromSheet(gsp, true);

			foreach (IDictionary<string, object> build in values)
			{
				var name = build["Column2"] as string;
				//var sets = build["Column3"] as string;
				//var enchant1 = build["Column4"] as string;
				//var substat1 = build["Column5"] as string;
				//var enchant2 = build["Column6"] as string;
				//var enchant3 = build["Column7"] as string;
				var cs1 = build["Column3"] as string;
				var cs2 = build["Column4"] as string;
				var cs3 = build["Column5"] as string;
				var cs4 = build["Column6"] as string;
				var cs5 = build["Column7"] as string;
				var trait1 = build["Column8"] as string;
				var trait2 = build["Column9"] as string;
				var trait3 = build["Column10"] as string;
				var trait4 = build["Column11"] as string;
				var trait5 = build["Column12"] as string;
				var acc1 = build["Column13"] as string;
				var acc2 = build["Column14"] as string;
				var acc3 = build["Column15"] as string;
				var accColor = build["Column16"] as string;
				var arti = build["Column17"] as string;
				var siTraits = build["Column18"] as string;

				Tuple<HeroEnum, HeroType> contentHero = null;
				name = name.Replace(" ", "").Trim();
				if (name.EndsWith("T"))
				{
					if (name != "TiaT")
					{
						name = name.Replace("T", "");
					} else
					{
						name = "Tia";
					}
					if (Enum.TryParse<HeroEnum>(name, true, out var heroEnum))
					{
						contentHero = new Tuple<HeroEnum, HeroType>(heroEnum, HeroType.T);
					}
				}
				else if (name.EndsWith("S"))
				{
					name = name.Replace("S", "");
					if (Enum.TryParse<HeroEnum>(name, true, out var heroEnum))
					{
						contentHero = new Tuple<HeroEnum, HeroType>(heroEnum, HeroType.S);
					}
				}
				else
				{
					if (Enum.TryParse<HeroEnum>(name, true, out var heroEnum))
					{
						contentHero = new Tuple<HeroEnum, HeroType>(heroEnum, HeroType.SR);
					}
				}

				if (contentHero != null)
				{
					var b = new HeroBuild() { Accessories = new string[] { acc1, acc2, acc3}, AccessoryColor = new AccessorySetEnum(), CsTraits = new Dictionary<ChaserTraitEnum, int>(), LvlTraits = new Dictionary<LevelTraitEnum, int>(), Artifact = null, SiTypes = new string[] { "Memory Core", "Body Core", "Soul Core" }, SiTraitList = new List<List<int>>() };
					Regex rg = new Regex(lookupRegex);
					for (int i = 0; i < 3; i++)
					{
						var stat = rg.Match(b.Accessories[i]).Value;

						b.Accessories[i] = stat.ToUpperFirstLetter().Replace("2", "");

					}
					var acc = rg.Match(accColor).Value;
					AccessorySetEnum accSetColor = AccessorySetEnum.None;

					switch (accColor)
					{
						case "Purple": accSetColor = AccessorySetEnum.Purple; break;
						case "Orange": accSetColor = AccessorySetEnum.Orange; break;
						case "Blue": accSetColor = AccessorySetEnum.Blue; break;
					}
					b.AccessoryColor = accSetColor;
					
					var artifact = rg.Match(arti).Value;
					if (artifact != "")
					{
						ArtifactType artiType = ArtifactType.Normal;
						var artiTypeString = "";

						switch (artifact.First())
						{
							case 'F': artiType = ArtifactType.Frozen; artiTypeString = "Frozen"; break;
							case 'C': artiType = ArtifactType.Cursed; artiTypeString = "Cursed"; break;
							case 'B': artiType = ArtifactType.Burning; artiTypeString = "Burning"; break;
						}
						b.Artifact = artiTypeString + " " + artifact.Substring(1, artifact.Length - 1);
						b.ArtifactType = artiType;
					} else
					{
						ArtifactType artiType = ArtifactType.Normal;
						b.Artifact = "Normal";
						b.ArtifactType = artiType;
					}
					
					var siCoresSplit = siTraits.Split("-");
					var memCore = siCoresSplit[0];
					var bodyCore = siCoresSplit[1];
					var soulCore = siCoresSplit[2];
					
					var memCoreList = new List<int> { };
					var bodyCoreList = new List<int> { };
					var soulCoreList = new List<int> { };

					for (var i = 0; i < memCore.Length; i++)
					{
						memCoreList.Add(Convert.ToInt32(memCore[i].ToString()));
					}
					for (var i = 0; i < bodyCore.Length; i++)
					{
						bodyCoreList.Add(Convert.ToInt32(bodyCore[i].ToString()));
					}
					for (var i = 0; i < soulCore.Length; i++)
					{
						soulCoreList.Add(Convert.ToInt32(soulCore[i].ToString()));
					}
					b.SiTraitList.Add(memCoreList);
					b.SiTraitList.Add(bodyCoreList);
					b.SiTraitList.Add(soulCoreList);				

		//foreach (var set in sets.Split("/", StringSplitOptions.TrimEntries))
		//{
		//    GearSet setEnum = GearSet.None;
		//    switch (set)
		//    {
		//        case "R": setEnum = GearSet.Red; break;
		//        case "2R2B": setEnum = GearSet.RedBlue; break;
		//        case "G": setEnum = GearSet.Green; break;
		//        case "O": setEnum = GearSet.Orange; break;
		//        case "P": setEnum = GearSet.Purple; break;
		//    }

		//    if (setEnum != GearSet.None)
		//    {
		//        b.Sets.Add(setEnum);
				//var equipment = new RecommendedEquipment() { SubStat = enchant2, SetColor = setEnum };
		//        if (RecommendedEquipments.TryGetValue(contentHero, out var equips))
		//        {
		//            equips.Add(equipment);
		//        }
		//        else
		//        {
		//            RecommendedEquipments.Add(contentHero, new List<RecommendedEquipment> { equipment });
		//        }
		//    }
		//}

					foreach (var trait in new[] {trait1, trait2, trait3, trait4, trait5})
					{
						var points = 5;
						var traitName = rg.Match(trait).Value.Replace(" ", "");

						if (trait == trait5) points = 3;

						LevelTraitEnum levelTrait = LevelTraitEnum.None;
						switch (traitName) {
							case "CRIT": levelTrait = LevelTraitEnum.CritChance; break;
							case "CDR": levelTrait = LevelTraitEnum.SkillCooldownReduction; break;
							case "ASI": levelTrait = LevelTraitEnum.BasicAtkSpeedIncrease; break;
							case "BDR": levelTrait = LevelTraitEnum.BasicAtkDamageReduction; break;
							case "SDR": levelTrait = LevelTraitEnum.SkillAtkDamageReduction; break;
							case "BDI": levelTrait = LevelTraitEnum.BasicAtkDamageIncrease; break;
							case "SDI": levelTrait = LevelTraitEnum.SkillAtkDamageIncrease; break;
							case "TDI": levelTrait = LevelTraitEnum.TrueDamageIncrease; break;
							case "IRH": levelTrait = LevelTraitEnum.HealingReceivedIncrease; break;
						}
						if (levelTrait != LevelTraitEnum.None)
						{
							b.LvlTraits.Add(levelTrait, points);
						}
					}

					foreach (var trait in Enum.GetValues<LevelTraitEnum>())
					{
						if (trait == LevelTraitEnum.None)
							continue;

						if (!b.LvlTraits.TryGetValue(trait, out _))
							b.LvlTraits.Add(trait, 0);
					}

					foreach (var cs in new[] {cs1, cs2, cs3, cs4, cs5})
					{
						if (cs?.IsNullOrWhiteSpace() == true)
							continue;

						var points = 5;
						var csName = rg.Match(cs).Value.Replace(" ", "");

						switch (csName.Last())
						{
							case '1': csName = csName.Substring(0, csName.Length - 1); points = 1; break;
							case '2': csName = csName.Substring(0, csName.Length - 1); points = 2; break;
							case '3': csName = csName.Substring(0, csName.Length - 1); points = 3; break;
							case '4': csName = csName.Substring(0, csName.Length - 1); points = 4; break;
						}

						ChaserTraitEnum chaserTrait = ChaserTraitEnum.None;
						switch (csName)
						{
							case "EP": chaserTrait = ChaserTraitEnum.ElevatedPower; break;
							case "LL": chaserTrait = ChaserTraitEnum.LongLife; break;
							case "HLP": chaserTrait = ChaserTraitEnum.Helper; break;
							case "POB": chaserTrait = ChaserTraitEnum.PrayerOfBlessing; break;
							case "IH": chaserTrait = ChaserTraitEnum.InvisibleHand; break;
							case "DP": chaserTrait = ChaserTraitEnum.DivineProtection; break;
							case "PL": chaserTrait = ChaserTraitEnum.PureLuck; break;
							case "BOL": chaserTrait = ChaserTraitEnum.BreathOfLife; break;
							case "FATE": chaserTrait = ChaserTraitEnum.Connection; break;
							case "IMP": chaserTrait = ChaserTraitEnum.Impulse; break;
							case "PE": chaserTrait = ChaserTraitEnum.PersistentExecutioner; break;
							case "SH": chaserTrait = ChaserTraitEnum.SoulHealer; break;
						}

						if (chaserTrait != ChaserTraitEnum.None)
						{
							b.CsTraits.Add(chaserTrait, points);
						}
					}

					var pointsSpent = b.CsTraits.Sum(x => x.Value);
					if (pointsSpent == 20)
					{
						b.CsTraits.Add(ChaserTraitEnum.LeftChaser, 3);
						b.CsTraits.Add(ChaserTraitEnum.RightChaser, 2);
					}
					else if (pointsSpent == 22)
					{
						b.CsTraits.Add(ChaserTraitEnum.LeftChaser, 3);
					}
					else if (pointsSpent == 23)
					{
						b.CsTraits.Add(ChaserTraitEnum.RightChaser, 2);
					}

					foreach (var cs in Enum.GetValues<ChaserTraitEnum>())
					{
						if (cs == ChaserTraitEnum.None)
							continue;

						if (!b.CsTraits.TryGetValue(cs, out _))
							b.CsTraits.Add(cs, 0);
					}

					Builds.Add(contentHero, b);
				}
			}
		}
	}
}
