using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000BD4 RID: 3028
	public static class GenScene
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x0022D1A0 File Offset: 0x0022B5A0
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06004218 RID: 16920 RVA: 0x0022D1CC File Offset: 0x0022B5CC
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x0022D1F8 File Offset: 0x0022B5F8
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null);
		}

		// Token: 0x04002D2A RID: 11562
		public const string EntrySceneName = "Entry";

		// Token: 0x04002D2B RID: 11563
		public const string PlaySceneName = "Play";
	}
}
