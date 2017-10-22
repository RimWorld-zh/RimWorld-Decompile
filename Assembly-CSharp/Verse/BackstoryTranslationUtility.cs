using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Verse
{
	public static class BackstoryTranslationUtility
	{
		private static IEnumerable<XElement> BackstoryTranslationElements(LoadedLanguage lang)
		{
			foreach (string folderPath2 in lang.FolderPaths)
			{
				string _ = folderPath2;
				FileInfo fi = new FileInfo(Path.Combine(folderPath2.ToString(), "Backstories/Backstories.xml"));
				if (fi.Exists)
				{
					XDocument doc;
					try
					{
						doc = XDocument.Load(fi.FullName);
					}
					catch (Exception ex)
					{
						Log.Warning("Exception loading backstory translation data from file " + fi + ": " + ex);
						yield break;
					}
					using (IEnumerator<XElement> enumerator2 = doc.Root.Elements().GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							XElement element = enumerator2.Current;
							yield return element;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
					continue;
				}
				break;
			}
			yield break;
			IL_01c2:
			/*Error near IL_01c3: Unexpected return in MoveNext()*/;
		}

		public static void LoadAndInjectBackstoryData(LoadedLanguage lang)
		{
			foreach (XElement item in BackstoryTranslationUtility.BackstoryTranslationElements(lang))
			{
				string text = "[unknown]";
				try
				{
					text = item.Name.ToString();
					string value = item.Element("title").Value;
					string value2 = item.Element("titleShort").Value;
					string value3 = item.Element("desc").Value;
					Backstory backstory = default(Backstory);
					if (!BackstoryDatabase.TryGetWithIdentifier(text, out backstory))
					{
						throw new Exception("Backstory not found matching identifier " + text);
					}
					if (value == backstory.Title && value2 == backstory.TitleShort && value3 == backstory.baseDesc)
					{
						Log.Error("Backstory translation exactly matches default data: " + text);
					}
					else
					{
						if (value != null)
						{
							backstory.SetTitle(value);
						}
						if (value2 != null)
						{
							backstory.SetTitleShort(value2);
						}
						if (value3 != null)
						{
							backstory.baseDesc = value3;
						}
					}
				}
				catch (Exception ex)
				{
					Log.Warning("Couldn't load backstory " + text + ": " + ex + "\nFull XML text:\n\n" + item.ToString());
				}
			}
		}

		public static IEnumerable<string> MissingBackstoryTranslations(LoadedLanguage lang)
		{
			List<string> neededTranslations = (from kvp in BackstoryDatabase.allBackstories
			select kvp.Key).ToList();
			foreach (XElement item in BackstoryTranslationUtility.BackstoryTranslationElements(lang))
			{
				string identifier = item.Name.ToString();
				if (neededTranslations.Contains(identifier))
				{
					neededTranslations.Remove(identifier);
					string title = item.Element("title").Value;
					string titleShort = item.Element("titleShort").Value;
					string desc = item.Element("desc").Value;
					if (title.NullOrEmpty())
					{
						yield return identifier + ".title missing";
						/*Error: Unable to find new state assignment for yield return*/;
					}
					if (titleShort.NullOrEmpty())
					{
						yield return identifier + ".titleShort missing";
						/*Error: Unable to find new state assignment for yield return*/;
					}
					if (desc.NullOrEmpty())
					{
						yield return identifier + ".desc missing";
						/*Error: Unable to find new state assignment for yield return*/;
					}
					continue;
				}
				yield return "Translation doesn't correspond to any backstory: " + identifier;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			using (List<string>.Enumerator enumerator2 = neededTranslations.GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					string tra = enumerator2.Current;
					yield return "Missing backstory: " + tra;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0302:
			/*Error near IL_0303: Unexpected return in MoveNext()*/;
		}
	}
}
