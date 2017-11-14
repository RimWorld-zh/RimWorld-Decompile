using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	public static class ModsConfig
	{
		private class ModsConfigData
		{
			public int buildNumber = -1;

			public List<string> activeMods = new List<string>();
		}

		private static ModsConfigData data;

		public static IEnumerable<ModMetaData> ActiveModsInLoadOrder
		{
			get
			{
				ModLister.EnsureInit();
				int i = 0;
				if (i < ModsConfig.data.activeMods.Count)
				{
					yield return ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i]);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		static ModsConfig()
		{
			bool flag = false;
			ModsConfig.data = DirectXmlLoader.ItemFromXmlFile<ModsConfigData>(GenFilePaths.ModsConfigFilePath, true);
			if (ModsConfig.data.buildNumber < VersionControl.CurrentBuild)
			{
				Log.Message("Mods config data is from build " + ModsConfig.data.buildNumber + " while we are at build " + VersionControl.CurrentBuild + ". Resetting.");
				ModsConfig.data = new ModsConfigData();
				flag = true;
			}
			ModsConfig.data.buildNumber = VersionControl.CurrentBuild;
			if (File.Exists(GenFilePaths.ModsConfigFilePath) && !flag)
				return;
			ModsConfig.data.activeMods.Add(ModContentPack.CoreModIdentifier);
			ModsConfig.Save();
		}

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

		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add(ModContentPack.CoreModIdentifier);
			ModsConfig.Save();
		}

		internal static void Reorder(int modIndex, int newIndex)
		{
			if (modIndex != newIndex)
			{
				string item = ModsConfig.data.activeMods[modIndex];
				ModsConfig.data.activeMods.RemoveAt(modIndex);
				ModsConfig.data.activeMods.Insert(newIndex, item);
			}
		}

		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.data.activeMods.Contains(mod.Identifier);
		}

		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.Identifier, active);
		}

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

		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod) != null
			select mod).ToList();
		}

		public static void Save()
		{
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate
			{
				GenCommandLine.Restart();
			}, null, null, null, false));
		}
	}
}
