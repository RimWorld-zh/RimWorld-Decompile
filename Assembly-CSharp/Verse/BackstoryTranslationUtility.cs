using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BEF RID: 3055
	public static class BackstoryTranslationUtility
	{
		// Token: 0x06004297 RID: 17047 RVA: 0x00230A78 File Offset: 0x0022EE78
		private static IEnumerable<XElement> BackstoryTranslationElements(IEnumerable<string> folderPaths, List<string> loadErrors)
		{
			foreach (string folderPath in folderPaths)
			{
				string localFolderPath = folderPath;
				FileInfo fi = new FileInfo(Path.Combine(localFolderPath.ToString(), "Backstories/Backstories.xml"));
				if (!fi.Exists)
				{
					yield break;
				}
				XDocument doc;
				try
				{
					doc = XDocument.Load(fi.FullName);
				}
				catch (Exception ex)
				{
					if (loadErrors != null)
					{
						loadErrors.Add(string.Concat(new object[]
						{
							"Exception loading backstory translation data from file ",
							fi,
							": ",
							ex
						}));
					}
					yield break;
				}
				foreach (XElement element in doc.Root.Elements())
				{
					yield return element;
				}
			}
			yield break;
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x00230AAC File Offset: 0x0022EEAC
		public static void LoadAndInjectBackstoryData(IEnumerable<string> folderPaths, List<string> loadErrors)
		{
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(folderPaths, loadErrors))
			{
				string text = "[unknown]";
				try
				{
					text = xelement.Name.ToString();
					string value = xelement.Element("title").Value;
					string text2 = (xelement.Element("titleFemale") == null) ? null : xelement.Element("titleFemale").Value;
					string value2 = xelement.Element("titleShort").Value;
					string text3 = (xelement.Element("titleShortFemale") == null) ? null : xelement.Element("titleShortFemale").Value;
					string value3 = xelement.Element("desc").Value;
					Backstory backstory;
					if (!BackstoryDatabase.TryGetWithIdentifier(text, out backstory, false))
					{
						throw new Exception("Backstory not found matching identifier " + text);
					}
					if (value == backstory.title && text2 == backstory.titleFemale && value2 == backstory.titleShort && text3 == backstory.titleShortFemale && value3 == backstory.baseDesc)
					{
						throw new Exception("Backstory translation exactly matches default data: " + text);
					}
					if (value != null)
					{
						backstory.SetTitle(value, backstory.titleFemale);
					}
					if (text2 != null)
					{
						backstory.SetTitle(backstory.title, text2);
					}
					if (value2 != null)
					{
						backstory.SetTitleShort(value2, backstory.titleShortFemale);
					}
					if (text3 != null)
					{
						backstory.SetTitleShort(backstory.titleShort, text3);
					}
					if (value3 != null)
					{
						backstory.baseDesc = value3;
					}
				}
				catch (Exception ex)
				{
					loadErrors.Add(string.Concat(new object[]
					{
						"Couldn't load backstory ",
						text,
						": ",
						ex,
						"\nFull XML text:\n\n",
						xelement.ToString()
					}));
				}
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x00230D28 File Offset: 0x0022F128
		public static List<string> MissingBackstoryTranslations(LoadedLanguage lang)
		{
			List<KeyValuePair<string, Backstory>> list = BackstoryDatabase.allBackstories.ToList<KeyValuePair<string, Backstory>>();
			List<string> list2 = new List<string>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.FolderPaths, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					string modifiedIdentifier = BackstoryDatabase.GetIdentifierClosestMatch(text, false);
					bool flag = list.Any((KeyValuePair<string, Backstory> x) => x.Key == modifiedIdentifier);
					KeyValuePair<string, Backstory> backstory = list.Find((KeyValuePair<string, Backstory> x) => x.Key == modifiedIdentifier);
					if (flag)
					{
						list.RemoveAt(list.FindIndex((KeyValuePair<string, Backstory> x) => x.Key == backstory.Key));
						string value = xelement.Element("title").Value;
						string str = (xelement.Element("titleFemale") == null) ? null : xelement.Element("titleFemale").Value;
						string value2 = xelement.Element("titleShort").Value;
						string str2 = (xelement.Element("titleShortFemale") == null) ? null : xelement.Element("titleShortFemale").Value;
						string value3 = xelement.Element("desc").Value;
						if (value.NullOrEmpty())
						{
							list2.Add(text + ".title missing");
						}
						if (flag && !backstory.Value.titleFemale.NullOrEmpty() && str.NullOrEmpty())
						{
							list2.Add(text + ".titleFemale missing");
						}
						if (value2.NullOrEmpty())
						{
							list2.Add(text + ".titleShort missing");
						}
						if (flag && !backstory.Value.titleShortFemale.NullOrEmpty() && str2.NullOrEmpty())
						{
							list2.Add(text + ".titleShortFemale missing");
						}
						if (value3.NullOrEmpty())
						{
							list2.Add(text + ".desc missing");
						}
					}
					else
					{
						list2.Add("Translation doesn't correspond to any backstory: " + text);
					}
				}
				catch (Exception ex)
				{
					list2.Add(string.Concat(new object[]
					{
						"Exception reading ",
						xelement.Name,
						": ",
						ex.Message
					}));
				}
			}
			foreach (KeyValuePair<string, Backstory> keyValuePair in list)
			{
				list2.Add("Missing backstory: " + keyValuePair.Key);
			}
			return list2;
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x00231080 File Offset: 0x0022F480
		public static List<string> BackstoryTranslationsMatchingEnglish(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.FolderPaths, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					Backstory backstory;
					if (BackstoryDatabase.allBackstories.TryGetValue(BackstoryDatabase.GetIdentifierClosestMatch(text, true), out backstory))
					{
						string value = xelement.Element("title").Value;
						string text2 = (xelement.Element("titleFemale") == null) ? null : xelement.Element("titleFemale").Value;
						string value2 = xelement.Element("titleShort").Value;
						string text3 = (xelement.Element("titleShortFemale") == null) ? null : xelement.Element("titleShortFemale").Value;
						string value3 = xelement.Element("desc").Value;
						if (!value.NullOrEmpty() && value == backstory.rawEnglishTitle)
						{
							list.Add(text + ".title '" + value.Replace("\n", "\\n") + "'");
						}
						if (!text2.NullOrEmpty() && text2 == backstory.rawEnglishTitleFemale)
						{
							list.Add(text + ".titleFemale '" + text2.Replace("\n", "\\n") + "'");
						}
						if (!value2.NullOrEmpty() && value2 == backstory.rawEnglishTitleShort)
						{
							list.Add(text + ".titleShort '" + value2.Replace("\n", "\\n") + "'");
						}
						if (!text3.NullOrEmpty() && text3 == backstory.rawEnglishTitleShortFemale)
						{
							list.Add(text + ".titleShortFemale '" + text3.Replace("\n", "\\n") + "'");
						}
						if (!value3.NullOrEmpty() && value3 == backstory.rawEnglishDesc)
						{
							list.Add(text + ".desc '" + value3.Replace("\n", "\\n") + "'");
						}
					}
				}
				catch (Exception ex)
				{
					list.Add(string.Concat(new object[]
					{
						"Exception reading ",
						xelement.Name,
						": ",
						ex.Message
					}));
				}
			}
			return list;
		}
	}
}
