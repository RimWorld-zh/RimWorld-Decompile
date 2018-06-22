using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C62 RID: 3170
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x060045C0 RID: 17856 RVA: 0x0024D558 File Offset: 0x0024B958
		public static void ErrorWhileLoadingAssets(Exception e)
		{
			string text = "ErrorWhileLoadingAssets".Translate();
			if (ModsConfig.ActiveModsInLoadOrder.Count<ModMetaData>() != 1 || !ModsConfig.ActiveModsInLoadOrder.First<ModMetaData>().IsCoreMod)
			{
				text = text + "\n\n" + "ErrorWhileLoadingAssets_ModsInfo".Translate();
			}
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingAssetsTitle".Translate());
			GenScene.GoToMainMenu();
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x0024D5C0 File Offset: 0x0024B9C0
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x0024D5E8 File Offset: 0x0024B9E8
		public static void ErrorWhileLoadingGame(Exception e)
		{
			string text = "ErrorWhileLoadingMap".Translate();
			string text2;
			string text3;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text2, out text3))
			{
				text = text + "\n\n" + "ModsMismatchWarningText".Translate(new object[]
				{
					text2,
					text3
				});
			}
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}
	}
}
