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
		// Token: 0x040030E9 RID: 12521
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x06004836 RID: 18486 RVA: 0x002605C3 File Offset: 0x0025E9C3
		static ModLister()
		{
			ModLister.RebuildModList();
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004837 RID: 18487 RVA: 0x002605D8 File Offset: 0x0025E9D8
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004838 RID: 18488 RVA: 0x002605F4 File Offset: 0x0025E9F4
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00260652 File Offset: 0x0025EA52
		internal static void EnsureInit()
		{
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00260658 File Offset: 0x0025EA58
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

		// Token: 0x0600483B RID: 18491 RVA: 0x0026089C File Offset: 0x0025EC9C
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

		// Token: 0x0600483C RID: 18492 RVA: 0x00260918 File Offset: 0x0025ED18
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

		// Token: 0x0600483D RID: 18493 RVA: 0x00260978 File Offset: 0x0025ED78
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
	}
}
