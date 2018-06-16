using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000CC8 RID: 3272
	public static class ModLister
	{
		// Token: 0x06004824 RID: 18468 RVA: 0x0025EE17 File Offset: 0x0025D217
		static ModLister()
		{
			ModLister.RebuildModList();
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004825 RID: 18469 RVA: 0x0025EE2C File Offset: 0x0025D22C
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004826 RID: 18470 RVA: 0x0025EE48 File Offset: 0x0025D248
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x0025EEA6 File Offset: 0x0025D2A6
		internal static void EnsureInit()
		{
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x0025EEAC File Offset: 0x0025D2AC
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

		// Token: 0x06004829 RID: 18473 RVA: 0x0025F0F0 File Offset: 0x0025D4F0
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

		// Token: 0x0600482A RID: 18474 RVA: 0x0025F16C File Offset: 0x0025D56C
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

		// Token: 0x0600482B RID: 18475 RVA: 0x0025F1CC File Offset: 0x0025D5CC
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

		// Token: 0x040030D9 RID: 12505
		private static List<ModMetaData> mods = new List<ModMetaData>();
	}
}
