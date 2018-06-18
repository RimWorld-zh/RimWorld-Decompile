using System;

namespace RimWorld
{
	// Token: 0x02000959 RID: 2393
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x06003664 RID: 13924 RVA: 0x001D0B5F File Offset: 0x001CEF5F
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		// Token: 0x0400229C RID: 8860
		public static PrisonerInteractionModeDef NoInteraction;

		// Token: 0x0400229D RID: 8861
		public static PrisonerInteractionModeDef Chat;

		// Token: 0x0400229E RID: 8862
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x0400229F RID: 8863
		public static PrisonerInteractionModeDef Release;

		// Token: 0x040022A0 RID: 8864
		public static PrisonerInteractionModeDef Execution;
	}
}
