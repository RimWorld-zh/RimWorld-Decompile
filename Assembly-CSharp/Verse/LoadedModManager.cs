using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	public static class LoadedModManager
	{
		private static List<ModContentPack> runningMods = new List<ModContentPack>();

		private static Dictionary<Type, Mod> runningModClasses = new Dictionary<Type, Mod>();

		public static List<ModContentPack> RunningModsListForReading
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		public static IEnumerable<ModContentPack> RunningMods
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		public static IEnumerable<Mod> ModHandles
		{
			get
			{
				return LoadedModManager.runningModClasses.Values;
			}
		}

		public static void LoadAllActiveMods()
		{
			XmlInheritance.Clear();
			int num = 0;
			foreach (ModMetaData item2 in ModsConfig.ActiveModsInLoadOrder.ToList())
			{
				DeepProfiler.Start("Initializing " + item2);
				if (!item2.RootDir.Exists)
				{
					ModsConfig.SetActive(item2.Identifier, false);
					Log.Warning("Failed to find active mod " + item2.Name + "(" + item2.Identifier + ") at " + item2.RootDir);
					DeepProfiler.End();
				}
				else
				{
					ModContentPack item = new ModContentPack(item2.RootDir, num, item2.Name);
					num++;
					LoadedModManager.runningMods.Add(item);
					DeepProfiler.End();
				}
			}
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack + " content");
				modContentPack.ReloadContent();
				DeepProfiler.End();
			}
			foreach (Type item3 in typeof(Mod).InstantiableDescendantsAndSelf())
			{
				if (!LoadedModManager.runningModClasses.ContainsKey(item3))
				{
					ModContentPack modContentPack2 = (from modpack in LoadedModManager.runningMods
					where modpack.assemblies.loadedAssemblies.Contains(item3.Assembly)
					select modpack).FirstOrDefault();
					LoadedModManager.runningModClasses[item3] = (Mod)Activator.CreateInstance(item3, modContentPack2);
				}
			}
			for (int j = 0; j < LoadedModManager.runningMods.Count; j++)
			{
				ModContentPack modContentPack3 = LoadedModManager.runningMods[j];
				DeepProfiler.Start("Loading " + modContentPack3);
				modContentPack3.LoadDefs(LoadedModManager.runningMods.SelectMany((Func<ModContentPack, IEnumerable<PatchOperation>>)((ModContentPack rm) => rm.Patches)));
				DeepProfiler.End();
			}
			foreach (ModContentPack runningMod in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patch in runningMod.Patches)
				{
					patch.Complete(runningMod.Name);
				}
				runningMod.ClearPatchesCache();
			}
			XmlInheritance.Clear();
		}

		public static void ClearDestroy()
		{
			foreach (ModContentPack runningMod in LoadedModManager.runningMods)
			{
				runningMod.ClearDestroy();
			}
			LoadedModManager.runningMods.Clear();
		}

		public static T GetMod<T>() where T : Mod
		{
			return (T)(LoadedModManager.GetMod(typeof(T)) as T);
		}

		public static Mod GetMod(Type type)
		{
			return (!LoadedModManager.runningModClasses.ContainsKey(type)) ? (from kvp in LoadedModManager.runningModClasses
			where type.IsAssignableFrom(kvp.Key)
			select kvp).FirstOrDefault().Value : LoadedModManager.runningModClasses[type];
		}

		private static string GetSettingsFilename(string modIdentifier, string modHandleName)
		{
			return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename(string.Format("Mod_{0}_{1}.xml", modIdentifier, modHandleName)));
		}

		public static T ReadModSettings<T>(string modIdentifier, string modHandleName) where T : ModSettings, new()
		{
			string settingsFilename = LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName);
			T val = (T)null;
			try
			{
				if (File.Exists(settingsFilename))
				{
					Scribe.loader.InitLoading(settingsFilename);
					Scribe_Deep.Look<T>(ref val, "ModSettings", new object[0]);
					Scribe.loader.FinalizeLoading();
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Caught exception while loading mod settings data for {0}. Generating fresh settings. The exception was: {1}", modIdentifier, ex.ToString()));
				val = (T)null;
			}
			if (val == null)
			{
				val = new T();
			}
			return val;
		}

		public static void WriteModSettings(string modIdentifier, string modHandleName, ModSettings settings)
		{
			Scribe.saver.InitSaving(LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName), "SettingsBlock");
			Scribe_Deep.Look(ref settings, "ModSettings");
			Scribe.saver.FinalizeSaving();
		}
	}
}
