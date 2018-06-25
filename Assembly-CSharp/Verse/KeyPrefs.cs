using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000F5F RID: 3935
	public class KeyPrefs
	{
		// Token: 0x04003E73 RID: 15987
		private static KeyPrefsData data;

		// Token: 0x04003E74 RID: 15988
		private static Dictionary<string, KeyBindingData> unresolvedBindings;

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06005F3F RID: 24383 RVA: 0x00309308 File Offset: 0x00307708
		// (set) Token: 0x06005F40 RID: 24384 RVA: 0x00309322 File Offset: 0x00307722
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

		// Token: 0x06005F41 RID: 24385 RVA: 0x0030932C File Offset: 0x0030772C
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.KeyPrefsFilePath).Exists;
			Dictionary<string, KeyBindingData> dictionary = DirectXmlLoader.ItemFromXmlFile<Dictionary<string, KeyBindingData>>(GenFilePaths.KeyPrefsFilePath, true);
			KeyPrefs.data = new KeyPrefsData();
			KeyPrefs.unresolvedBindings = new Dictionary<string, KeyBindingData>();
			foreach (KeyValuePair<string, KeyBindingData> keyValuePair in dictionary)
			{
				KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(keyValuePair.Key);
				if (namedSilentFail != null)
				{
					KeyPrefs.data.keyPrefs[namedSilentFail] = keyValuePair.Value;
				}
				else
				{
					KeyPrefs.unresolvedBindings[keyValuePair.Key] = keyValuePair.Value;
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

		// Token: 0x06005F42 RID: 24386 RVA: 0x00309434 File Offset: 0x00307834
		public static void Save()
		{
			try
			{
				Dictionary<string, KeyBindingData> dictionary = new Dictionary<string, KeyBindingData>();
				foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in KeyPrefs.data.keyPrefs)
				{
					dictionary[keyValuePair.Key.defName] = keyValuePair.Value;
				}
				foreach (KeyValuePair<string, KeyBindingData> keyValuePair2 in KeyPrefs.unresolvedBindings)
				{
					try
					{
						dictionary.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					catch (ArgumentException)
					{
					}
				}
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(dictionary, typeof(KeyPrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.KeyPrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(new object[]
				{
					GenFilePaths.KeyPrefsFilePath,
					ex.ToString()
				}));
				Log.Error("Exception saving keyprefs: " + ex, false);
			}
		}

		// Token: 0x02000F60 RID: 3936
		public enum BindingSlot : byte
		{
			// Token: 0x04003E76 RID: 15990
			A,
			// Token: 0x04003E77 RID: 15991
			B
		}
	}
}
