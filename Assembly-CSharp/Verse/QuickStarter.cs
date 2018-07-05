using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	public static class QuickStarter
	{
		private static bool quickStarted = false;

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

		// Note: this type is marked as 'beforefieldinit'.
		static QuickStarter()
		{
		}
	}
}
