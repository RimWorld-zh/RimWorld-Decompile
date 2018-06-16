using System;

namespace Verse
{
	// Token: 0x02000DA4 RID: 3492
	public static class Scribe
	{
		// Token: 0x06004DEC RID: 19948 RVA: 0x0028ADEC File Offset: 0x002891EC
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x0028AE0C File Offset: 0x0028920C
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

		// Token: 0x06004DEE RID: 19950 RVA: 0x0028AE84 File Offset: 0x00289284
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

		// Token: 0x04003402 RID: 13314
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x04003403 RID: 13315
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x04003404 RID: 13316
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
