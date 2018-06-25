using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using UnityEngine;

namespace Verse
{
	public class ModContentPack
	{
		private DirectoryInfo rootDirInt;

		public int loadOrder;

		private string nameInt;

		private ModContentHolder<AudioClip> audioClips;

		private ModContentHolder<Texture2D> textures;

		private ModContentHolder<string> strings;

		public ModAssemblyHandler assemblies;

		private List<PatchOperation> patches;

		private List<DefPackage> defPackages = new List<DefPackage>();

		private DefPackage impliedDefPackage;

		public static readonly string CoreModIdentifier = "Core";

		[CompilerGenerated]
		private static Func<DefPackage, IEnumerable<Def>> <>f__am$cache0;

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

		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		public string Identifier
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		public int OverwritePriority
		{
			get
			{
				return (!this.IsCoreMod) ? 1 : 0;
			}
		}

		public bool IsCoreMod
		{
			get
			{
				return this.rootDirInt.Name == ModContentPack.CoreModIdentifier;
			}
		}

		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defPackages.SelectMany((DefPackage x) => x.defs);
			}
		}

		public bool LoadedAnyAssembly
		{
			get
			{
				return this.assemblies.loadedAssemblies.Count > 0;
			}
		}

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

		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
		}

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

		public void AddDefPackage(DefPackage defPackage)
		{
			this.defPackages.Add(defPackage);
		}

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

		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		public void AddImpliedDef(Def def)
		{
			if (this.impliedDefPackage == null)
			{
				this.impliedDefPackage = new DefPackage("ImpliedDefs", "");
				this.defPackages.Add(this.impliedDefPackage);
			}
			this.impliedDefPackage.AddDef(def);
		}

		public override string ToString()
		{
			return this.Identifier;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ModContentPack()
		{
		}

		[CompilerGenerated]
		private static IEnumerable<Def> <get_AllDefs>m__0(DefPackage x)
		{
			return x.defs;
		}

		[CompilerGenerated]
		private void <ReloadContent>m__1()
		{
			this.audioClips.ReloadAll();
			this.textures.ReloadAll();
			this.strings.ReloadAll();
		}

		[CompilerGenerated]
		private sealed class <LoadDefs>c__Iterator0 : IEnumerable, IEnumerable<LoadableXmlAsset>, IEnumerator, IDisposable, IEnumerator<LoadableXmlAsset>
		{
			internal IEnumerator<LoadableXmlAsset> $locvar0;

			internal LoadableXmlAsset <asset>__1;

			internal DefPackage <defPackage>__2;

			internal ModContentPack $this;

			internal LoadableXmlAsset $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <LoadDefs>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (this.defPackages.Count != 0)
					{
						Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405, false);
					}
					enumerator = DirectXmlLoader.XmlAssetsInModFolder(this, "Defs/").GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						asset = enumerator.Current;
						defPackage = new DefPackage(asset.name, GenFilePaths.FolderPathRelativeToDefsFolder(asset.fullFolderPath, this));
						base.AddDefPackage(defPackage);
						asset.defPackage = defPackage;
						this.$current = asset;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			LoadableXmlAsset IEnumerator<LoadableXmlAsset>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.LoadableXmlAsset>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<LoadableXmlAsset> IEnumerable<LoadableXmlAsset>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ModContentPack.<LoadDefs>c__Iterator0 <LoadDefs>c__Iterator = new ModContentPack.<LoadDefs>c__Iterator0();
				<LoadDefs>c__Iterator.$this = this;
				return <LoadDefs>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetDefPackagesInFolder>c__AnonStorey1
		{
			internal string fullPath;

			internal ModContentPack $this;

			public <GetDefPackagesInFolder>c__AnonStorey1()
			{
			}

			internal bool <>m__0(DefPackage x)
			{
				return x.GetFullFolderPath(this.$this).StartsWith(this.fullPath);
			}
		}
	}
}
