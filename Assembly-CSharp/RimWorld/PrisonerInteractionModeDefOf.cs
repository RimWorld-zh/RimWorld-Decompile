using System;

namespace RimWorld
{
	// Token: 0x02000955 RID: 2389
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x0600365D RID: 13917 RVA: 0x001D0D47 File Offset: 0x001CF147
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		// Token: 0x0400229A RID: 8858
		public static PrisonerInteractionModeDef NoInteraction;

		// Token: 0x0400229B RID: 8859
		public static PrisonerInteractionModeDef Chat;

		// Token: 0x0400229C RID: 8860
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x0400229D RID: 8861
		public static PrisonerInteractionModeDef Release;

		// Token: 0x0400229E RID: 8862
		public static PrisonerInteractionModeDef Execution;
	}
}
