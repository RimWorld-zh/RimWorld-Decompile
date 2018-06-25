using System;

namespace RimWorld
{
	// Token: 0x02000957 RID: 2391
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x040022A2 RID: 8866
		public static PrisonerInteractionModeDef NoInteraction;

		// Token: 0x040022A3 RID: 8867
		public static PrisonerInteractionModeDef Chat;

		// Token: 0x040022A4 RID: 8868
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x040022A5 RID: 8869
		public static PrisonerInteractionModeDef Release;

		// Token: 0x040022A6 RID: 8870
		public static PrisonerInteractionModeDef Execution;

		// Token: 0x06003661 RID: 13921 RVA: 0x001D115B File Offset: 0x001CF55B
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}
	}
}
