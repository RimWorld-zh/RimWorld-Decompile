using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C64 RID: 3172
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x060045C3 RID: 17859 RVA: 0x0024D634 File Offset: 0x0024BA34
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

		// Token: 0x060045C4 RID: 17860 RVA: 0x0024D69C File Offset: 0x0024BA9C
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x0024D6C4 File Offset: 0x0024BAC4
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
