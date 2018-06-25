using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CC5 RID: 3269
	public class ModContentPack
	{
		// Token: 0x040030D6 RID: 12502
		private DirectoryInfo rootDirInt;

		// Token: 0x040030D7 RID: 12503
		public int loadOrder;

		// Token: 0x040030D8 RID: 12504
		private string nameInt;

		// Token: 0x040030D9 RID: 12505
		private ModContentHolder<AudioClip> audioClips;

		// Token: 0x040030DA RID: 12506
		private ModContentHolder<Texture2D> textures;

		// Token: 0x040030DB RID: 12507
		private ModContentHolder<string> strings;

		// Token: 0x040030DC RID: 12508
		public ModAssemblyHandler assemblies;

		// Token: 0x040030DD RID: 12509
		private List<PatchOperation> patches;

		// Token: 0x040030DE RID: 12510
		private List<DefPackage> defPackages = new List<DefPackage>();

		// Token: 0x040030DF RID: 12511
		private DefPackage impliedDefPackage;

		// Token: 0x040030E0 RID: 12512
		public static readonly string CoreModIdentifier = "Core";

		// Token: 0x06004820 RID: 18464 RVA: 0x0025FB24 File Offset: 0x0025DF24
		public ModContentPack(DirectoryInfo directory, int loadOrder, string name)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assemblies = new ModAssemblyHandler(this);
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06004821 RID: 18465 RVA: 0x0025FB88 File Offset: 0x0025DF88
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06004822 RID: 18466 RVA: 0x0025FBA8 File Offset: 0x0025DFA8
		public string Identifier
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004823 RID: 18467 RVA: 0x0025FBC8 File Offset: 0x0025DFC8
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004824 RID: 18468 RVA: 0x0025FBE4 File Offset: 0x0025DFE4
		public int OverwritePriority
		{
			get
			{
				return (!this.IsCoreMod) ? 1 : 0;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004825 RID: 18469 RVA: 0x0025FC0C File Offset: 0x0025E00C
		public bool IsCoreMod
		{
			get
			{
				return this.rootDirInt.Name == ModContentPack.CoreModIdentifier;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004826 RID: 18470 RVA: 0x0025FC38 File Offset: 0x0025E038
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defPackages.SelectMany((DefPackage x) => x.defs);
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004827 RID: 18471 RVA: 0x0025FC78 File Offset: 0x0025E078
		public bool LoadedAnyAssembly
		{
			get
			{
				return this.assemblies.loadedAssemblies.Count > 0;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004828 RID: 18472 RVA: 0x0025FCA0 File Offset: 0x0025E0A0
		public IEnumerable<PatchOperation> Patches
		{
			get
			{
				if (this.patches == null)
				{
					this.LoadPatches();
				}
				return this.patches;
			}
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0025FCCC File Offset: 0x0025E0CC
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0025FCE8 File Offset: 0x0025E0E8
		public ModContentHolder<T> GetContentHolder<T>() where T : class
		{
			ModContentHolder<T> result;
			if (typeof(T) == typeof(Texture2D))
			{
				result = (ModContentHolder<T>)this.textures;
			}
			else if (typeof(T) == typeof(AudioClip))
			{
				result = (ModContentHolder<T>)this.audioClips;
			}
			else if (typeof(T) == typeof(string))
			{
				result = (ModContentHolder<T>)this.strings;
			}
			else
			{
				Log.Error("Mod lacks manager for asset type " + this.strings, false);
				result = null;
			}
			return result;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x0025FD92 File Offset: 0x0025E192
		public void ReloadContent()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.audioClips.ReloadAll();
				this.textures.ReloadAll();
				this.strings.ReloadAll();
			});
			this.assemblies.ReloadAll();
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0025FDB4 File Offset: 0x0025E1B4
		public IEnumerable<LoadableXmlAsset> LoadDefs()
		{
			if (this.defPackages.Count != 0)
			{
				Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405, false);
			}
			foreach (LoadableXmlAsset asset in DirectXmlLoader.XmlAssetsInModFolder(this, "Defs/"))
			{
				DefPackage defPackage = new DefPackage(asset.name, GenFilePaths.FolderPathRelativeToDefsFolder(asset.fullFolderPath, this));
				this.AddDefPackage(defPackage);
				asset.defPackage = defPackage;
				yield return asset;
			}
			yield break;
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0025FDE0 File Offset: 0x0025E1E0
		public IEnumerable<DefPackage> GetDefPackagesInFolder(string relFolder)
		{
			string path = Path.Combine(Path.Combine(this.RootDir, "Defs/"), relFolder);
			IEnumerable<DefPackage> result;
			if (!Directory.Exists(path))
			{
				result = Enumerable.Empty<DefPackage>();
			}
			else
			{
				string fullPath = Path.GetFullPath(path);
				result = from x in this.defPackages
				where x.GetFullFolderPath(this).StartsWith(fullPath)
				select x;
			}
			return result;
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x0025FE52 File Offset: 0x0025E252
		public void AddDefPackage(DefPackage defPackage)
		{
			this.defPackages.Add(defPackage);
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x0025FE64 File Offset: 0x0025E264
		private void LoadPatches()
		{
			DeepProfiler.Start("Loading all patches");
			this.patches = new List<PatchOperation>();
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/").ToList<LoadableXmlAsset>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlElement documentElement = list[i].xmlDoc.DocumentElement;
				if (documentElement.Name != "Patch")
				{
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name), false);
				}
				else
				{
					for (int j = 0; j < documentElement.ChildNodes.Count; j++)
					{
						XmlNode xmlNode = documentElement.ChildNodes[j];
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							if (xmlNode.Name != "Operation")
							{
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", documentElement.ChildNodes[j].Name), false);
							}
							else
							{
								PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(xmlNode, false);
								patchOperation.sourceFile = list[i].FullFilePath;
								this.patches.Add(patchOperation);
							}
						}
					}
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0025FF9E File Offset: 0x0025E39E
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x0025FFA8 File Offset: 0x0025E3A8
		public void AddImpliedDef(Def def)
		{
			if (this.impliedDefPackage == null)
			{
				this.impliedDefPackage = new DefPackage("ImpliedDefs", "");
				this.defPackages.Add(this.impliedDefPackage);
			}
			this.impliedDefPackage.AddDef(def);
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x0025FFF8 File Offset: 0x0025E3F8
		public override string ToString()
		{
			return this.Identifier;
		}
	}
}
