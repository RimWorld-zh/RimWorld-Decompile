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
		// Token: 0x06004858 RID: 18520 RVA: 0x0025FADC File Offset: 0x0025DEDC
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

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004859 RID: 18521 RVA: 0x0025FBA8 File Offset: 0x0025DFA8
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

		// Token: 0x0600485A RID: 18522 RVA: 0x0025FBCC File Offset: 0x0025DFCC
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

		// Token: 0x0600485B RID: 18523 RVA: 0x0025FC75 File Offset: 0x0025E075
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add(ModContentPack.CoreModIdentifier);
			ModsConfig.Save();
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x0025FCA0 File Offset: 0x0025E0A0
		internal static void Reorder(int modIndex, int newIndex)
		{
			if (modIndex != newIndex)
			{
				ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
				ModsConfig.data.activeMods.RemoveAt((modIndex >= newIndex) ? (modIndex + 1) : modIndex);
			}
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x0025FCFC File Offset: 0x0025E0FC
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.data.activeMods.Contains(mod.Identifier);
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x0025FD26 File Offset: 0x0025E126
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.Identifier, active);
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x0025FD38 File Offset: 0x0025E138
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

		// Token: 0x06004860 RID: 18528 RVA: 0x0025FDA0 File Offset: 0x0025E1A0
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod) != null
			select mod).ToList<string>();
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0025FDD5 File Offset: 0x0025E1D5
		public static void Save()
		{
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0025FDE8 File Offset: 0x0025E1E8
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		// Token: 0x040030EC RID: 12524
		private static ModsConfig.ModsConfigData data;

		// Token: 0x02000CCC RID: 3276
		private class ModsConfigData
		{
			// Token: 0x040030EF RID: 12527
			public int buildNumber = -1;

			// Token: 0x040030F0 RID: 12528
			public List<string> activeMods = new List<string>();
		}
	}
}
