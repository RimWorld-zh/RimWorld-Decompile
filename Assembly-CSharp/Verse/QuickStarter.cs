using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000BDB RID: 3035
	internal static class QuickStarter
	{
		// Token: 0x06004233 RID: 16947 RVA: 0x0022D730 File Offset: 0x0022BB30
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

		// Token: 0x04002D3B RID: 11579
		private static bool quickStarted = false;
	}
}
