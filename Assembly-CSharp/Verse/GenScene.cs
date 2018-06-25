using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000BD6 RID: 3030
	public static class GenScene
	{
		// Token: 0x04002D2A RID: 11562
		public const string EntrySceneName = "Entry";

		// Token: 0x04002D2B RID: 11563
		public const string PlaySceneName = "Play";

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x0022D27C File Offset: 0x0022B67C
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x0022D2A8 File Offset: 0x0022B6A8
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x0022D2D4 File Offset: 0x0022B6D4
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
