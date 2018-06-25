using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	public static class BackstoryTranslationUtility
	{
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
						if (!value.NullOrEmpty() && value == backstory.untranslatedTitle)
						{
							list.Add(text + ".title '" + value.Replace("\n", "\\n") + "'");
						}
						if (!text2.NullOrEmpty() && text2 == backstory.untranslatedTitleFemale)
						{
							list.Add(text + ".titleFemale '" + text2.Replace("\n", "\\n") + "'");
						}
						if (!value2.NullOrEmpty() && value2 == backstory.untranslatedTitleShort)
						{
							list.Add(text + ".titleShort '" + value2.Replace("\n", "\\n") + "'");
						}
						if (!text3.NullOrEmpty() && text3 == backstory.untranslatedTitleShortFemale)
						{
							list.Add(text + ".titleShortFemale '" + text3.Replace("\n", "\\n") + "'");
						}
						if (!value3.NullOrEmpty() && value3 == backstory.untranslatedDesc)
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

		[CompilerGenerated]
		private sealed class <BackstoryTranslationElements>c__Iterator0 : IEnumerable, IEnumerable<XElement>, IEnumerator, IDisposable, IEnumerator<XElement>
		{
			internal IEnumerable<string> folderPaths;

			internal IEnumerator<string> $locvar0;

			internal string <folderPath>__1;

			internal string <localFolderPath>__2;

			internal FileInfo <fi>__2;

			internal XDocument <doc>__3;

			internal List<string> loadErrors;

			internal IEnumerator<XElement> $locvar1;

			internal XElement <element>__4;

			internal XElement $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <BackstoryTranslationElements>c__Iterator0()
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
					enumerator = folderPaths.GetEnumerator();
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
						Block_6:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								element = enumerator2.Current;
								this.$current = element;
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
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						folderPath = enumerator.Current;
						localFolderPath = folderPath;
						fi = new FileInfo(Path.Combine(localFolderPath.ToString(), "Backstories/Backstories.xml"));
						if (!fi.Exists)
						{
							return false;
						}
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
							return false;
						}
						enumerator2 = doc.Root.Elements().GetEnumerator();
						num = 4294967293u;
						goto Block_6;
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

			XElement IEnumerator<XElement>.Current
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
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
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
				return this.System.Collections.Generic.IEnumerable<System.Xml.Linq.XElement>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<XElement> IEnumerable<XElement>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BackstoryTranslationUtility.<BackstoryTranslationElements>c__Iterator0 <BackstoryTranslationElements>c__Iterator = new BackstoryTranslationUtility.<BackstoryTranslationElements>c__Iterator0();
				<BackstoryTranslationElements>c__Iterator.folderPaths = folderPaths;
				<BackstoryTranslationElements>c__Iterator.loadErrors = loadErrors;
				return <BackstoryTranslationElements>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <MissingBackstoryTranslations>c__AnonStorey1
		{
			internal string modifiedIdentifier;

			internal KeyValuePair<string, Backstory> backstory;

			public <MissingBackstoryTranslations>c__AnonStorey1()
			{
			}

			internal bool <>m__0(KeyValuePair<string, Backstory> x)
			{
				return x.Key == this.modifiedIdentifier;
			}

			internal bool <>m__1(KeyValuePair<string, Backstory> x)
			{
				return x.Key == this.modifiedIdentifier;
			}

			internal bool <>m__2(KeyValuePair<string, Backstory> x)
			{
				return x.Key == this.backstory.Key;
			}
		}
	}
}
