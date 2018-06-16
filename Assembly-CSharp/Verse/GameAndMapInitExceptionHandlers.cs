using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C66 RID: 3174
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x060045B9 RID: 17849 RVA: 0x0024C1B0 File Offset: 0x0024A5B0
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

		// Token: 0x060045BA RID: 17850 RVA: 0x0024C218 File Offset: 0x0024A618
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x0024C240 File Offset: 0x0024A640
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
