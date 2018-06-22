using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000BD7 RID: 3031
	internal static class QuickStarter
	{
		// Token: 0x06004235 RID: 16949 RVA: 0x0022DE4C File Offset: 0x0022C24C
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

		// Token: 0x04002D40 RID: 11584
		private static bool quickStarted = false;
	}
}
