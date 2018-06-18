using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DF RID: 1247
	public static class BackstoryDatabase
	{
		// Token: 0x06001630 RID: 5680 RVA: 0x000C4CF8 File Offset: 0x000C30F8
		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x000C4D08 File Offset: 0x000C3108
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

		// Token: 0x06001632 RID: 5682 RVA: 0x000C4DD8 File Offset: 0x000C31D8
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

		// Token: 0x06001633 RID: 5683 RVA: 0x000C4EA8 File Offset: 0x000C32A8
		public static bool TryGetWithIdentifier(string identifier, out Backstory bs, bool closestMatchWarning = true)
		{
			identifier = BackstoryDatabase.GetIdentifierClosestMatch(identifier, closestMatchWarning);
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x000C4ED4 File Offset: 0x000C32D4
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
							Log.Warning("Couldn't find exact match for backstory " + identifier + " , using closest match " + value.identifier, false);
						}
						return value.identifier;
					}
				}
				Log.Warning("Couldn't find exact match for backstory " + identifier + ", or any close match.", false);
				result = identifier;
			}
			return result;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x000C4FBC File Offset: 0x000C33BC
		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement<KeyValuePair<string, Backstory>>().Value;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000C5004 File Offset: 0x000C3404
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

		// Token: 0x06001637 RID: 5687 RVA: 0x000C5088 File Offset: 0x000C3488
		public static string StripNumericSuffix(string key)
		{
			return BackstoryDatabase.regex.Match(key).Captures[0].Value;
		}

		// Token: 0x04000CEA RID: 3306
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		// Token: 0x04000CEB RID: 3307
		private static Dictionary<Pair<BackstorySlot, string>, List<Backstory>> shuffleableBackstoryList = new Dictionary<Pair<BackstorySlot, string>, List<Backstory>>();

		// Token: 0x04000CEC RID: 3308
		private static Regex regex = new Regex("^[^0-9]*");
	}
}
