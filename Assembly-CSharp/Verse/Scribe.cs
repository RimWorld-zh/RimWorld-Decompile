using System;

namespace Verse
{
	// Token: 0x02000DA2 RID: 3490
	public static class Scribe
	{
		// Token: 0x0400340B RID: 13323
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x0400340C RID: 13324
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x0400340D RID: 13325
		public static LoadSaveMode mode = LoadSaveMode.Inactive;

		// Token: 0x06004E03 RID: 19971 RVA: 0x0028C4A8 File Offset: 0x0028A8A8
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x0028C4C8 File Offset: 0x0028A8C8
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

		// Token: 0x06004E05 RID: 19973 RVA: 0x0028C540 File Offset: 0x0028A940
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
	}
}
