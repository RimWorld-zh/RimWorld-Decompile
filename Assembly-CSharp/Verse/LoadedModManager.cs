using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CBE RID: 3262
	public static class LoadedModManager
	{
		// Token: 0x040030C6 RID: 12486
		private static List<ModContentPack> runningMods = new List<ModContentPack>();

		// Token: 0x040030C7 RID: 12487
		private static Dictionary<Type, Mod> runningModClasses = new Dictionary<Type, Mod>();

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060047F3 RID: 18419 RVA: 0x0025E138 File Offset: 0x0025C538
		public static List<ModContentPack> RunningModsListForReading
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x060047F4 RID: 18420 RVA: 0x0025E154 File Offset: 0x0025C554
		public static IEnumerable<ModContentPack> RunningMods
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x060047F5 RID: 18421 RVA: 0x0025E170 File Offset: 0x0025C570
		public static IEnumerable<Mod> ModHandles
		{
			get
			{
				return LoadedModManager.runningModClasses.Values;
			}
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x0025E190 File Offset: 0x0025C590
		public static void LoadAllActiveMods()
		{
			XmlInheritance.Clear();
			LoadedModManager.InitializeMods();
			LoadedModManager.LoadModContent();
			LoadedModManager.CreateModClasses();
			List<LoadableXmlAsset> xmls = LoadedModManager.LoadModXML();
			Dictionary<XmlNode, LoadableXmlAsset> assetlookup = new Dictionary<XmlNode, LoadableXmlAsset>();
			XmlDocument xmlDoc = LoadedModManager.CombineIntoUnifiedXML(xmls, assetlookup);
			LoadedModManager.ApplyPatches(xmlDoc, assetlookup);
			LoadedModManager.ParseAndProcessXML(xmlDoc, assetlookup);
			LoadedModManager.ClearCachedPatches();
			XmlInheritance.Clear();
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0025E1E0 File Offset: 0x0025C5E0
		public static void InitializeMods()
		{
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
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x0025E2E4 File Offset: 0x0025C6E4
		public static void LoadModContent()
		{
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack + " content");
				modContentPack.ReloadContent();
				DeepProfiler.End();
			}
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x0025E33C File Offset: 0x0025C73C
		public static void CreateModClasses()
		{
			using (IEnumerator<Type> enumerator = typeof(Mod).InstantiableDescendantsAndSelf().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					if (!LoadedModManager.runningModClasses.ContainsKey(type))
					{
						ModContentPack modContentPack = (from modpack in LoadedModManager.runningMods
						where modpack.assemblies.loadedAssemblies.Contains(type.Assembly)
						select modpack).FirstOrDefault<ModContentPack>();
						LoadedModManager.runningModClasses[type] = (Mod)Activator.CreateInstance(type, new object[]
						{
							modContentPack
						});
					}
				}
			}
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x0025E408 File Offset: 0x0025C808
		public static List<LoadableXmlAsset> LoadModXML()
		{
			List<LoadableXmlAsset> list = new List<LoadableXmlAsset>();
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack);
				list.AddRange(modContentPack.LoadDefs());
				DeepProfiler.End();
			}
			return list;
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0025E470 File Offset: 0x0025C870
		public static void ApplyPatches(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			foreach (PatchOperation patchOperation in LoadedModManager.runningMods.SelectMany((ModContentPack rm) => rm.Patches))
			{
				patchOperation.Apply(xmlDoc);
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0025E4F0 File Offset: 0x0025C8F0
		public static XmlDocument CombineIntoUnifiedXML(List<LoadableXmlAsset> xmls, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("Defs"));
			foreach (LoadableXmlAsset loadableXmlAsset in xmls)
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
					IEnumerator enumerator2 = loadableXmlAsset.xmlDoc.DocumentElement.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj = enumerator2.Current;
							XmlNode node = (XmlNode)obj;
							XmlNode xmlNode = xmlDocument.ImportNode(node, true);
							assetlookup[xmlNode] = loadableXmlAsset;
							xmlDocument.DocumentElement.AppendChild(xmlNode);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			return xmlDocument;
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0025E69C File Offset: 0x0025CA9C
		public static void ParseAndProcessXML(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
			for (int i = 0; i < childNodes.Count; i++)
			{
				if (childNodes[i].NodeType == XmlNodeType.Element)
				{
					LoadableXmlAsset loadableXmlAsset = assetlookup.TryGetValue(childNodes[i], null);
					XmlInheritance.TryRegister(childNodes[i], (loadableXmlAsset == null) ? null : loadableXmlAsset.mod);
				}
			}
			XmlInheritance.Resolve();
			DefPackage defPackage = new DefPackage("(unknown)", "(unknown)");
			ModContentPack modContentPack = LoadedModManager.runningMods.FirstOrDefault<ModContentPack>();
			modContentPack.AddDefPackage(defPackage);
			IEnumerator enumerator = xmlDoc.DocumentElement.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode = (XmlNode)obj;
					LoadableXmlAsset loadableXmlAsset2 = assetlookup.TryGetValue(xmlNode, null);
					DefPackage defPackage2 = (loadableXmlAsset2 == null) ? defPackage : loadableXmlAsset2.defPackage;
					Def def = DirectXmlLoader.DefFromNode(xmlNode, loadableXmlAsset2);
					if (def != null)
					{
						def.modContentPack = ((loadableXmlAsset2 == null) ? modContentPack : loadableXmlAsset2.mod);
						defPackage2.AddDef(def);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x0025E7F4 File Offset: 0x0025CBF4
		public static void ClearCachedPatches()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation in modContentPack.Patches)
				{
					patchOperation.Complete(modContentPack.Name);
				}
				modContentPack.ClearPatchesCache();
			}
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x0025E8A4 File Offset: 0x0025CCA4
		public static void ClearDestroy()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				modContentPack.ClearDestroy();
			}
			LoadedModManager.runningMods.Clear();
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x0025E90C File Offset: 0x0025CD0C
		public static T GetMod<T>() where T : Mod
		{
			return LoadedModManager.GetMod(typeof(T)) as T;
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0025E93C File Offset: 0x0025CD3C
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

		// Token: 0x06004802 RID: 18434 RVA: 0x0025E9AC File Offset: 0x0025CDAC
		private static string GetSettingsFilename(string modIdentifier, string modHandleName)
		{
			return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename(string.Format("Mod_{0}_{1}.xml", modIdentifier, modHandleName)));
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0025E9DC File Offset: 0x0025CDDC
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

		// Token: 0x06004804 RID: 18436 RVA: 0x0025EA84 File Offset: 0x0025CE84
		public static void WriteModSettings(string modIdentifier, string modHandleName, ModSettings settings)
		{
			Scribe.saver.InitSaving(LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName), "SettingsBlock");
			Scribe_Deep.Look<ModSettings>(ref settings, "ModSettings", new object[0]);
			Scribe.saver.FinalizeSaving();
		}
	}
}
