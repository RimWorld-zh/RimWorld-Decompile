using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CBF RID: 3263
	public static class LoadedModManager
	{
		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060047E7 RID: 18407 RVA: 0x0025CC6C File Offset: 0x0025B06C
		public static List<ModContentPack> RunningModsListForReading
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060047E8 RID: 18408 RVA: 0x0025CC88 File Offset: 0x0025B088
		public static IEnumerable<ModContentPack> RunningMods
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x060047E9 RID: 18409 RVA: 0x0025CCA4 File Offset: 0x0025B0A4
		public static IEnumerable<Mod> ModHandles
		{
			get
			{
				return LoadedModManager.runningModClasses.Values;
			}
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x0025CCC4 File Offset: 0x0025B0C4
		public static void LoadAllActiveMods()
		{
			XmlInheritance.Clear();
			int num = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>())
			{
				DeepProfiler.Start("Initializing " + modMetaData);
				if (!modMetaData.RootDir.Exists)
				{
					ModsConfig.SetActive(modMetaData.Identifier, false);
					Log.Warning(string.Concat(new object[]
					{
						"Failed to find active mod ",
						modMetaData.Name,
						"(",
						modMetaData.Identifier,
						") at ",
						modMetaData.RootDir
					}), false);
					DeepProfiler.End();
				}
				else
				{
					ModContentPack item = new ModContentPack(modMetaData.RootDir, num, modMetaData.Name);
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
			using (IEnumerator<Type> enumerator2 = typeof(Mod).InstantiableDescendantsAndSelf().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Type type = enumerator2.Current;
					if (!LoadedModManager.runningModClasses.ContainsKey(type))
					{
						ModContentPack modContentPack2 = (from modpack in LoadedModManager.runningMods
						where modpack.assemblies.loadedAssemblies.Contains(type.Assembly)
						select modpack).FirstOrDefault<ModContentPack>();
						LoadedModManager.runningModClasses[type] = (Mod)Activator.CreateInstance(type, new object[]
						{
							modContentPack2
						});
					}
				}
			}
			List<LoadableXmlAsset> list = new List<LoadableXmlAsset>();
			for (int j = 0; j < LoadedModManager.runningMods.Count; j++)
			{
				ModContentPack modContentPack3 = LoadedModManager.runningMods[j];
				DeepProfiler.Start("Loading " + modContentPack3);
				list.AddRange(modContentPack3.LoadDefs());
				DeepProfiler.End();
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("Defs"));
			Dictionary<XmlNode, LoadableXmlAsset> dictionary = new Dictionary<XmlNode, LoadableXmlAsset>();
			foreach (LoadableXmlAsset loadableXmlAsset in list)
			{
				if (loadableXmlAsset.xmlDoc == null || loadableXmlAsset.xmlDoc.DocumentElement == null)
				{
					Log.Error(string.Format("{0}: unknown parse failure", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name), false);
				}
				else
				{
					if (loadableXmlAsset.xmlDoc.DocumentElement.Name != "Defs")
					{
						Log.Error(string.Format("{0}: root element named {1}; should be named Defs", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name, loadableXmlAsset.xmlDoc.DocumentElement.Name), false);
					}
					IEnumerator enumerator4 = loadableXmlAsset.xmlDoc.DocumentElement.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							object obj = enumerator4.Current;
							XmlNode node = (XmlNode)obj;
							XmlNode xmlNode = xmlDocument.ImportNode(node, true);
							dictionary[xmlNode] = loadableXmlAsset;
							xmlDocument.DocumentElement.AppendChild(xmlNode);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator4 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			foreach (PatchOperation patchOperation in LoadedModManager.runningMods.SelectMany((ModContentPack rm) => rm.Patches))
			{
				patchOperation.Apply(xmlDocument);
			}
			XmlNodeList childNodes = xmlDocument.DocumentElement.ChildNodes;
			for (int k = 0; k < childNodes.Count; k++)
			{
				if (childNodes[k].NodeType == XmlNodeType.Element)
				{
					LoadableXmlAsset loadableXmlAsset2 = dictionary.TryGetValue(childNodes[k], null);
					XmlInheritance.TryRegister(childNodes[k], (loadableXmlAsset2 == null) ? null : loadableXmlAsset2.mod);
				}
			}
			XmlInheritance.Resolve();
			DefPackage defPackage = new DefPackage("(unknown)", "(unknown)");
			ModContentPack modContentPack4 = LoadedModManager.runningMods.FirstOrDefault<ModContentPack>();
			modContentPack4.AddDefPackage(defPackage);
			IEnumerator enumerator6 = xmlDocument.DocumentElement.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator6.MoveNext())
				{
					object obj2 = enumerator6.Current;
					XmlNode xmlNode2 = (XmlNode)obj2;
					LoadableXmlAsset loadableXmlAsset3 = dictionary.TryGetValue(xmlNode2, null);
					DefPackage defPackage2 = (loadableXmlAsset3 == null) ? defPackage : loadableXmlAsset3.defPackage;
					Def def = DirectXmlLoader.DefFromNode(xmlNode2, loadableXmlAsset3);
					if (def != null)
					{
						def.modContentPack = ((loadableXmlAsset3 == null) ? modContentPack4 : loadableXmlAsset3.mod);
						defPackage2.AddDef(def);
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator6 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			foreach (ModContentPack modContentPack5 in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation2 in modContentPack5.Patches)
				{
					patchOperation2.Complete(modContentPack5.Name);
				}
				modContentPack5.ClearPatchesCache();
			}
			XmlInheritance.Clear();
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x0025D3B0 File Offset: 0x0025B7B0
		public static void ClearDestroy()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				modContentPack.ClearDestroy();
			}
			LoadedModManager.runningMods.Clear();
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x0025D418 File Offset: 0x0025B818
		public static T GetMod<T>() where T : Mod
		{
			return LoadedModManager.GetMod(typeof(T)) as T;
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x0025D448 File Offset: 0x0025B848
		public static Mod GetMod(Type type)
		{
			Mod result;
			if (LoadedModManager.runningModClasses.ContainsKey(type))
			{
				result = LoadedModManager.runningModClasses[type];
			}
			else
			{
				result = (from kvp in LoadedModManager.runningModClasses
				where type.IsAssignableFrom(kvp.Key)
				select kvp).FirstOrDefault<KeyValuePair<Type, Mod>>().Value;
			}
			return result;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0025D4B8 File Offset: 0x0025B8B8
		private static string GetSettingsFilename(string modIdentifier, string modHandleName)
		{
			return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename(string.Format("Mod_{0}_{1}.xml", modIdentifier, modHandleName)));
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0025D4E8 File Offset: 0x0025B8E8
		public static T ReadModSettings<T>(string modIdentifier, string modHandleName) where T : ModSettings, new()
		{
			string settingsFilename = LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName);
			T t = (T)((object)null);
			try
			{
				if (File.Exists(settingsFilename))
				{
					Scribe.loader.InitLoading(settingsFilename);
					Scribe_Deep.Look<T>(ref t, "ModSettings", new object[0]);
					Scribe.loader.FinalizeLoading();
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Caught exception while loading mod settings data for {0}. Generating fresh settings. The exception was: {1}", modIdentifier, ex.ToString()), false);
				t = (T)((object)null);
			}
			if (t == null)
			{
				t = Activator.CreateInstance<T>();
			}
			return t;
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0025D590 File Offset: 0x0025B990
		public static void WriteModSettings(string modIdentifier, string modHandleName, ModSettings settings)
		{
			Scribe.saver.InitSaving(LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName), "SettingsBlock");
			Scribe_Deep.Look<ModSettings>(ref settings, "ModSettings", new object[0]);
			Scribe.saver.FinalizeSaving();
		}

		// Token: 0x040030BB RID: 12475
		private static List<ModContentPack> runningMods = new List<ModContentPack>();

		// Token: 0x040030BC RID: 12476
		private static Dictionary<Type, Mod> runningModClasses = new Dictionary<Type, Mod>();
	}
}
