using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class LoadedLanguage
	{
		public string folderName;

		public LanguageInfo info = null;

		private LanguageWorker workerInt;

		private bool dataIsLoaded = false;

		public List<string> loadErrors = new List<string>();

		public List<string> backstoriesLoadErrors = new List<string>();

		public Texture2D icon = BaseContent.BadTex;

		public Dictionary<string, string> keyedReplacements = new Dictionary<string, string>();

		public Dictionary<string, Pair<string, int>> keyedReplacementsFileSource = new Dictionary<string, Pair<string, int>>();

		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		public LoadedLanguage(string folderPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			this.folderName = directoryInfo.Name;
		}

		public string FriendlyNameNative
		{
			get
			{
				string friendlyNameNative;
				if (this.info == null || this.info.friendlyNameNative.NullOrEmpty())
				{
					friendlyNameNative = this.folderName;
				}
				else
				{
					friendlyNameNative = this.info.friendlyNameNative;
				}
				return friendlyNameNative;
			}
		}

		public string FriendlyNameEnglish
		{
			get
			{
				string friendlyNameEnglish;
				if (this.info == null || this.info.friendlyNameEnglish.NullOrEmpty())
				{
					friendlyNameEnglish = this.folderName;
				}
				else
				{
					friendlyNameEnglish = this.info.friendlyNameEnglish;
				}
				return friendlyNameEnglish;
			}
		}

		public IEnumerable<string> FolderPaths
		{
			get
			{
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					string langDirPath = Path.Combine(mod.RootDir, "Languages");
					string myDirPath = Path.Combine(langDirPath, this.folderName);
					DirectoryInfo myDir = new DirectoryInfo(myDirPath);
					if (myDir.Exists)
					{
						yield return myDirPath;
					}
				}
				yield break;
			}
		}

		public LanguageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LanguageWorker)Activator.CreateInstance(this.info.languageWorkerClass);
				}
				return this.workerInt;
			}
		}

		public void TryLoadMetadataFrom(string folderPath)
		{
			if (this.info == null)
			{
				string filePath = Path.Combine(folderPath.ToString(), "LanguageInfo.xml");
				this.info = DirectXmlLoader.ItemFromXmlFile<LanguageInfo>(filePath, false);
				if (this.info.friendlyNameNative.NullOrEmpty())
				{
					FileInfo fileInfo = new FileInfo(Path.Combine(folderPath.ToString(), "FriendlyName.txt"));
					if (fileInfo.Exists)
					{
						this.info.friendlyNameNative = GenFile.TextFromRawFile(fileInfo.ToString());
					}
				}
				if (this.info.friendlyNameNative.NullOrEmpty())
				{
					this.info.friendlyNameNative = this.folderName;
				}
				if (this.info.friendlyNameEnglish.NullOrEmpty())
				{
					this.info.friendlyNameEnglish = this.folderName;
				}
			}
		}

		public void LoadData()
		{
			if (!this.dataIsLoaded)
			{
				this.dataIsLoaded = true;
				DeepProfiler.Start("Loading language data: " + this.folderName);
				foreach (string text in this.FolderPaths)
				{
					string localFolderPath = text;
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (this.icon == BaseContent.BadTex)
						{
							FileInfo fileInfo = new FileInfo(Path.Combine(localFolderPath.ToString(), "LangIcon.png"));
							if (fileInfo.Exists)
							{
								this.icon = ModContentLoader<Texture2D>.LoadItem(fileInfo.FullName, null).contentItem;
							}
						}
					});
					DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text.ToString(), "CodeLinked"));
					if (directoryInfo.Exists)
					{
						this.loadErrors.Add("Translations aren't called CodeLinked any more. Please rename to Keyed: " + directoryInfo);
					}
					else
					{
						directoryInfo = new DirectoryInfo(Path.Combine(text.ToString(), "Keyed"));
					}
					if (directoryInfo.Exists)
					{
						foreach (FileInfo file in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
						{
							this.LoadFromFile_Keyed(file);
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(text.ToString(), "DefLinked"));
					if (directoryInfo2.Exists)
					{
						this.loadErrors.Add("Translations aren't called DefLinked any more. Please rename to DefInjected: " + directoryInfo2);
					}
					else
					{
						directoryInfo2 = new DirectoryInfo(Path.Combine(text.ToString(), "DefInjected"));
					}
					if (directoryInfo2.Exists)
					{
						foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							string name = directoryInfo3.Name;
							Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name);
							if (typeInAnyAssembly == null && name.Length > 3)
							{
								typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1));
							}
							if (typeInAnyAssembly == null)
							{
								this.loadErrors.Add(string.Concat(new string[]
								{
									"Error loading language from ",
									text,
									": dir ",
									directoryInfo3.Name,
									" doesn't correspond to any def type. Skipping..."
								}));
							}
							else
							{
								foreach (FileInfo file2 in directoryInfo3.GetFiles("*.xml", SearchOption.AllDirectories))
								{
									this.LoadFromFile_DefInject(file2, typeInAnyAssembly);
								}
							}
						}
					}
					DirectoryInfo directoryInfo4 = new DirectoryInfo(Path.Combine(text.ToString(), "Strings"));
					if (directoryInfo4.Exists)
					{
						foreach (DirectoryInfo directoryInfo5 in directoryInfo4.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							foreach (FileInfo file3 in directoryInfo5.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								this.LoadFromFile_Strings(file3, directoryInfo4);
							}
						}
					}
				}
				DeepProfiler.End();
			}
		}

		private void LoadFromFile_Strings(FileInfo file, DirectoryInfo stringsTopDir)
		{
			string text;
			try
			{
				text = GenFile.TextFromRawFile(file.FullName);
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from strings file ",
					file,
					": ",
					ex
				}));
				return;
			}
			string text2 = file.FullName;
			if (stringsTopDir != null)
			{
				text2 = text2.Substring(stringsTopDir.FullName.Length + 1);
			}
			text2 = text2.Substring(0, text2.Length - Path.GetExtension(text2).Length);
			text2 = text2.Replace('\\', '/');
			List<string> list = new List<string>();
			foreach (string item in GenText.LinesFromString(text))
			{
				list.Add(item);
			}
			List<string> list2;
			if (this.stringFiles.TryGetValue(text2, out list2))
			{
				foreach (string item2 in list)
				{
					list2.Add(item2);
				}
			}
			else
			{
				this.stringFiles.Add(text2, list);
			}
		}

		private void LoadFromFile_Keyed(FileInfo file)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			try
			{
				foreach (DirectXmlLoaderSimple.XmlKeyValuePair xmlKeyValuePair in DirectXmlLoaderSimple.ValuesFromXmlFile(file))
				{
					if (this.keyedReplacements.ContainsKey(xmlKeyValuePair.key) || dictionary.ContainsKey(xmlKeyValuePair.key))
					{
						this.loadErrors.Add("Duplicate keyed translation key: " + xmlKeyValuePair.key + " in language " + this.folderName);
					}
					else
					{
						dictionary.Add(xmlKeyValuePair.key, xmlKeyValuePair.value);
						dictionary2.Add(xmlKeyValuePair.key, xmlKeyValuePair.lineNumber);
					}
				}
			}
			catch (Exception ex)
			{
				this.loadErrors.Add(string.Concat(new object[]
				{
					"Exception loading from translation file ",
					file,
					": ",
					ex
				}));
				dictionary.Clear();
			}
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				this.keyedReplacements.Add(keyValuePair.Key, keyValuePair.Value);
				this.keyedReplacementsFileSource.Add(keyValuePair.Key, new Pair<string, int>(file.Name, dictionary2[keyValuePair.Key]));
			}
		}

		public void LoadFromFile_DefInject(FileInfo file, Type defType)
		{
			DefInjectionPackage defInjectionPackage = (from di in this.defInjections
			where di.defType == defType
			select di).FirstOrDefault<DefInjectionPackage>();
			if (defInjectionPackage == null)
			{
				defInjectionPackage = new DefInjectionPackage(defType);
				this.defInjections.Add(defInjectionPackage);
			}
			defInjectionPackage.AddDataFromFile(file);
		}

		public bool HaveTextForKey(string key)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			return this.keyedReplacements.ContainsKey(key);
		}

		public bool TryGetTextFromKey(string key, out string translated)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			bool result;
			if (!this.keyedReplacements.TryGetValue(key, out translated))
			{
				translated = key;
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public bool TryGetStringsFromFile(string fileName, out List<string> stringsList)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			bool result;
			if (!this.stringFiles.TryGetValue(fileName, out stringsList))
			{
				stringsList = null;
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public string GetKeySourceFileAndLine(string key)
		{
			Pair<string, int> pair;
			string result;
			if (!this.keyedReplacementsFileSource.TryGetValue(key, out pair))
			{
				result = "unknown";
			}
			else
			{
				result = pair.First + ":" + pair.Second;
			}
			return result;
		}

		public void InjectIntoData_BeforeImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(false);
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg, false);
				}
			}
		}

		public void InjectIntoData_AfterImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			int num = this.loadErrors.Count;
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(true);
					num += defInjectionPackage.loadErrors.Count;
				}
				catch (Exception arg)
				{
					Log.Error("Critical error while injecting translations into defs: " + arg, false);
				}
			}
			BackstoryTranslationUtility.LoadAndInjectBackstoryData(this.FolderPaths, this.backstoriesLoadErrors);
			num += this.backstoriesLoadErrors.Count;
			if (num != 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Translation data for language ",
					LanguageDatabase.activeLanguage.FriendlyNameEnglish,
					" has ",
					num,
					" errors. Generate translation report for more info."
				}), false);
			}
		}

		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<ModContentPack> $locvar0;

			internal ModContentPack <mod>__1;

			internal string <langDirPath>__2;

			internal string <myDirPath>__2;

			internal DirectoryInfo <myDir>__2;

			internal LoadedLanguage $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
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
					enumerator = LoadedModManager.RunningMods.GetEnumerator();
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
					case 1u:
						IL_D5:
						break;
					}
					if (enumerator.MoveNext())
					{
						mod = enumerator.Current;
						langDirPath = Path.Combine(mod.RootDir, "Languages");
						myDirPath = Path.Combine(langDirPath, this.folderName);
						myDir = new DirectoryInfo(myDirPath);
						if (myDir.Exists)
						{
							this.$current = myDirPath;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_D5;
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

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				LoadedLanguage.<>c__Iterator0 <>c__Iterator = new LoadedLanguage.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <LoadData>c__AnonStorey1
		{
			internal string localFolderPath;

			internal LoadedLanguage $this;

			public <LoadData>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				if (this.$this.icon == BaseContent.BadTex)
				{
					FileInfo fileInfo = new FileInfo(Path.Combine(this.localFolderPath.ToString(), "LangIcon.png"));
					if (fileInfo.Exists)
					{
						this.$this.icon = ModContentLoader<Texture2D>.LoadItem(fileInfo.FullName, null).contentItem;
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <LoadFromFile_DefInject>c__AnonStorey2
		{
			internal Type defType;

			public <LoadFromFile_DefInject>c__AnonStorey2()
			{
			}

			internal bool <>m__0(DefInjectionPackage di)
			{
				return di.defType == this.defType;
			}
		}
	}
}
