using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000CC4 RID: 3268
	public static class ModLister
	{
		// Token: 0x040030E2 RID: 12514
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x06004833 RID: 18483 RVA: 0x00260207 File Offset: 0x0025E607
		static ModLister()
		{
			ModLister.RebuildModList();
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004834 RID: 18484 RVA: 0x0026021C File Offset: 0x0025E61C
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004835 RID: 18485 RVA: 0x00260238 File Offset: 0x0025E638
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x00260296 File Offset: 0x0025E696
		internal static void EnsureInit()
		{
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0026029C File Offset: 0x0025E69C
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

		// Token: 0x06004838 RID: 18488 RVA: 0x002604E0 File Offset: 0x0025E8E0
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

		// Token: 0x06004839 RID: 18489 RVA: 0x0026055C File Offset: 0x0025E95C
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

		// Token: 0x0600483A RID: 18490 RVA: 0x002605BC File Offset: 0x0025E9BC
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
