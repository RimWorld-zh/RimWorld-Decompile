using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CC8 RID: 3272
	public static class ModsConfig
	{
		// Token: 0x06004869 RID: 18537 RVA: 0x00260EF4 File Offset: 0x0025F2F4
		static ModsConfig()
		{
			bool flag = false;
			ModsConfig.data = DirectXmlLoader.ItemFromXmlFile<ModsConfig.ModsConfigData>(GenFilePaths.ModsConfigFilePath, true);
			if (ModsConfig.data.buildNumber < VersionControl.CurrentBuild)
			{
				Log.Message(string.Concat(new object[]
				{
					"Mods config data is from build ",
					ModsConfig.data.buildNumber,
					" while we are at build ",
					VersionControl.CurrentBuild,
					". Resetting."
				}), false);
				ModsConfig.data = new ModsConfig.ModsConfigData();
				flag = true;
			}
			ModsConfig.data.buildNumber = VersionControl.CurrentBuild;
			bool flag2 = File.Exists(GenFilePaths.ModsConfigFilePath);
			if (!flag2 || flag)
			{
				ModsConfig.data.activeMods.Add(ModContentPack.CoreModIdentifier);
				ModsConfig.Save();
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600486A RID: 18538 RVA: 0x00260FC0 File Offset: 0x0025F3C0
		public static IEnumerable<ModMetaData> ActiveModsInLoadOrder
		{
			get
			{
				ModLister.EnsureInit();
				for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
				{
					yield return ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i]);
				}
				yield break;
			}
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x00260FE4 File Offset: 0x0025F3E4
		public static void DeactivateNotInstalledMods(Action<string> logCallback = null)
		{
			int i;
			for (i = ModsConfig.data.activeMods.Count - 1; i >= 0; i--)
			{
				if (!ModLister.AllInstalledMods.Any((ModMetaData m) => m.Identifier == ModsConfig.data.activeMods[i]))
				{
					if (logCallback != null)
					{
						logCallback("Deactivating " + ModsConfig.data.activeMods[i]);
					}
					ModsConfig.data.activeMods.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x0026108D File Offset: 0x0025F48D
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add(ModContentPack.CoreModIdentifier);
			ModsConfig.Save();
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x002610B8 File Offset: 0x0025F4B8
		internal static void Reorder(int modIndex, int newIndex)
		{
			if (modIndex != newIndex)
			{
				ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
				ModsConfig.data.activeMods.RemoveAt((modIndex >= newIndex) ? (modIndex + 1) : modIndex);
			}
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x00261114 File Offset: 0x0025F514
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.data.activeMods.Contains(mod.Identifier);
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x0026113E File Offset: 0x0025F53E
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.Identifier, active);
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x00261150 File Offset: 0x0025F550
		public static void SetActive(string modIdentifier, bool active)
		{
			if (active)
			{
				if (!ModsConfig.data.activeMods.Contains(modIdentifier))
				{
					ModsConfig.data.activeMods.Add(modIdentifier);
				}
			}
			else if (ModsConfig.data.activeMods.Contains(modIdentifier))
			{
				ModsConfig.data.activeMods.Remove(modIdentifier);
			}
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x002611B8 File Offset: 0x0025F5B8
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod) != null
			select mod).ToList<string>();
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x002611ED File Offset: 0x0025F5ED
		public static void Save()
		{
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x00261200 File Offset: 0x0025F600
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		// Token: 0x040030F7 RID: 12535
		private static ModsConfig.ModsConfigData data;

		// Token: 0x02000CC9 RID: 3273
		private class ModsConfigData
		{
			// Token: 0x040030FA RID: 12538
			public int buildNumber = -1;

			// Token: 0x040030FB RID: 12539
			public List<string> activeMods = new List<string>();
		}
	}
}
