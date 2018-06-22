using System;

namespace Verse
{
	// Token: 0x02000DA0 RID: 3488
	public static class Scribe
	{
		// Token: 0x06004DFF RID: 19967 RVA: 0x0028C37C File Offset: 0x0028A77C
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x0028C39C File Offset: 0x0028A79C
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

		// Token: 0x06004E01 RID: 19969 RVA: 0x0028C414 File Offset: 0x0028A814
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

		// Token: 0x0400340B RID: 13323
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x0400340C RID: 13324
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x0400340D RID: 13325
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
