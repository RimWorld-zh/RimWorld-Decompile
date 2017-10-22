using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class LoadedLanguage
	{
		public string folderName;

		public LanguageInfo info = null;

		private LanguageWorker workerInt;

		private bool dataIsLoaded = false;

		public Texture2D icon = BaseContent.BadTex;

		public Dictionary<string, string> keyedReplacements = new Dictionary<string, string>();

		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		public string FriendlyNameNative
		{
			get
			{
				return (this.info != null && !this.info.friendlyNameNative.NullOrEmpty()) ? this.info.friendlyNameNative : this.folderName;
			}
		}

		public string FriendlyNameEnglish
		{
			get
			{
				return (this.info != null && !this.info.friendlyNameEnglish.NullOrEmpty()) ? this.info.friendlyNameEnglish : this.folderName;
			}
		}

		public IEnumerable<string> FolderPaths
		{
			get
			{
				using (IEnumerator<ModContentPack> enumerator = LoadedModManager.RunningMods.GetEnumerator())
				{
					string myDirPath;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							ModContentPack mod = enumerator.Current;
							string langDirPath = Path.Combine(mod.RootDir, "Languages");
							myDirPath = Path.Combine(langDirPath, this.folderName);
							DirectoryInfo myDir = new DirectoryInfo(myDirPath);
							if (myDir.Exists)
								break;
							continue;
						}
						yield break;
					}
					yield return myDirPath;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_010f:
				/*Error near IL_0110: Unexpected return in MoveNext()*/;
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

		public LoadedLanguage(string folderPath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			this.folderName = directoryInfo.Name;
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
				foreach (string folderPath in this.FolderPaths)
				{
					string localFolderPath = folderPath;
					LongEventHandler.ExecuteWhenFinished((Action)delegate
					{
						if ((UnityEngine.Object)this.icon == (UnityEngine.Object)BaseContent.BadTex)
						{
							FileInfo fileInfo = new FileInfo(Path.Combine(localFolderPath.ToString(), "LangIcon.png"));
							if (fileInfo.Exists)
							{
								this.icon = ModContentLoader<Texture2D>.LoadItem(fileInfo.FullName, (string)null).contentItem;
							}
						}
					});
					DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(folderPath.ToString(), "CodeLinked"));
					if (directoryInfo.Exists)
					{
						Log.Warning("Translations aren't called CodeLinked any more. Please rename to Keyed: " + directoryInfo);
					}
					else
					{
						directoryInfo = new DirectoryInfo(Path.Combine(folderPath.ToString(), "Keyed"));
					}
					if (directoryInfo.Exists)
					{
						FileInfo[] files = directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories);
						for (int i = 0; i < files.Length; i++)
						{
							FileInfo file = files[i];
							this.LoadFromFile_Keyed(file);
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(folderPath.ToString(), "DefLinked"));
					if (directoryInfo2.Exists)
					{
						Log.Warning("Translations aren't called DefLinked any more. Please rename to DefInjected: " + directoryInfo2);
					}
					else
					{
						directoryInfo2 = new DirectoryInfo(Path.Combine(folderPath.ToString(), "DefInjected"));
					}
					if (directoryInfo2.Exists)
					{
						DirectoryInfo[] directories = directoryInfo2.GetDirectories("*", SearchOption.TopDirectoryOnly);
						for (int j = 0; j < directories.Length; j++)
						{
							DirectoryInfo directoryInfo3 = directories[j];
							string name = directoryInfo3.Name;
							Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name);
							if (typeInAnyAssembly == null && name.Length > 3)
							{
								typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1));
							}
							if (typeInAnyAssembly == null)
							{
								Log.Warning("Error loading language from " + folderPath + ": dir " + directoryInfo3.Name + " doesn't correspond to any def type. Skipping...");
							}
							else
							{
								FileInfo[] files2 = directoryInfo3.GetFiles("*.xml", SearchOption.AllDirectories);
								for (int k = 0; k < files2.Length; k++)
								{
									FileInfo file2 = files2[k];
									this.LoadFromFile_DefInject(file2, typeInAnyAssembly);
								}
							}
						}
					}
					DirectoryInfo directoryInfo4 = new DirectoryInfo(Path.Combine(folderPath.ToString(), "Strings"));
					if (directoryInfo4.Exists)
					{
						DirectoryInfo[] directories2 = directoryInfo4.GetDirectories("*", SearchOption.TopDirectoryOnly);
						for (int l = 0; l < directories2.Length; l++)
						{
							DirectoryInfo directoryInfo5 = directories2[l];
							FileInfo[] files3 = directoryInfo5.GetFiles("*.txt", SearchOption.AllDirectories);
							for (int m = 0; m < files3.Length; m++)
							{
								FileInfo file3 = files3[m];
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
				Log.Warning("Exception loading from strings file " + file + ": " + ex);
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
			List<string> list2 = default(List<string>);
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
			try
			{
				foreach (KeyValuePair<string, string> item in DirectXmlLoaderSimple.ValuesFromXmlFile(file))
				{
					if (this.keyedReplacements.ContainsKey(item.Key) || dictionary.ContainsKey(item.Key))
					{
						Log.Warning("Duplicate code-linked translation key: " + item.Key + " in language " + this.folderName);
					}
					else
					{
						dictionary.Add(item.Key, item.Value);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning("Exception loading from translation file " + file + ": " + ex);
				dictionary.Clear();
			}
			foreach (KeyValuePair<string, string> item2 in dictionary)
			{
				this.keyedReplacements.Add(item2.Key, item2.Value);
			}
		}

		public void LoadFromFile_DefInject(FileInfo file, Type defType)
		{
			DefInjectionPackage defInjectionPackage = (from di in this.defInjections
			where di.defType == defType
			select di).FirstOrDefault();
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

		public void InjectIntoData()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			foreach (DefInjectionPackage defInjection in this.defInjections)
			{
				defInjection.InjectIntoDefs();
			}
			BackstoryTranslationUtility.LoadAndInjectBackstoryData(this);
		}

		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}
	}
}
