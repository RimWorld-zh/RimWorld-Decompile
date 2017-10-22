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
			List<ModMetaData>.Enumerator enumerator = ModsConfig.ActiveModsInLoadOrder.ToList().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ModMetaData current = enumerator.Current;
					DeepProfiler.Start("Initializing " + current);
					if (!current.RootDir.Exists)
					{
						ModsConfig.SetActive(current.Identifier, false);
						Log.Warning("Failed to find active mod " + current.Name + "(" + current.Identifier + ") at " + current.RootDir);
						DeepProfiler.End();
					}
					else
					{
						ModContentPack item = new ModContentPack(current.RootDir, num, current.Name);
						num++;
						LoadedModManager.runningMods.Add(item);
						DeepProfiler.End();
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack + " content");
				modContentPack.ReloadContent();
				DeepProfiler.End();
			}
			for (int j = 0; j < LoadedModManager.runningMods.Count; j++)
			{
				ModContentPack modContentPack2 = LoadedModManager.runningMods[j];
				DeepProfiler.Start("Loading " + modContentPack2);
				modContentPack2.LoadDefs(LoadedModManager.runningMods.SelectMany((Func<ModContentPack, IEnumerable<PatchOperation>>)((ModContentPack rm) => rm.Patches)));
				DeepProfiler.End();
			}
			List<ModContentPack>.Enumerator enumerator2 = LoadedModManager.runningMods.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					ModContentPack current2 = enumerator2.Current;
					foreach (PatchOperation patch in current2.Patches)
					{
						patch.Complete(current2.Name);
					}
					current2.ClearPatchesCache();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			using (IEnumerator<Type> enumerator4 = typeof(Mod).InstantiableDescendantsAndSelf().GetEnumerator())
			{
				Type type;
				while (enumerator4.MoveNext())
				{
					type = enumerator4.Current;
					if (!LoadedModManager.runningModClasses.ContainsKey(type))
					{
						ModContentPack modContentPack3 = (from modpack in LoadedModManager.runningMods
						where modpack.assemblies.loadedAssemblies.Contains(type.Assembly)
						select modpack).FirstOrDefault();
						LoadedModManager.runningModClasses[type] = (Mod)Activator.CreateInstance(type, modContentPack3);
					}
				}
			}
			XmlInheritance.Clear();
		}

		public static void ClearDestroy()
		{
			List<ModContentPack>.Enumerator enumerator = LoadedModManager.runningMods.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ModContentPack current = enumerator.Current;
					current.ClearDestroy();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			LoadedModManager.runningMods.Clear();
		}

		public static T GetMod<T>() where T : Mod
		{
			return (T)(LoadedModManager.GetMod(typeof(T)) as T);
		}

		public static Mod GetMod(Type type)
		{
			if (LoadedModManager.runningModClasses.ContainsKey(type))
			{
				return LoadedModManager.runningModClasses[type];
			}
			return (from kvp in LoadedModManager.runningModClasses
			where type.IsAssignableFrom(kvp.Key)
			select kvp).FirstOrDefault().Value;
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
