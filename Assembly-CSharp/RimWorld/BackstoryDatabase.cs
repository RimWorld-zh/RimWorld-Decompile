using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DB RID: 1243
	public static class BackstoryDatabase
	{
		// Token: 0x06001627 RID: 5671 RVA: 0x000C4CEC File Offset: 0x000C30EC
		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000C4CFC File Offset: 0x000C30FC
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

		// Token: 0x06001629 RID: 5673 RVA: 0x000C4DCC File Offset: 0x000C31CC
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

		// Token: 0x0600162A RID: 5674 RVA: 0x000C4E9C File Offset: 0x000C329C
		public static bool TryGetWithIdentifier(string identifier, out Backstory bs, bool closestMatchWarning = true)
		{
			identifier = BackstoryDatabase.GetIdentifierClosestMatch(identifier, closestMatchWarning);
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x000C4EC8 File Offset: 0x000C32C8
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

		// Token: 0x0600162C RID: 5676 RVA: 0x000C4FB0 File Offset: 0x000C33B0
		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement<KeyValuePair<string, Backstory>>().Value;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x000C4FF8 File Offset: 0x000C33F8
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

		// Token: 0x0600162E RID: 5678 RVA: 0x000C507C File Offset: 0x000C347C
		public static string StripNumericSuffix(string key)
		{
			return BackstoryDatabase.regex.Match(key).Captures[0].Value;
		}

		// Token: 0x04000CE7 RID: 3303
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		// Token: 0x04000CE8 RID: 3304
		private static Dictionary<Pair<BackstorySlot, string>, List<Backstory>> shuffleableBackstoryList = new Dictionary<Pair<BackstorySlot, string>, List<Backstory>>();

		// Token: 0x04000CE9 RID: 3305
		private static Regex regex = new Regex("^[^0-9]*");
	}
}
