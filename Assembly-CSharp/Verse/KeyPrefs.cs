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
			foreach (KeyValuePair<string, KeyBindingData> item in dictionary)
			{
				KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(item.Key);
				if (namedSilentFail != null)
				{
					KeyPrefs.data.keyPrefs[namedSilentFail] = item.Value;
				}
				else
				{
					KeyPrefs.unresolvedBindings[item.Key] = item.Value;
				}
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
				foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyPref in KeyPrefs.data.keyPrefs)
				{
					dictionary[keyPref.Key.defName] = keyPref.Value;
				}
				foreach (KeyValuePair<string, KeyBindingData> unresolvedBinding in KeyPrefs.unresolvedBindings)
				{
					try
					{
						dictionary.Add(unresolvedBinding.Key, unresolvedBinding.Value);
					}
					catch (ArgumentException)
					{
					}
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
