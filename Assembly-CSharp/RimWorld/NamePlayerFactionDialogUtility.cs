using System;
using System.IO;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000804 RID: 2052
	public static class NamePlayerFactionDialogUtility
	{
		// Token: 0x06002DDB RID: 11739 RVA: 0x0018251C File Offset: 0x0018091C
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && GenText.IsValidFilename(s);
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x00182558 File Offset: 0x00180958
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
