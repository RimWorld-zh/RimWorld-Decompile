using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000BD8 RID: 3032
	public static class GenScene
	{
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06004215 RID: 16917 RVA: 0x0022CA84 File Offset: 0x0022AE84
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x0022CAB0 File Offset: 0x0022AEB0
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x0022CADC File Offset: 0x0022AEDC
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
