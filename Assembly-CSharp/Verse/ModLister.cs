using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse.Steam;

namespace Verse
{
	public static class ModLister
	{
		private static List<ModMetaData> mods = new List<ModMetaData>();

		[CompilerGenerated]
		private static Func<ModMetaData, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ModMetaData, DirectoryInfo> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<DirectoryInfo, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<WorkshopItem, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ModMetaData, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<ModMetaData, bool> <>f__am$cache5;

		static ModLister()
		{
			ModLister.RebuildModList();
		}

		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		internal static void EnsureInit()
		{
		}

		internal static void RebuildModList()
		{
			string s = "Rebuilding mods list";
			ModLister.mods.Clear();
			s += "\nAdding mods from mods folder:";
			foreach (string localAbsPath in from d in new DirectoryInfo(GenFilePaths.CoreModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData = new ModMetaData(localAbsPath);
				ModLister.mods.Add(modMetaData);
				s = s + "\n  Adding " + modMetaData.ToStringLong();
			}
			s += "\nAdding mods from Steam:";
			foreach (WorkshopItem workshopItem in from it in WorkshopItems.AllSubscribedItems
			where it is WorkshopItem_Mod
			select it)
			{
				ModMetaData modMetaData2 = new ModMetaData(workshopItem);
				ModLister.mods.Add(modMetaData2);
				s = s + "\n  Adding " + modMetaData2.ToStringLong();
			}
			s += "\nDeactivating not-installed mods:";
			ModsConfig.DeactivateNotInstalledMods(delegate(string log)
			{
				s = s + "\n   " + log;
			});
			if (ModLister.mods.Count((ModMetaData m) => m.Active) == 0)
			{
				s += "\nThere are no active mods. Activating Core mod.";
				ModLister.mods.First((ModMetaData m) => m.IsCoreMod).Active = true;
			}
			if (Prefs.LogVerbose)
			{
				Log.Message(s, false);
			}
		}

		public static int InstalledModsListHash(bool activeOnly)
		{
			int num = 17;
			List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			for (int i = 0; i < list.Count<ModMetaData>(); i++)
			{
				if (!activeOnly || list[i].Active)
				{
					num = num * 31 + list[i].GetHashCode();
					num = num * 31 + i * 2654241;
				}
			}
			return num;
		}

		internal static ModMetaData GetModWithIdentifier(string identifier)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].Identifier == identifier)
				{
					return ModLister.mods[i];
				}
			}
			return null;
		}

		public static bool HasActiveModWithName(string name)
		{
			for (int i = 0; i < ModLister.mods.Count; i++)
			{
				if (ModLister.mods[i].Active && ModLister.mods[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}

		[CompilerGenerated]
		private static bool <get_AllActiveModDirs>m__0(ModMetaData mod)
		{
			return mod.Active;
		}

		[CompilerGenerated]
		private static DirectoryInfo <get_AllActiveModDirs>m__1(ModMetaData mod)
		{
			return mod.RootDir;
		}

		[CompilerGenerated]
		private static string <RebuildModList>m__2(DirectoryInfo d)
		{
			return d.FullName;
		}

		[CompilerGenerated]
		private static bool <RebuildModList>m__3(WorkshopItem it)
		{
			return it is WorkshopItem_Mod;
		}

		[CompilerGenerated]
		private static bool <RebuildModList>m__4(ModMetaData m)
		{
			return m.Active;
		}

		[CompilerGenerated]
		private static bool <RebuildModList>m__5(ModMetaData m)
		{
			return m.IsCoreMod;
		}

		[CompilerGenerated]
		private sealed class <RebuildModList>c__AnonStorey0
		{
			internal string s;

			public <RebuildModList>c__AnonStorey0()
			{
			}

			internal void <>m__0(string log)
			{
				this.s = this.s + "\n   " + log;
			}
		}
	}
}
