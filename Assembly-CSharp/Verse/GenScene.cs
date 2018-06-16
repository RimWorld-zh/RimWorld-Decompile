using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000BD8 RID: 3032
	public static class GenScene
	{
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06004213 RID: 16915 RVA: 0x0022CA0C File Offset: 0x0022AE0C
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x0022CA38 File Offset: 0x0022AE38
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x0022CA64 File Offset: 0x0022AE64
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null);
		}

		// Token: 0x04002D25 RID: 11557
		public const string EntrySceneName = "Entry";

		// Token: 0x04002D26 RID: 11558
		public const string PlaySceneName = "Play";
	}
}
