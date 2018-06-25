using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000BD7 RID: 3031
	public static class GenScene
	{
		// Token: 0x04002D31 RID: 11569
		public const string EntrySceneName = "Entry";

		// Token: 0x04002D32 RID: 11570
		public const string PlaySceneName = "Play";

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x0022D55C File Offset: 0x0022B95C
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x0022D588 File Offset: 0x0022B988
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x0022D5B4 File Offset: 0x0022B9B4
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null);
		}
	}
}
