using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000BDA RID: 3034
	internal static class QuickStarter
	{
		// Token: 0x04002D47 RID: 11591
		private static bool quickStarted = false;

		// Token: 0x06004238 RID: 16952 RVA: 0x0022E208 File Offset: 0x0022C608
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
