using System;

namespace RimWorld
{
	// Token: 0x02000934 RID: 2356
	[DefOf]
	public static class StorytellerDefOf
	{
		// Token: 0x04002052 RID: 8274
		public static StorytellerDef Cassandra;

		// Token: 0x04002053 RID: 8275
		public static StorytellerDef Tutor;

		// Token: 0x0600363E RID: 13886 RVA: 0x001D0EE5 File Offset: 0x001CF2E5
		static StorytellerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StorytellerDefOf));
		}
	}
}
