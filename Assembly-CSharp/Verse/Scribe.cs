using System;

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
			bool result;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				result = false;
			}
			else if (Scribe.mode == LoadSaveMode.Saving)
			{
				result = Scribe.saver.EnterNode(nodeName);
			}
			else
			{
				result = ((Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit) || Scribe.loader.EnterNode(nodeName));
			}
			return result;
		}

		public static void ExitNode()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.saver.ExitNode();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.loader.ExitNode();
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Scribe()
		{
		}
	}
}
