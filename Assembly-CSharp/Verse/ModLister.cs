using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000CC7 RID: 3271
	public static class ModLister
	{
		// Token: 0x06004822 RID: 18466 RVA: 0x0025EDEF File Offset: 0x0025D1EF
		static ModLister()
		{
			ModLister.RebuildModList();
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004823 RID: 18467 RVA: 0x0025EE04 File Offset: 0x0025D204
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004824 RID: 18468 RVA: 0x0025EE20 File Offset: 0x0025D220
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0025EE7E File Offset: 0x0025D27E
		internal static void EnsureInit()
		{
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x0025EE84 File Offset: 0x0025D284
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

		// Token: 0x06004827 RID: 18471 RVA: 0x0025F0C8 File Offset: 0x0025D4C8
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

		// Token: 0x06004828 RID: 18472 RVA: 0x0025F144 File Offset: 0x0025D544
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

		// Token: 0x06004829 RID: 18473 RVA: 0x0025F1A4 File Offset: 0x0025D5A4
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

		// Token: 0x040030D7 RID: 12503
		private static List<ModMetaData> mods = new List<ModMetaData>();
	}
}
