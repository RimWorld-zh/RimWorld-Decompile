using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	public static class TranslationFilesCleaner
	{
		private const string NewlineTag = "NEWLINE";

		private const string NewlineTagFull = "<!--NEWLINE-->";

		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, LoadedLanguage.KeyedReplacement>, LoadedLanguage.KeyedReplacement> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<LoadedLanguage.KeyedReplacement, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<LoadedLanguage.KeyedReplacement, XElement> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<DefInjectionPackage, IEnumerable<KeyValuePair<string, DefInjectionPackage.DefInjection>>> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, DefInjectionPackage.DefInjection>, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, DefInjectionPackage.DefInjection>, bool> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<TranslationFilesCleaner.PossibleDefInjection, string> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, DefInjectionPackage.DefInjection>, string> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<TranslationFilesCleaner.PossibleDefInjection, string> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, DefInjectionPackage.DefInjection>, string> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<string, string> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, Backstory>, string> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<ModContentPack, bool> <>f__am$cacheC;

		[CompilerGenerated]
		private static Func<char, bool> <>f__mg$cache1;

		public static void CleanupTranslationFiles()
		{
			LoadedLanguage curLang = LanguageDatabase.activeLanguage;
			LoadedLanguage english = LanguageDatabase.defaultLanguage;
			if (curLang == english)
			{
				return;
			}
			IEnumerable<ModMetaData> activeModsInLoadOrder = ModsConfig.ActiveModsInLoadOrder;
			if (activeModsInLoadOrder.Count<ModMetaData>() != 1 || !activeModsInLoadOrder.First<ModMetaData>().IsCoreMod)
			{
				Messages.Message("MessageDisableModsBeforeCleaningTranslationFiles".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				if (curLang.anyKeyedReplacementsXmlParseError || curLang.anyDefInjectionsXmlParseError)
				{
					string text = curLang.lastKeyedReplacementsXmlParseErrorInFile ?? curLang.lastDefInjectionsXmlParseErrorInFile;
					Messages.Message("MessageCantCleanupTranslationFilesBeucaseOfXmlError".Translate(new object[]
					{
						text
					}), MessageTypeDefOf.RejectInput, false);
					return;
				}
				english.LoadData();
				curLang.LoadData();
				Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("ConfirmCleanupTranslationFiles".Translate(new object[]
				{
					curLang.FriendlyNameNative
				}), delegate
				{
					if (TranslationFilesCleaner.<>f__mg$cache0 == null)
					{
						TranslationFilesCleaner.<>f__mg$cache0 = new Action(TranslationFilesCleaner.DoCleanupTranslationFiles);
					}
					LongEventHandler.QueueLongEvent(TranslationFilesCleaner.<>f__mg$cache0, "CleaningTranslationFiles".Translate(), true, null);
				}, true, null);
				dialog_MessageBox.buttonAText = "ConfirmCleanupTranslationFiles_Confirm".Translate();
				Find.WindowStack.Add(dialog_MessageBox);
			}, null, false, null);
		}

		private static void DoCleanupTranslationFiles()
		{
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
			{
				return;
			}
			try
			{
				try
				{
					TranslationFilesCleaner.CleanupKeyedTranslations();
				}
				catch (Exception arg)
				{
					Log.Error("Could not cleanup keyed translations: " + arg, false);
				}
				try
				{
					TranslationFilesCleaner.CleanupDefInjections();
				}
				catch (Exception arg2)
				{
					Log.Error("Could not cleanup def-injections: " + arg2, false);
				}
				try
				{
					TranslationFilesCleaner.CleanupBackstories();
				}
				catch (Exception arg3)
				{
					Log.Error("Could not cleanup backstories: " + arg3, false);
				}
				Messages.Message("MessageTranslationFilesCleanupDone".Translate(new object[]
				{
					TranslationFilesCleaner.GetActiveLanguageCoreModFolderPath()
				}), MessageTypeDefOf.TaskCompletion, false);
			}
			catch (Exception arg4)
			{
				Log.Error("Could not cleanup translation files: " + arg4, false);
			}
		}

		private static void CleanupKeyedTranslations()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage english = LanguageDatabase.defaultLanguage;
			string activeLanguageCoreModFolderPath = TranslationFilesCleaner.GetActiveLanguageCoreModFolderPath();
			string text = Path.Combine(activeLanguageCoreModFolderPath, "CodeLinked");
			string text2 = Path.Combine(activeLanguageCoreModFolderPath, "Keyed");
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (directoryInfo.Exists)
			{
				if (!Directory.Exists(text2))
				{
					Directory.Move(text, text2);
					Thread.Sleep(1000);
					directoryInfo = new DirectoryInfo(text2);
				}
			}
			else
			{
				directoryInfo = new DirectoryInfo(text2);
			}
			if (!directoryInfo.Exists)
			{
				Log.Error("Could not find keyed translations folder for the active language.", false);
				return;
			}
			DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(TranslationFilesCleaner.GetEnglishLanguageCoreModFolderPath(), "Keyed"));
			if (!directoryInfo2.Exists)
			{
				Log.Error("English keyed translations folder doesn't exist.", false);
				return;
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
			{
				try
				{
					fileInfo.Delete();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not delete ",
						fileInfo.Name,
						": ",
						ex
					}), false);
				}
			}
			foreach (FileInfo fileInfo2 in directoryInfo2.GetFiles("*.xml", SearchOption.AllDirectories))
			{
				try
				{
					string path = new Uri(directoryInfo2.FullName + Path.DirectorySeparatorChar).MakeRelativeUri(new Uri(fileInfo2.FullName)).ToString();
					string text3 = Path.Combine(directoryInfo.FullName, path);
					Directory.CreateDirectory(Path.GetDirectoryName(text3));
					fileInfo2.CopyTo(text3);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not copy ",
						fileInfo2.Name,
						": ",
						ex2
					}), false);
				}
			}
			List<LoadedLanguage.KeyedReplacement> list = (from x in activeLanguage.keyedReplacements
			where !x.Value.isPlaceholder && !english.HaveTextForKey(x.Key, false)
			select x.Value).ToList<LoadedLanguage.KeyedReplacement>();
			HashSet<LoadedLanguage.KeyedReplacement> writtenUnusedKeyedTranslations = new HashSet<LoadedLanguage.KeyedReplacement>();
			foreach (FileInfo fileInfo3 in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
			{
				try
				{
					XDocument xdocument = XDocument.Load(fileInfo3.FullName, LoadOptions.PreserveWhitespace);
					XElement xelement = xdocument.DescendantNodes().OfType<XElement>().FirstOrDefault<XElement>();
					if (xelement != null)
					{
						try
						{
							foreach (XNode xnode in xelement.DescendantNodes())
							{
								XElement xelement2 = xnode as XElement;
								if (xelement2 != null)
								{
									foreach (XNode xnode2 in xelement2.DescendantNodes())
									{
										try
										{
											XText xtext = xnode2 as XText;
											if (xtext != null && !xtext.Value.NullOrEmpty())
											{
												string value = " EN: " + xtext.Value + " ";
												xnode.AddBeforeSelf(new XComment(value));
												xnode.AddBeforeSelf(Environment.NewLine);
												xnode.AddBeforeSelf("  ");
											}
										}
										catch (Exception ex3)
										{
											Log.Error(string.Concat(new object[]
											{
												"Could not add comment node in ",
												fileInfo3.Name,
												": ",
												ex3
											}), false);
										}
										xnode2.Remove();
									}
									try
									{
										string text4;
										if (activeLanguage.TryGetTextFromKey(xelement2.Name.ToString(), out text4))
										{
											if (!text4.NullOrEmpty())
											{
												xelement2.Add(new XText(text4.Replace("\n", "\\n")));
											}
										}
										else
										{
											xelement2.Add(new XText("TODO"));
										}
									}
									catch (Exception ex4)
									{
										Log.Error(string.Concat(new object[]
										{
											"Could not add existing translation or placeholder in ",
											fileInfo3.Name,
											": ",
											ex4
										}), false);
									}
								}
							}
							bool flag = false;
							foreach (LoadedLanguage.KeyedReplacement keyedReplacement in list)
							{
								if (new Uri(fileInfo3.FullName).Equals(new Uri(keyedReplacement.fileSourceFullPath)))
								{
									if (!flag)
									{
										xelement.Add("  ");
										xelement.Add(new XComment(" UNUSED "));
										xelement.Add(Environment.NewLine);
										flag = true;
									}
									XElement xelement3 = new XElement(keyedReplacement.key);
									if (keyedReplacement.isPlaceholder)
									{
										xelement3.Add(new XText("TODO"));
									}
									else if (!keyedReplacement.value.NullOrEmpty())
									{
										xelement3.Add(new XText(keyedReplacement.value.Replace("\n", "\\n")));
									}
									xelement.Add("  ");
									xelement.Add(xelement3);
									xelement.Add(Environment.NewLine);
									writtenUnusedKeyedTranslations.Add(keyedReplacement);
								}
							}
							if (flag)
							{
								xelement.Add(Environment.NewLine);
							}
						}
						finally
						{
							TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument.Root, fileInfo3.FullName);
						}
					}
				}
				catch (Exception ex5)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not process ",
						fileInfo3.Name,
						": ",
						ex5
					}), false);
				}
			}
			foreach (IGrouping<string, LoadedLanguage.KeyedReplacement> grouping in from x in list
			where !writtenUnusedKeyedTranslations.Contains(x)
			group x by x.fileSourceFullPath)
			{
				try
				{
					if (File.Exists(grouping.Key))
					{
						Log.Error("Could not save unused keyed translations to " + grouping.Key + " because this file already exists.", false);
					}
					else
					{
						object[] array = new object[1];
						int num = 0;
						XName name = "LanguageData";
						object[] array2 = new object[4];
						array2[0] = new XComment("NEWLINE");
						array2[1] = new XComment(" UNUSED ");
						array2[2] = grouping.Select(delegate(LoadedLanguage.KeyedReplacement x)
						{
							string text5 = (!x.isPlaceholder) ? x.value : "TODO";
							return new XElement(x.key, new XText((!text5.NullOrEmpty()) ? text5.Replace("\n", "\\n") : string.Empty));
						});
						array2[3] = new XComment("NEWLINE");
						array[num] = new XElement(name, array2);
						XDocument doc = new XDocument(array);
						TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(doc, grouping.Key);
					}
				}
				catch (Exception ex6)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not save unused keyed translations to ",
						grouping.Key,
						": ",
						ex6
					}), false);
				}
			}
		}

		private static void CleanupDefInjections()
		{
			string activeLanguageCoreModFolderPath = TranslationFilesCleaner.GetActiveLanguageCoreModFolderPath();
			string text = Path.Combine(activeLanguageCoreModFolderPath, "DefLinked");
			string text2 = Path.Combine(activeLanguageCoreModFolderPath, "DefInjected");
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (directoryInfo.Exists)
			{
				if (!Directory.Exists(text2))
				{
					Directory.Move(text, text2);
					Thread.Sleep(1000);
					directoryInfo = new DirectoryInfo(text2);
				}
			}
			else
			{
				directoryInfo = new DirectoryInfo(text2);
			}
			if (!directoryInfo.Exists)
			{
				Log.Error("Could not find def-injections folder for the active language.", false);
				return;
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
			{
				try
				{
					fileInfo.Delete();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not delete ",
						fileInfo.Name,
						": ",
						ex
					}), false);
				}
			}
			foreach (Type type in GenDefDatabase.AllDefTypesWithDatabases())
			{
				try
				{
					TranslationFilesCleaner.CleanupDefInjectionsForDefType(type, directoryInfo.FullName);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not process def-injections for type ",
						type.Name,
						": ",
						ex2
					}), false);
				}
			}
		}

		private static void CleanupDefInjectionsForDefType(Type defType, string defInjectionsFolderPath)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			List<KeyValuePair<string, DefInjectionPackage.DefInjection>> list = (from x in (from x in activeLanguage.defInjections
			where x.defType == defType
			select x).SelectMany((DefInjectionPackage x) => x.injections)
			where !x.Value.isPlaceholder
			select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
			Dictionary<string, DefInjectionPackage.DefInjection> dictionary = new Dictionary<string, DefInjectionPackage.DefInjection>();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in list)
			{
				if (!dictionary.ContainsKey(keyValuePair.Value.normalizedPath))
				{
					dictionary.Add(keyValuePair.Value.normalizedPath, keyValuePair.Value);
				}
			}
			List<TranslationFilesCleaner.PossibleDefInjection> possibleDefInjections = new List<TranslationFilesCleaner.PossibleDefInjection>();
			DefInjectionUtility.ForEachPossibleDefInjection(defType, delegate(string suggestedPath, string normalizedPath, bool isCollection, string str, IEnumerable<string> collection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
			{
				if (translationAllowed)
				{
					TranslationFilesCleaner.PossibleDefInjection possibleDefInjection2 = new TranslationFilesCleaner.PossibleDefInjection();
					possibleDefInjection2.suggestedPath = suggestedPath;
					possibleDefInjection2.normalizedPath = normalizedPath;
					possibleDefInjection2.isCollection = isCollection;
					possibleDefInjection2.fullListTranslationAllowed = fullListTranslationAllowed;
					possibleDefInjection2.curValue = str;
					possibleDefInjection2.curValueCollection = collection;
					possibleDefInjection2.fieldInfo = fieldInfo;
					possibleDefInjection2.def = def;
					possibleDefInjections.Add(possibleDefInjection2);
				}
			});
			if (!possibleDefInjections.Any<TranslationFilesCleaner.PossibleDefInjection>() && !list.Any<KeyValuePair<string, DefInjectionPackage.DefInjection>>())
			{
				return;
			}
			List<KeyValuePair<string, DefInjectionPackage.DefInjection>> source = (from x in list
			where !x.Value.injected
			select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
			using (IEnumerator<string> enumerator2 = (from x in possibleDefInjections
			select TranslationFilesCleaner.GetSourceFile(x.def)).Concat(from x in source
			select x.Value.fileSource).Distinct<string>().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string fileName = enumerator2.Current;
					try
					{
						XDocument xdocument = new XDocument();
						bool flag = false;
						try
						{
							XElement xelement = new XElement("LanguageData");
							xdocument.Add(xelement);
							xelement.Add(new XComment("NEWLINE"));
							List<TranslationFilesCleaner.PossibleDefInjection> source2 = (from x in possibleDefInjections
							where TranslationFilesCleaner.GetSourceFile(x.def) == fileName
							select x).ToList<TranslationFilesCleaner.PossibleDefInjection>();
							List<KeyValuePair<string, DefInjectionPackage.DefInjection>> source3 = (from x in source
							where x.Value.fileSource == fileName
							select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
							using (IEnumerator<string> enumerator3 = (from x in (from x in source2
							select x.def.defName).Concat(from x in source3
							select x.Value.DefName).Distinct<string>()
							orderby x
							select x).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									string defName = enumerator3.Current;
									try
									{
										IEnumerable<TranslationFilesCleaner.PossibleDefInjection> enumerable = from x in source2
										where x.def.defName == defName
										select x;
										IEnumerable<KeyValuePair<string, DefInjectionPackage.DefInjection>> enumerable2 = from x in source3
										where x.Value.DefName == defName
										select x;
										if (enumerable.Any<TranslationFilesCleaner.PossibleDefInjection>())
										{
											bool flag2 = false;
											foreach (TranslationFilesCleaner.PossibleDefInjection possibleDefInjection in enumerable)
											{
												if (possibleDefInjection.isCollection)
												{
													IEnumerable<string> englishList = TranslationFilesCleaner.GetEnglishList(possibleDefInjection.normalizedPath, possibleDefInjection.curValueCollection, dictionary);
													bool flag3 = false;
													if (englishList != null)
													{
														int num = 0;
														foreach (string text in englishList)
														{
															if (dictionary.ContainsKey(possibleDefInjection.normalizedPath + "." + num))
															{
																flag3 = true;
																break;
															}
															num++;
														}
													}
													if (flag3 || !possibleDefInjection.fullListTranslationAllowed)
													{
														if (englishList != null)
														{
															int num2 = -1;
															foreach (string text2 in englishList)
															{
																num2++;
																string key = possibleDefInjection.normalizedPath + "." + num2;
																string suggestedPath2 = possibleDefInjection.suggestedPath + "." + num2;
																DefInjectionPackage.DefInjection defInjection;
																if (!dictionary.TryGetValue(key, out defInjection))
																{
																	defInjection = null;
																}
																if (defInjection != null || DefInjectionUtility.ShouldCheckMissingInjection(text2, possibleDefInjection.fieldInfo, possibleDefInjection.def))
																{
																	flag2 = true;
																	flag = true;
																	try
																	{
																		if (!text2.NullOrEmpty())
																		{
																			xelement.Add(new XComment(" EN: " + text2.Replace("\n", "\\n") + " "));
																		}
																	}
																	catch (Exception ex)
																	{
																		Log.Error(string.Concat(new object[]
																		{
																			"Could not add comment node in ",
																			fileName,
																			": ",
																			ex
																		}), false);
																	}
																	xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(suggestedPath2, defInjection));
																}
															}
														}
													}
													else
													{
														bool flag4 = false;
														if (englishList != null)
														{
															foreach (string str2 in englishList)
															{
																if (DefInjectionUtility.ShouldCheckMissingInjection(str2, possibleDefInjection.fieldInfo, possibleDefInjection.def))
																{
																	flag4 = true;
																	break;
																}
															}
														}
														DefInjectionPackage.DefInjection defInjection2;
														if (!dictionary.TryGetValue(possibleDefInjection.normalizedPath, out defInjection2))
														{
															defInjection2 = null;
														}
														if (defInjection2 != null || flag4)
														{
															flag2 = true;
															flag = true;
															try
															{
																string text3 = TranslationFilesCleaner.ListToLiNodesString(englishList);
																if (!text3.NullOrEmpty())
																{
																	xelement.Add(new XComment(" EN:\n" + text3.Indented("    ") + "\n  "));
																}
															}
															catch (Exception ex2)
															{
																Log.Error(string.Concat(new object[]
																{
																	"Could not add comment node in ",
																	fileName,
																	": ",
																	ex2
																}), false);
															}
															xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(possibleDefInjection.suggestedPath, defInjection2));
														}
													}
												}
												else
												{
													DefInjectionPackage.DefInjection defInjection3;
													if (!dictionary.TryGetValue(possibleDefInjection.normalizedPath, out defInjection3))
													{
														defInjection3 = null;
													}
													string text4 = (defInjection3 == null || !defInjection3.injected) ? possibleDefInjection.curValue : defInjection3.replacedString;
													if (defInjection3 != null || DefInjectionUtility.ShouldCheckMissingInjection(text4, possibleDefInjection.fieldInfo, possibleDefInjection.def))
													{
														flag2 = true;
														flag = true;
														try
														{
															if (!text4.NullOrEmpty())
															{
																xelement.Add(new XComment(" EN: " + text4.Replace("\n", "\\n") + " "));
															}
														}
														catch (Exception ex3)
														{
															Log.Error(string.Concat(new object[]
															{
																"Could not add comment node in ",
																fileName,
																": ",
																ex3
															}), false);
														}
														xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(possibleDefInjection.suggestedPath, defInjection3));
													}
												}
											}
											if (flag2)
											{
												xelement.Add(new XComment("NEWLINE"));
											}
										}
										if (enumerable2.Any<KeyValuePair<string, DefInjectionPackage.DefInjection>>())
										{
											flag = true;
											xelement.Add(new XComment(" UNUSED "));
											foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair2 in enumerable2)
											{
												xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(keyValuePair2.Value.path, keyValuePair2.Value));
											}
											xelement.Add(new XComment("NEWLINE"));
										}
									}
									catch (Exception ex4)
									{
										Log.Error(string.Concat(new object[]
										{
											"Could not process def-injections for def ",
											defName,
											": ",
											ex4
										}), false);
									}
								}
							}
						}
						finally
						{
							if (flag)
							{
								string text5 = Path.Combine(defInjectionsFolderPath, defType.Name);
								Directory.CreateDirectory(text5);
								TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument, Path.Combine(text5, fileName));
							}
						}
					}
					catch (Exception ex5)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not process def-injections for file ",
							fileName,
							": ",
							ex5
						}), false);
					}
				}
			}
		}

		private static void CleanupBackstories()
		{
			string activeLanguageCoreModFolderPath = TranslationFilesCleaner.GetActiveLanguageCoreModFolderPath();
			string text = Path.Combine(activeLanguageCoreModFolderPath, "Backstories");
			Directory.CreateDirectory(text);
			string path = Path.Combine(text, "Backstories.xml");
			File.Delete(path);
			XDocument xdocument = new XDocument();
			try
			{
				XElement xelement = new XElement("BackstoryTranslations");
				xdocument.Add(xelement);
				xelement.Add(new XComment("NEWLINE"));
				foreach (KeyValuePair<string, Backstory> keyValuePair in from x in BackstoryDatabase.allBackstories
				orderby x.Key
				select x)
				{
					try
					{
						XElement xelement2 = new XElement(keyValuePair.Key);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "title", keyValuePair.Value.title, keyValuePair.Value.untranslatedTitle, keyValuePair.Value.titleTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "titleFemale", keyValuePair.Value.titleFemale, keyValuePair.Value.untranslatedTitleFemale, keyValuePair.Value.titleFemaleTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "titleShort", keyValuePair.Value.titleShort, keyValuePair.Value.untranslatedTitleShort, keyValuePair.Value.titleShortTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "titleShortFemale", keyValuePair.Value.titleShortFemale, keyValuePair.Value.untranslatedTitleShortFemale, keyValuePair.Value.titleShortFemaleTranslated);
						TranslationFilesCleaner.AddBackstoryFieldElement(xelement2, "desc", keyValuePair.Value.baseDesc, keyValuePair.Value.untranslatedDesc, keyValuePair.Value.descTranslated);
						xelement.Add(xelement2);
						xelement.Add(new XComment("NEWLINE"));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not process backstory ",
							keyValuePair.Key,
							": ",
							ex
						}), false);
					}
				}
			}
			finally
			{
				TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument, path);
			}
		}

		private static void AddBackstoryFieldElement(XElement addTo, string fieldName, string currentValue, string untranslatedValue, bool wasTranslated)
		{
			if (wasTranslated || !untranslatedValue.NullOrEmpty())
			{
				if (!untranslatedValue.NullOrEmpty())
				{
					addTo.Add(new XComment(" EN: " + untranslatedValue.Replace("\n", "\\n") + " "));
				}
				string text = (!wasTranslated) ? "TODO" : currentValue;
				addTo.Add(new XElement(fieldName, (!text.NullOrEmpty()) ? text.Replace("\n", "\\n") : string.Empty));
			}
		}

		private static string GetActiveLanguageCoreModFolderPath()
		{
			return TranslationFilesCleaner.GetLanguageCoreModFolderPath(LanguageDatabase.activeLanguage);
		}

		private static string GetEnglishLanguageCoreModFolderPath()
		{
			return TranslationFilesCleaner.GetLanguageCoreModFolderPath(LanguageDatabase.defaultLanguage);
		}

		private static string GetLanguageCoreModFolderPath(LoadedLanguage language)
		{
			ModContentPack modContentPack = LoadedModManager.RunningMods.FirstOrDefault((ModContentPack x) => x.IsCoreMod);
			string path = Path.Combine(modContentPack.RootDir, "Languages");
			return Path.Combine(path, language.folderName);
		}

		private static void SaveXMLDocumentWithProcessedNewlineTags(XNode doc, string path)
		{
			File.WriteAllText(path, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" + doc.ToString().Replace("<!--NEWLINE-->", string.Empty).Replace("&gt;", ">"), Encoding.UTF8);
		}

		private static string ListToLiNodesString(IEnumerable<string> list)
		{
			if (list == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in list)
			{
				stringBuilder.Append("<li>");
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(text.Replace("\n", "\\n"));
				}
				stringBuilder.Append("</li>");
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		private static XElement ListToXElement(IEnumerable<string> list, string name, List<Pair<int, string>> comments)
		{
			XElement xelement = new XElement(name);
			if (list != null)
			{
				int num = 0;
				foreach (string text in list)
				{
					if (comments != null)
					{
						for (int i = 0; i < comments.Count; i++)
						{
							if (comments[i].First == num)
							{
								xelement.Add(new XComment(comments[i].Second));
							}
						}
					}
					XElement xelement2 = new XElement("li");
					if (!text.NullOrEmpty())
					{
						xelement2.Add(new XText(text.Replace("\n", "\\n")));
					}
					xelement.Add(xelement2);
					num++;
				}
				if (comments != null)
				{
					for (int j = 0; j < comments.Count; j++)
					{
						if (comments[j].First == num)
						{
							xelement.Add(new XComment(comments[j].Second));
						}
					}
				}
			}
			return xelement;
		}

		private static string AppendXmlExtensionIfNotAlready(string fileName)
		{
			if (!fileName.ToLower().EndsWith(".xml"))
			{
				return fileName + ".xml";
			}
			return fileName;
		}

		private static string GetSourceFile(Def def)
		{
			if (def.defPackage != null)
			{
				return TranslationFilesCleaner.AppendXmlExtensionIfNotAlready(def.defPackage.fileName);
			}
			return "Unknown.xml";
		}

		private static string TryRemoveLastIndexSymbol(string str)
		{
			int num = str.LastIndexOf('.');
			if (num >= 0)
			{
				IEnumerable<char> source = str.Substring(num + 1);
				if (TranslationFilesCleaner.<>f__mg$cache1 == null)
				{
					TranslationFilesCleaner.<>f__mg$cache1 = new Func<char, bool>(char.IsNumber);
				}
				if (source.All(TranslationFilesCleaner.<>f__mg$cache1))
				{
					return str.Substring(0, num);
				}
			}
			return str;
		}

		private static IEnumerable<string> GetEnglishList(string normalizedPath, IEnumerable<string> curValue, Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath)
		{
			DefInjectionPackage.DefInjection defInjection;
			if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection) && defInjection.injected)
			{
				return defInjection.replacedList;
			}
			if (curValue == null)
			{
				return null;
			}
			List<string> list = curValue.ToList<string>();
			for (int i = 0; i < list.Count; i++)
			{
				string key = normalizedPath + "." + i;
				DefInjectionPackage.DefInjection defInjection2;
				if (injectionsByNormalizedPath.TryGetValue(key, out defInjection2) && defInjection2.injected)
				{
					list[i] = defInjection2.replacedString;
				}
			}
			return list;
		}

		private static XElement GetDefInjectableFieldNode(string suggestedPath, DefInjectionPackage.DefInjection existingInjection)
		{
			if (existingInjection == null || existingInjection.isPlaceholder)
			{
				return new XElement(suggestedPath, new XText("TODO"));
			}
			if (existingInjection.IsFullListInjection)
			{
				return TranslationFilesCleaner.ListToXElement(existingInjection.fullListInjection, suggestedPath, existingInjection.fullListInjectionComments);
			}
			XElement xelement = new XElement(suggestedPath);
			if (!existingInjection.injection.NullOrEmpty())
			{
				xelement.Add(new XText(existingInjection.injection.Replace("\n", "\\n")));
			}
			return xelement;
		}

		[CompilerGenerated]
		private static LoadedLanguage.KeyedReplacement <CleanupKeyedTranslations>m__0(KeyValuePair<string, LoadedLanguage.KeyedReplacement> x)
		{
			return x.Value;
		}

		[CompilerGenerated]
		private static string <CleanupKeyedTranslations>m__1(LoadedLanguage.KeyedReplacement x)
		{
			return x.fileSourceFullPath;
		}

		[CompilerGenerated]
		private static XElement <CleanupKeyedTranslations>m__2(LoadedLanguage.KeyedReplacement x)
		{
			string text = (!x.isPlaceholder) ? x.value : "TODO";
			return new XElement(x.key, new XText((!text.NullOrEmpty()) ? text.Replace("\n", "\\n") : string.Empty));
		}

		[CompilerGenerated]
		private static IEnumerable<KeyValuePair<string, DefInjectionPackage.DefInjection>> <CleanupDefInjectionsForDefType>m__3(DefInjectionPackage x)
		{
			return x.injections;
		}

		[CompilerGenerated]
		private static bool <CleanupDefInjectionsForDefType>m__4(KeyValuePair<string, DefInjectionPackage.DefInjection> x)
		{
			return !x.Value.isPlaceholder;
		}

		[CompilerGenerated]
		private static bool <CleanupDefInjectionsForDefType>m__5(KeyValuePair<string, DefInjectionPackage.DefInjection> x)
		{
			return !x.Value.injected;
		}

		[CompilerGenerated]
		private static string <CleanupDefInjectionsForDefType>m__6(TranslationFilesCleaner.PossibleDefInjection x)
		{
			return TranslationFilesCleaner.GetSourceFile(x.def);
		}

		[CompilerGenerated]
		private static string <CleanupDefInjectionsForDefType>m__7(KeyValuePair<string, DefInjectionPackage.DefInjection> x)
		{
			return x.Value.fileSource;
		}

		[CompilerGenerated]
		private static string <CleanupDefInjectionsForDefType>m__8(TranslationFilesCleaner.PossibleDefInjection x)
		{
			return x.def.defName;
		}

		[CompilerGenerated]
		private static string <CleanupDefInjectionsForDefType>m__9(KeyValuePair<string, DefInjectionPackage.DefInjection> x)
		{
			return x.Value.DefName;
		}

		[CompilerGenerated]
		private static string <CleanupDefInjectionsForDefType>m__A(string x)
		{
			return x;
		}

		[CompilerGenerated]
		private static string <CleanupBackstories>m__B(KeyValuePair<string, Backstory> x)
		{
			return x.Key;
		}

		[CompilerGenerated]
		private static bool <GetLanguageCoreModFolderPath>m__C(ModContentPack x)
		{
			return x.IsCoreMod;
		}

		private class PossibleDefInjection
		{
			public string suggestedPath;

			public string normalizedPath;

			public bool isCollection;

			public bool fullListTranslationAllowed;

			public string curValue;

			public IEnumerable<string> curValueCollection;

			public FieldInfo fieldInfo;

			public Def def;

			public PossibleDefInjection()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <CleanupTranslationFiles>c__AnonStorey0
		{
			internal LoadedLanguage curLang;

			internal LoadedLanguage english;

			private static Action <>f__am$cache0;

			public <CleanupTranslationFiles>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (this.curLang.anyKeyedReplacementsXmlParseError || this.curLang.anyDefInjectionsXmlParseError)
				{
					string text = this.curLang.lastKeyedReplacementsXmlParseErrorInFile ?? this.curLang.lastDefInjectionsXmlParseErrorInFile;
					Messages.Message("MessageCantCleanupTranslationFilesBeucaseOfXmlError".Translate(new object[]
					{
						text
					}), MessageTypeDefOf.RejectInput, false);
					return;
				}
				this.english.LoadData();
				this.curLang.LoadData();
				Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("ConfirmCleanupTranslationFiles".Translate(new object[]
				{
					this.curLang.FriendlyNameNative
				}), delegate
				{
					if (TranslationFilesCleaner.<>f__mg$cache0 == null)
					{
						TranslationFilesCleaner.<>f__mg$cache0 = new Action(TranslationFilesCleaner.DoCleanupTranslationFiles);
					}
					LongEventHandler.QueueLongEvent(TranslationFilesCleaner.<>f__mg$cache0, "CleaningTranslationFiles".Translate(), true, null);
				}, true, null);
				dialog_MessageBox.buttonAText = "ConfirmCleanupTranslationFiles_Confirm".Translate();
				Find.WindowStack.Add(dialog_MessageBox);
			}

			private static void <>m__1()
			{
				if (TranslationFilesCleaner.<>f__mg$cache0 == null)
				{
					TranslationFilesCleaner.<>f__mg$cache0 = new Action(TranslationFilesCleaner.DoCleanupTranslationFiles);
				}
				LongEventHandler.QueueLongEvent(TranslationFilesCleaner.<>f__mg$cache0, "CleaningTranslationFiles".Translate(), true, null);
			}
		}

		[CompilerGenerated]
		private sealed class <CleanupKeyedTranslations>c__AnonStorey1
		{
			internal LoadedLanguage english;

			internal HashSet<LoadedLanguage.KeyedReplacement> writtenUnusedKeyedTranslations;

			public <CleanupKeyedTranslations>c__AnonStorey1()
			{
			}

			internal bool <>m__0(KeyValuePair<string, LoadedLanguage.KeyedReplacement> x)
			{
				return !x.Value.isPlaceholder && !this.english.HaveTextForKey(x.Key, false);
			}

			internal bool <>m__1(LoadedLanguage.KeyedReplacement x)
			{
				return !this.writtenUnusedKeyedTranslations.Contains(x);
			}
		}

		[CompilerGenerated]
		private sealed class <CleanupDefInjectionsForDefType>c__AnonStorey2
		{
			internal Type defType;

			internal List<TranslationFilesCleaner.PossibleDefInjection> possibleDefInjections;

			public <CleanupDefInjectionsForDefType>c__AnonStorey2()
			{
			}

			internal bool <>m__0(DefInjectionPackage x)
			{
				return x.defType == this.defType;
			}

			internal void <>m__1(string suggestedPath, string normalizedPath, bool isCollection, string str, IEnumerable<string> collection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
			{
				if (translationAllowed)
				{
					TranslationFilesCleaner.PossibleDefInjection possibleDefInjection = new TranslationFilesCleaner.PossibleDefInjection();
					possibleDefInjection.suggestedPath = suggestedPath;
					possibleDefInjection.normalizedPath = normalizedPath;
					possibleDefInjection.isCollection = isCollection;
					possibleDefInjection.fullListTranslationAllowed = fullListTranslationAllowed;
					possibleDefInjection.curValue = str;
					possibleDefInjection.curValueCollection = collection;
					possibleDefInjection.fieldInfo = fieldInfo;
					possibleDefInjection.def = def;
					this.possibleDefInjections.Add(possibleDefInjection);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <CleanupDefInjectionsForDefType>c__AnonStorey3
		{
			internal string fileName;

			public <CleanupDefInjectionsForDefType>c__AnonStorey3()
			{
			}

			internal bool <>m__0(TranslationFilesCleaner.PossibleDefInjection x)
			{
				return TranslationFilesCleaner.GetSourceFile(x.def) == this.fileName;
			}

			internal bool <>m__1(KeyValuePair<string, DefInjectionPackage.DefInjection> x)
			{
				return x.Value.fileSource == this.fileName;
			}
		}

		[CompilerGenerated]
		private sealed class <CleanupDefInjectionsForDefType>c__AnonStorey4
		{
			internal string defName;

			public <CleanupDefInjectionsForDefType>c__AnonStorey4()
			{
			}

			internal bool <>m__0(TranslationFilesCleaner.PossibleDefInjection x)
			{
				return x.def.defName == this.defName;
			}

			internal bool <>m__1(KeyValuePair<string, DefInjectionPackage.DefInjection> x)
			{
				return x.Value.DefName == this.defName;
			}
		}
	}
}
