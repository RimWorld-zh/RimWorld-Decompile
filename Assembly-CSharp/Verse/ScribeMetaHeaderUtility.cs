using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Verse
{
	public class ScribeMetaHeaderUtility
	{
		public enum ScribeHeaderMode
		{
			None,
			Map,
			World,
			Scenario
		}

		private static ScribeHeaderMode lastMode;

		public static string loadedGameVersion;

		public static List<string> loadedModIdsList;

		public static List<string> loadedModNamesList;

		public const string MetaNodeName = "meta";

		public const string GameVersionNodeName = "gameVersion";

		public const string ModIdsNodeName = "modIds";

		public const string ModNamesNodeName = "modNames";

		public static void WriteMetaHeader()
		{
			if (Scribe.EnterNode("meta"))
			{
				try
				{
					string currentVersionStringWithRev = VersionControl.CurrentVersionStringWithRev;
					Scribe_Values.Look(ref currentVersionStringWithRev, "gameVersion", null, false);
					List<string> list = (from mod in LoadedModManager.RunningMods
					select mod.Identifier).ToList();
					Scribe_Collections.Look(ref list, "modIds", LookMode.Undefined);
					List<string> list2 = (from mod in LoadedModManager.RunningMods
					select mod.Name).ToList();
					Scribe_Collections.Look(ref list2, "modNames", LookMode.Undefined);
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		public static void LoadGameDataHeader(ScribeHeaderMode mode, bool logVersionConflictWarning)
		{
			ScribeMetaHeaderUtility.loadedGameVersion = "Unknown";
			ScribeMetaHeaderUtility.loadedModIdsList = null;
			ScribeMetaHeaderUtility.loadedModNamesList = null;
			ScribeMetaHeaderUtility.lastMode = mode;
			if (Scribe.mode != 0 && Scribe.EnterNode("meta"))
			{
				try
				{
					Scribe_Values.Look<string>(ref ScribeMetaHeaderUtility.loadedGameVersion, "gameVersion", (string)null, false);
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
				if (mode != ScribeHeaderMode.Map && UnityData.isEditor)
					return;
				if (!ScribeMetaHeaderUtility.VersionsMatch())
				{
					Log.Warning("Loaded file (" + mode + ") is from version " + ScribeMetaHeaderUtility.loadedGameVersion + ", we are running version " + VersionControl.CurrentVersionStringWithRev + ".");
				}
			}
		}

		private static bool VersionsMatch()
		{
			return VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == VersionControl.BuildFromVersionString(VersionControl.CurrentVersionStringWithRev);
		}

		public static bool TryCreateDialogsForVersionMismatchWarnings(Action confirmedAction)
		{
			string text = null;
			string text2 = null;
			if (!BackCompatibility.IsSaveCompatibleWith(ScribeMetaHeaderUtility.loadedGameVersion) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				text2 = "VersionMismatch".Translate();
				string text3 = (!ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty()) ? ScribeMetaHeaderUtility.loadedGameVersion : ("(" + "UnknownLower".Translate() + ")");
				text = ((ScribeMetaHeaderUtility.lastMode != ScribeHeaderMode.Map) ? ((ScribeMetaHeaderUtility.lastMode != ScribeHeaderMode.World) ? "FileIncompatibleWarning".Translate(text3, VersionControl.CurrentVersionString) : "WorldFileVersionMismatch".Translate(text3, VersionControl.CurrentVersionString)) : "SaveGameIncompatibleWarningText".Translate(text3, VersionControl.CurrentVersionString));
			}
			bool flag = false;
			string text4 = default(string);
			string text5 = default(string);
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text4, out text5))
			{
				flag = true;
				string text6 = "ModsMismatchWarningText".Translate(text4, text5);
				text = ((text != null) ? (text + "\n\n" + text6) : text6);
				if (text2 == null)
				{
					text2 = "ModsMismatchWarningTitle".Translate();
				}
			}
			if (text != null)
			{
				string text7 = text;
				string title = text2;
				Dialog_MessageBox dialog = Dialog_MessageBox.CreateConfirmation(text7, confirmedAction, false, title);
				dialog.buttonAText = "LoadAnyway".Translate();
				if (flag)
				{
					dialog.buttonCText = "ChangeLoadedMods".Translate();
					dialog.buttonCAction = delegate
					{
						int num = ModLister.InstalledModsListHash(false);
						ModsConfig.SetActiveToList(ScribeMetaHeaderUtility.loadedModIdsList);
						ModsConfig.Save();
						if (num == ModLister.InstalledModsListHash(false))
						{
							IEnumerable<string> items = from id in Enumerable.Range(0, ScribeMetaHeaderUtility.loadedModIdsList.Count)
							where ModLister.GetModWithIdentifier(ScribeMetaHeaderUtility.loadedModIdsList[id]) == null
							select ScribeMetaHeaderUtility.loadedModNamesList[id];
							Messages.Message(string.Format("{0}: {1}", "MissingMods".Translate(), GenText.ToCommaList(items, true)), MessageTypeDefOf.RejectInput);
							dialog.buttonCClose = false;
						}
						else
						{
							ModsConfig.RestartFromChangedMods();
						}
					};
				}
				Find.WindowStack.Add(dialog);
				return true;
			}
			return false;
		}

		public static bool LoadedModsMatchesActiveMods(out string loadedModsSummary, out string runningModsSummary)
		{
			loadedModsSummary = null;
			runningModsSummary = null;
			List<string> list = (from mod in LoadedModManager.RunningMods
			select mod.Identifier).ToList();
			if (ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, list))
			{
				return true;
			}
			if (ScribeMetaHeaderUtility.loadedModNamesList == null)
			{
				loadedModsSummary = "None".Translate();
			}
			else
			{
				loadedModsSummary = GenText.ToCommaList(ScribeMetaHeaderUtility.loadedModNamesList, true);
			}
			runningModsSummary = GenText.ToCommaList(from id in list
			select ModLister.GetModWithIdentifier(id).Name, true);
			return false;
		}

		private static bool ModListsMatch(List<string> a, List<string> b)
		{
			if (a != null && b != null)
			{
				if (a.Count != b.Count)
				{
					return false;
				}
				for (int i = 0; i < a.Count; i++)
				{
					if (a[i] != b[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static string GameVersionOf(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new ArgumentException();
			}
			try
			{
				using (StreamReader input = new StreamReader(file.FullName))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(input))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader) && xmlTextReader.ReadToDescendant("gameVersion"))
						{
							return VersionControl.VersionStringWithoutRev(xmlTextReader.ReadString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception getting game version of " + file.Name + ": " + ex.ToString());
			}
			return null;
		}

		public static bool ReadToMetaElement(XmlTextReader textReader)
		{
			if (!ScribeMetaHeaderUtility.ReadToNextElement(textReader))
			{
				return false;
			}
			if (!ScribeMetaHeaderUtility.ReadToNextElement(textReader))
			{
				return false;
			}
			if (textReader.Name != "meta")
			{
				return false;
			}
			return true;
		}

		private static bool ReadToNextElement(XmlTextReader textReader)
		{
			while (true)
			{
				if (!textReader.Read())
				{
					return false;
				}
				if (textReader.NodeType == XmlNodeType.Element)
					break;
			}
			return true;
		}
	}
}
