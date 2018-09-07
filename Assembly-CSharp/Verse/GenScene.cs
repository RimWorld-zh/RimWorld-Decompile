using System;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	public static class GenScene
	{
		public const string EntrySceneName = "Entry";

		public const string PlaySceneName = "Play";

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null);
		}

		[CompilerGenerated]
		private static void <GoToMainMenu>m__0()
		{
			MemoryUtility.ClearAllMapsAndWorld();
			Current.Game = null;
		}
	}
}
