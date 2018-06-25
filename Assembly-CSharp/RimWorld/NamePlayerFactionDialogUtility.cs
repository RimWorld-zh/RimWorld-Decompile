using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class NamePlayerFactionDialogUtility
	{
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && GenText.IsValidFilename(s);
		}

		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
			if (Find.GameInfo.permadeathMode)
			{
				string oldSavefileName = Find.GameInfo.permadeathModeUniqueName;
				string newSavefileName = PermadeathModeUtility.GeneratePermadeathSaveNameBasedOnPlayerInput(s, oldSavefileName);
				if (oldSavefileName != newSavefileName)
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						Find.GameInfo.permadeathModeUniqueName = newSavefileName;
						Find.Autosaver.DoAutosave();
						FileInfo fileInfo = GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name) == oldSavefileName);
						if (fileInfo != null)
						{
							fileInfo.Delete();
						}
					}, "Autosaving", false, null);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <Named>c__AnonStorey0
		{
			internal string newSavefileName;

			internal string oldSavefileName;

			public <Named>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Find.GameInfo.permadeathModeUniqueName = this.newSavefileName;
				Find.Autosaver.DoAutosave();
				FileInfo fileInfo = GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name) == this.oldSavefileName);
				if (fileInfo != null)
				{
					fileInfo.Delete();
				}
			}

			internal bool <>m__1(FileInfo x)
			{
				return Path.GetFileNameWithoutExtension(x.Name) == this.oldSavefileName;
			}
		}
	}
}
