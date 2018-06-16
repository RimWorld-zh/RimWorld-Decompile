using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFB RID: 3067
	public class LoadedLanguage
	{
		// Token: 0x060042DF RID: 17119 RVA: 0x0023590C File Offset: 0x00233D0C
		public LoadedLanguage(string folderPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			this.folderName = directoryInfo.Name;
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x060042E0 RID: 17120 RVA: 0x00235990 File Offset: 0x00233D90
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

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x060042E1 RID: 17121 RVA: 0x002359DC File Offset: 0x00233DDC
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

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x060042E2 RID: 17122 RVA: 0x00235A28 File Offset: 0x00233E28
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

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x060042E3 RID: 17123 RVA: 0x00235A54 File Offset: 0x00233E54
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

		// Token: 0x060042E4 RID: 17124 RVA: 0x00235A98 File Offset: 0x00233E98
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

		// Token: 0x060042E5 RID: 17125 RVA: 0x00235B6C File Offset: 0x00233F6C
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

		// Token: 0x060042E6 RID: 17126 RVA: 0x00235EB0 File Offset: 0x002342B0
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

		// Token: 0x060042E7 RID: 17127 RVA: 0x0023602C File Offset: 0x0023442C
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

		// Token: 0x060042E8 RID: 17128 RVA: 0x002361E4 File Offset: 0x002345E4
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

		// Token: 0x060042E9 RID: 17129 RVA: 0x00236244 File Offset: 0x00234644
		public bool HaveTextForKey(string key)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			return this.keyedReplacements.ContainsKey(key);
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00236278 File Offset: 0x00234678
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

		// Token: 0x060042EB RID: 17131 RVA: 0x002362BC File Offset: 0x002346BC
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

		// Token: 0x060042EC RID: 17132 RVA: 0x00236300 File Offset: 0x00234700
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

		// Token: 0x060042ED RID: 17133 RVA: 0x00236350 File Offset: 0x00234750
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

		// Token: 0x060042EE RID: 17134 RVA: 0x002363F0 File Offset: 0x002347F0
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

		// Token: 0x060042EF RID: 17135 RVA: 0x0023650C File Offset: 0x0023490C
		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		// Token: 0x04002DA8 RID: 11688
		public string folderName;

		// Token: 0x04002DA9 RID: 11689
		public LanguageInfo info = null;

		// Token: 0x04002DAA RID: 11690
		private LanguageWorker workerInt;

		// Token: 0x04002DAB RID: 11691
		private bool dataIsLoaded = false;

		// Token: 0x04002DAC RID: 11692
		public List<string> loadErrors = new List<string>();

		// Token: 0x04002DAD RID: 11693
		public List<string> backstoriesLoadErrors = new List<string>();

		// Token: 0x04002DAE RID: 11694
		public Texture2D icon = BaseContent.BadTex;

		// Token: 0x04002DAF RID: 11695
		public Dictionary<string, string> keyedReplacements = new Dictionary<string, string>();

		// Token: 0x04002DB0 RID: 11696
		public Dictionary<string, Pair<string, int>> keyedReplacementsFileSource = new Dictionary<string, Pair<string, int>>();

		// Token: 0x04002DB1 RID: 11697
		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		// Token: 0x04002DB2 RID: 11698
		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();
	}
}
