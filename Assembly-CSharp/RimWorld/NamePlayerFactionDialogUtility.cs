using System;
using System.IO;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000806 RID: 2054
	public static class NamePlayerFactionDialogUtility
	{
		// Token: 0x06002DDF RID: 11743 RVA: 0x0018266C File Offset: 0x00180A6C
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && GenText.IsValidFilename(s);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x001826A8 File Offset: 0x00180AA8
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
	}
}
