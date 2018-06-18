using System;

namespace Verse
{
	// Token: 0x02000DA3 RID: 3491
	public static class Scribe
	{
		// Token: 0x06004DEA RID: 19946 RVA: 0x0028ADCC File Offset: 0x002891CC
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0028ADEC File Offset: 0x002891EC
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

		// Token: 0x06004DEC RID: 19948 RVA: 0x0028AE64 File Offset: 0x00289264
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

		// Token: 0x04003400 RID: 13312
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x04003401 RID: 13313
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x04003402 RID: 13314
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
