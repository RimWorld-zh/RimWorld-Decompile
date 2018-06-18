using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000DA4 RID: 3492
	public class ScribeMetaHeaderUtility
	{
		// Token: 0x06004DEF RID: 19951 RVA: 0x0028AED8 File Offset: 0x002892D8
		public static void WriteMetaHeader()
		{
			if (Scribe.EnterNode("meta"))
			{
				try
				{
					string currentVersionStringWithRev = VersionControl.CurrentVersionStringWithRev;
					Scribe_Values.Look<string>(ref currentVersionStringWithRev, "gameVersion", null, false);
					List<string> list = (from mod in LoadedModManager.RunningMods
					select mod.Identifier).ToList<string>();
					Scribe_Collections.Look<string>(ref list, "modIds", LookMode.Undefined, new object[0]);
					List<string> list2 = (from mod in LoadedModManager.RunningMods
					select mod.Name).ToList<string>();
					Scribe_Collections.Look<string>(ref list2, "modNames", LookMode.Undefined, new object[0]);
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0028AFAC File Offset: 0x002893AC
		public static void LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode mode, bool logVersionConflictWarning)
		{
			ScribeMetaHeaderUtility.loadedGameVersion = "Unknown";
			ScribeMetaHeaderUtility.loadedModIdsList = null;
			ScribeMetaHeaderUtility.loadedModNamesList = null;
			ScribeMetaHeaderUtility.lastMode = mode;
			if (Scribe.mode != LoadSaveMode.Inactive && Scribe.EnterNode("meta"))
			{
				try
				{
					Scribe_Values.Look<string>(ref ScribeMetaHeaderUtility.loadedGameVersion, "gameVersion", null, false);
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModIdsList, "modIds", LookMode.Undefined, new object[0]);
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModNamesList, "modNames", LookMode.Undefined, new object[0]);
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (logVersionConflictWarning)
			{
				if (mode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map || !UnityData.isEditor)
				{
					if (!ScribeMetaHeaderUtility.VersionsMatch())
					{
						Log.Warning(string.Concat(new object[]
						{
							"Loaded file (",
							mode,
							") is from version ",
							ScribeMetaHeaderUtility.loadedGameVersion,
							", we are running version ",
							VersionControl.CurrentVersionStringWithRev,
							"."
						}), false);
					}
				}
			}
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0028B0BC File Offset: 0x002894BC
		private static bool VersionsMatch()
		{
			return VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == VersionControl.BuildFromVersionString(VersionControl.CurrentVersionStringWithRev);
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x0028B0E8 File Offset: 0x002894E8
		public static bool TryCreateDialogsForVersionMismatchWarnings(Action confirmedAction)
		{
			string text = null;
			string text2 = null;
			if (!BackCompatibility.IsSaveCompatibleWith(ScribeMetaHeaderUtility.loadedGameVersion) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				text2 = "VersionMismatch".Translate();
				string text3 = (!ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty()) ? ScribeMetaHeaderUtility.loadedGameVersion : ("(" + "UnknownLower".Translate() + ")");
				if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map)
				{
					text = "SaveGameIncompatibleWarningText".Translate(new object[]
					{
						text3,
						VersionControl.CurrentVersionString
					});
				}
				else if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.World)
				{
					text = "WorldFileVersionMismatch".Translate(new object[]
					{
						text3,
						VersionControl.CurrentVersionString
					});
				}
				else
				{
					text = "FileIncompatibleWarning".Translate(new object[]
					{
						text3,
						VersionControl.CurrentVersionString
					});
				}
			}
			bool flag = false;
			string text4;
			string text5;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text4, out text5))
			{
				flag = true;
				string text6 = "ModsMismatchWarningText".Translate(new object[]
				{
					text4,
					text5
				});
				if (text == null)
				{
					text = text6;
				}
				else
				{
					text = text + "\n\n" + text6;
				}
				if (text2 == null)
				{
					text2 = "ModsMismatchWarningTitle".Translate();
				}
			}
			bool result;
			if (text != null)
			{
				ScribeMetaHeaderUtility.<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey0 <TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey = new ScribeMetaHeaderUtility.<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey0();
				ScribeMetaHeaderUtility.<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey0 <TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey2 = <TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey;
				string text7 = text;
				string title = text2;
				<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey2.dialog = Dialog_MessageBox.CreateConfirmation(text7, confirmedAction, false, title);
				<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey.dialog.buttonAText = "LoadAnyway".Translate();
				if (flag)
				{
					<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey.dialog.buttonCText = "ChangeLoadedMods".Translate();
					<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey.dialog.buttonCAction = delegate()
					{
						int num = ModLister.InstalledModsListHash(false);
						ModsConfig.SetActiveToList(ScribeMetaHeaderUtility.loadedModIdsList);
						ModsConfig.Save();
						if (num == ModLister.InstalledModsListHash(false))
						{
							IEnumerable<string> items = from id in Enumerable.Range(0, ScribeMetaHeaderUtility.loadedModIdsList.Count)
							where ModLister.GetModWithIdentifier(ScribeMetaHeaderUtility.loadedModIdsList[id]) == null
							select ScribeMetaHeaderUtility.loadedModNamesList[id];
							Messages.Message(string.Format("{0}: {1}", "MissingMods".Translate(), items.ToCommaList(false)), MessageTypeDefOf.RejectInput, false);
							<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey.dialog.buttonCClose = false;
						}
						else
						{
							ModsConfig.RestartFromChangedMods();
						}
					};
				}
				Find.WindowStack.Add(<TryCreateDialogsForVersionMismatchWarnings>c__AnonStorey.dialog);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x0028B2C4 File Offset: 0x002896C4
		public static bool LoadedModsMatchesActiveMods(out string loadedModsSummary, out string runningModsSummary)
		{
			loadedModsSummary = null;
			runningModsSummary = null;
			List<string> list = (from mod in LoadedModManager.RunningMods
			select mod.Identifier).ToList<string>();
			bool result;
			if (ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, list))
			{
				result = true;
			}
			else
			{
				if (ScribeMetaHeaderUtility.loadedModNamesList == null)
				{
					loadedModsSummary = "None".Translate();
				}
				else
				{
					loadedModsSummary = ScribeMetaHeaderUtility.loadedModNamesList.ToCommaList(false);
				}
				runningModsSummary = (from id in list
				select ModLister.GetModWithIdentifier(id).Name).ToCommaList(false);
				result = false;
			}
			return result;
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x0028B378 File Offset: 0x00289778
		private static bool ModListsMatch(List<string> a, List<string> b)
		{
			bool result;
			if (a == null || b == null)
			{
				result = false;
			}
			else if (a.Count != b.Count)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < a.Count; i++)
				{
					if (a[i] != b[i])
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x0028B3F4 File Offset: 0x002897F4
		public static string GameVersionOf(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new ArgumentException();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(file.FullName))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader))
						{
							if (xmlTextReader.ReadToDescendant("gameVersion"))
							{
								return VersionControl.VersionStringWithoutRev(xmlTextReader.ReadString());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception getting game version of " + file.Name + ": " + ex.ToString(), false);
			}
			return null;
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x0028B4DC File Offset: 0x002898DC
		public static bool ReadToMetaElement(XmlTextReader textReader)
		{
			return ScribeMetaHeaderUtility.ReadToNextElement(textReader) && ScribeMetaHeaderUtility.ReadToNextElement(textReader) && !(textReader.Name != "meta");
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0028B534 File Offset: 0x00289934
		private static bool ReadToNextElement(XmlTextReader textReader)
		{
			while (textReader.Read())
			{
				if (textReader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003403 RID: 13315
		private static ScribeMetaHeaderUtility.ScribeHeaderMode lastMode;

		// Token: 0x04003404 RID: 13316
		public static string loadedGameVersion;

		// Token: 0x04003405 RID: 13317
		public static List<string> loadedModIdsList;

		// Token: 0x04003406 RID: 13318
		public static List<string> loadedModNamesList;

		// Token: 0x04003407 RID: 13319
		public const string MetaNodeName = "meta";

		// Token: 0x04003408 RID: 13320
		public const string GameVersionNodeName = "gameVersion";

		// Token: 0x04003409 RID: 13321
		public const string ModIdsNodeName = "modIds";

		// Token: 0x0400340A RID: 13322
		public const string ModNamesNodeName = "modNames";

		// Token: 0x02000DA5 RID: 3493
		public enum ScribeHeaderMode
		{
			// Token: 0x04003410 RID: 13328
			None,
			// Token: 0x04003411 RID: 13329
			Map,
			// Token: 0x04003412 RID: 13330
			World,
			// Token: 0x04003413 RID: 13331
			Scenario
		}
	}
}
