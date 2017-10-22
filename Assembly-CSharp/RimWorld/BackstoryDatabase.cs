using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class BackstoryDatabase
	{
		public static Dictionary<string, Backstory> allBackstories = new Dictionary<string, Backstory>();

		public static void Clear()
		{
			BackstoryDatabase.allBackstories.Clear();
		}

		public static void ReloadAllBackstories()
		{
			foreach (Backstory item in DirectXmlLoader.LoadXmlDataInResourcesFolder<Backstory>("Backstories/Shuffled"))
			{
				item.PostLoad();
				item.ResolveReferences();
				foreach (string item2 in item.ConfigErrors(false))
				{
					Log.Error(item.Title + ": " + item2);
				}
				BackstoryDatabase.AddBackstory(item);
			}
			SolidBioDatabase.LoadAllBios();
		}

		public static void AddBackstory(Backstory bs)
		{
			BackstoryHardcodedData.InjectHardcodedData(bs);
			if (BackstoryDatabase.allBackstories.ContainsKey(bs.identifier))
			{
				Log.Error("Backstory " + bs.Title + " has same unique save key " + bs.identifier + " as old backstory " + BackstoryDatabase.allBackstories[bs.identifier].Title);
			}
			else
			{
				BackstoryDatabase.allBackstories.Add(bs.identifier, bs);
			}
		}

		public static bool TryGetWithIdentifier(string identifier, out Backstory bs)
		{
			return BackstoryDatabase.allBackstories.TryGetValue(identifier, out bs);
		}

		public static Backstory RandomBackstory(BackstorySlot slot)
		{
			return (from bs in BackstoryDatabase.allBackstories
			where bs.Value.slot == slot
			select bs).RandomElement().Value;
		}
	}
}
