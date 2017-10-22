namespace Verse
{
	public static class Scribe
	{
		public static ScribeSaver saver = new ScribeSaver();

		public static ScribeLoader loader = new ScribeLoader();

		public static LoadSaveMode mode = LoadSaveMode.Inactive;

		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		public static bool EnterNode(string nodeName)
		{
			return Scribe.mode != 0 && ((Scribe.mode != LoadSaveMode.Saving) ? ((Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit) || Scribe.loader.EnterNode(nodeName)) : Scribe.saver.EnterNode(nodeName));
		}

		public static void ExitNode()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.saver.ExitNode();
			}
			if (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit)
				return;
			Scribe.loader.ExitNode();
		}
	}
}
