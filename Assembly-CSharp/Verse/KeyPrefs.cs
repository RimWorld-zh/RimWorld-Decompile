using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Verse
{
	public class KeyPrefs
	{
		public enum BindingSlot : byte
		{
			A = 0,
			B = 1
		}

		private static KeyPrefsData data;

		private static Dictionary<string, KeyBindingData> unresolvedBindings;

		public static KeyPrefsData KeyPrefsData
		{
			get
			{
				return KeyPrefs.data;
			}
			set
			{
				KeyPrefs.data = value;
			}
		}

		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.KeyPrefsFilePath).Exists;
			Dictionary<string, KeyBindingData> dictionary = DirectXmlLoader.ItemFromXmlFile<Dictionary<string, KeyBindingData>>(GenFilePaths.KeyPrefsFilePath, true);
			KeyPrefs.data = new KeyPrefsData();
			KeyPrefs.unresolvedBindings = new Dictionary<string, KeyBindingData>();
			Dictionary<string, KeyBindingData>.Enumerator enumerator = dictionary.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, KeyBindingData> current = enumerator.Current;
					KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(current.Key);
					if (namedSilentFail != null)
					{
						KeyPrefs.data.keyPrefs[namedSilentFail] = current.Value;
					}
					else
					{
						KeyPrefs.unresolvedBindings[current.Key] = current.Value;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			if (flag)
			{
				KeyPrefs.data.ResetToDefaults();
			}
			KeyPrefs.data.AddMissingDefaultBindings();
			KeyPrefs.data.ErrorCheck();
			if (flag)
			{
				KeyPrefs.Save();
			}
		}

		public static void Save()
		{
			try
			{
				Dictionary<string, KeyBindingData> dictionary = new Dictionary<string, KeyBindingData>();
				Dictionary<KeyBindingDef, KeyBindingData>.Enumerator enumerator = KeyPrefs.data.keyPrefs.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<KeyBindingDef, KeyBindingData> current = enumerator.Current;
						dictionary[current.Key.defName] = current.Value;
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				Dictionary<string, KeyBindingData>.Enumerator enumerator2 = KeyPrefs.unresolvedBindings.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, KeyBindingData> current2 = enumerator2.Current;
						try
						{
							dictionary.Add(current2.Key, current2.Value);
						}
						catch (ArgumentException)
						{
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
				XDocument xDocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(dictionary, typeof(KeyPrefsData));
				xDocument.Add(content);
				xDocument.Save(GenFilePaths.KeyPrefsFilePath);
			}
			catch (Exception ex2)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.KeyPrefsFilePath, ex2.ToString()));
				Log.Error("Exception saving keyprefs: " + ex2);
			}
		}
	}
}
