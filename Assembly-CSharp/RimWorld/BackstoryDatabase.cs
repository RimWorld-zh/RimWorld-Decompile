using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	public static class BackstoryDatabase
	{
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		private static Dictionary<Pair<BackstorySlot, string>, List<Backstory>> shuffleableBackstoryList = new Dictionary<Pair<BackstorySlot, string>, List<Backstory>>();

		private static Regex regex = new Regex("^[^0-9]*");

		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		public static void ReloadAllBackstories()
		{
			foreach (Backstory backstory in DirectXmlLoader.LoadXmlDataInResourcesFolder<Backstory>("Backstories/Shuffled"))
			{
				backstory.PostLoad();
				backstory.ResolveReferences();
				foreach (string str in backstory.ConfigErrors(false))
				{
					Log.Error(backstory.title + ": " + str, false);
				}
				BackstoryDatabase.AddBackstory(backstory);
			}
			SolidBioDatabase.LoadAllBios();
		}

		public static void AddBackstory(Backstory bs)
		{
			BackstoryHardcodedData.InjectHardcodedData(bs);
			if (BackstoryDatabase.allBackstories.ContainsKey(bs.identifier))
			{
				if (bs == BackstoryDatabase.allBackstories[bs.identifier])
				{
					Log.Error("Tried to add the same backstory twice " + bs.identifier, false);
				}
				else
				{
					Log.Error(string.Concat(new string[]
					{
						"Backstory ",
						bs.title,
						" has same unique save key ",
						bs.identifier,
						" as old backstory ",
						BackstoryDatabase.allBackstories[bs.identifier].title
					}), false);
				}
			}
			else
			{
				BackstoryDatabase.allBackstories.Add(bs.identifier, bs);
				BackstoryDatabase.shuffleableBackstoryList.Clear();
			}
		}

		public static bool TryGetWithIdentifier(string identifier, out Backstory bs, bool closestMatchWarning = true)
		{
			identifier = BackstoryDatabase.GetIdentifierClosestMatch(identifier, closestMatchWarning);
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		public static string GetIdentifierClosestMatch(string identifier, bool closestMatchWarning = true)
		{
			string result;
			if (BackstoryDatabase.allBackstories.ContainsKey(identifier))
			{
				result = identifier;
			}
			else
			{
				string b = BackstoryDatabase.StripNumericSuffix(identifier);
				foreach (KeyValuePair<string, Backstory> keyValuePair in BackstoryDatabase.allBackstories)
				{
					Backstory value = keyValuePair.Value;
					if (BackstoryDatabase.StripNumericSuffix(value.identifier) == b)
					{
						if (closestMatchWarning)
						{
							Log.Warning("Couldn't find exact match for backstory " + identifier + ", using closest match " + value.identifier, false);
						}
						return value.identifier;
					}
				}
				Log.Warning("Couldn't find exact match for backstory " + identifier + ", or any close match.", false);
				result = identifier;
			}
			return result;
		}

		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement<KeyValuePair<string, Backstory>>().Value;
		}

		public static List<Backstory> ShuffleableBackstoryList(BackstorySlot slot, string tag)
		{
			Pair<BackstorySlot, string> key = new Pair<BackstorySlot, string>(slot, tag);
			if (!BackstoryDatabase.shuffleableBackstoryList.ContainsKey(key))
			{
				BackstoryDatabase.shuffleableBackstoryList[key] = (from bs in BackstoryDatabase.allBackstories.Values
				where bs.shuffleable && bs.slot == slot && bs.spawnCategories.Contains(tag)
				select bs).ToList<Backstory>();
			}
			return BackstoryDatabase.shuffleableBackstoryList[key];
		}

		public static string StripNumericSuffix(string key)
		{
			return BackstoryDatabase.regex.Match(key).Captures[0].Value;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static BackstoryDatabase()
		{
		}

		[CompilerGenerated]
		private sealed class <RandomBackstory>c__AnonStorey0
		{
			internal BackstorySlot slot;

			public <RandomBackstory>c__AnonStorey0()
			{
			}

			internal bool <>m__0(KeyValuePair<string, Backstory> bs)
			{
				return bs.Value.slot == this.slot;
			}
		}

		[CompilerGenerated]
		private sealed class <ShuffleableBackstoryList>c__AnonStorey1
		{
			internal BackstorySlot slot;

			internal string tag;

			public <ShuffleableBackstoryList>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Backstory bs)
			{
				return bs.shuffleable && bs.slot == this.slot && bs.spawnCategories.Contains(this.tag);
			}
		}
	}
}
