using System;

namespace RimWorld
{
	// Token: 0x02000957 RID: 2391
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x0400229B RID: 8859
		public static PrisonerInteractionModeDef NoInteraction;

		// Token: 0x0400229C RID: 8860
		public static PrisonerInteractionModeDef Chat;

		// Token: 0x0400229D RID: 8861
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x0400229E RID: 8862
		public static PrisonerInteractionModeDef Release;

		// Token: 0x0400229F RID: 8863
		public static PrisonerInteractionModeDef Execution;

		// Token: 0x06003661 RID: 13921 RVA: 0x001D0E87 File Offset: 0x001CF287
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}
	}
}
