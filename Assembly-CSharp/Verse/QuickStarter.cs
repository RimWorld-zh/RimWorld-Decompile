using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000BD9 RID: 3033
	internal static class QuickStarter
	{
		// Token: 0x04002D40 RID: 11584
		private static bool quickStarted = false;

		// Token: 0x06004238 RID: 16952 RVA: 0x0022DF28 File Offset: 0x0022C328
		public static bool CheckQuickStart()
		{
			bool result;
			if (GenCommandLine.CommandLineArgPassed("quicktest") && !QuickStarter.quickStarted && GenScene.InEntryScene)
			{
				QuickStarter.quickStarted = true;
				SceneManager.LoadScene("Play");
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
