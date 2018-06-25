using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CCB RID: 3275
	public static class ModsConfig
	{
		// Token: 0x040030FE RID: 12542
		private static ModsConfig.ModsConfigData data;

		// Token: 0x0600486C RID: 18540 RVA: 0x002612B0 File Offset: 0x0025F6B0
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

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600486D RID: 18541 RVA: 0x0026137C File Offset: 0x0025F77C
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

		// Token: 0x0600486E RID: 18542 RVA: 0x002613A0 File Offset: 0x0025F7A0
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

		// Token: 0x0600486F RID: 18543 RVA: 0x00261449 File Offset: 0x0025F849
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add(ModContentPack.CoreModIdentifier);
			ModsConfig.Save();
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x00261474 File Offset: 0x0025F874
		internal static void Reorder(int modIndex, int newIndex)
		{
			if (modIndex != newIndex)
			{
				ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
				ModsConfig.data.activeMods.RemoveAt((modIndex >= newIndex) ? (modIndex + 1) : modIndex);
			}
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x002614D0 File Offset: 0x0025F8D0
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.data.activeMods.Contains(mod.Identifier);
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x002614FA File Offset: 0x0025F8FA
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.Identifier, active);
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x0026150C File Offset: 0x0025F90C
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

		// Token: 0x06004874 RID: 18548 RVA: 0x00261574 File Offset: 0x0025F974
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod) != null
			select mod).ToList<string>();
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x002615A9 File Offset: 0x0025F9A9
		public static void Save()
		{
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x002615BC File Offset: 0x0025F9BC
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		// Token: 0x02000CCC RID: 3276
		private class ModsConfigData
		{
			// Token: 0x04003101 RID: 12545
			public int buildNumber = -1;

			// Token: 0x04003102 RID: 12546
			public List<string> activeMods = new List<string>();
		}
	}
}
