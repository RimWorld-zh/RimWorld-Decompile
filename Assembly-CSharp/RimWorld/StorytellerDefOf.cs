using System;

namespace RimWorld
{
	// Token: 0x02000934 RID: 2356
	[DefOf]
	public static class StorytellerDefOf
	{
		// Token: 0x0400204B RID: 8267
		public static StorytellerDef Cassandra;

		// Token: 0x0400204C RID: 8268
		public static StorytellerDef Tutor;

		// Token: 0x0600363E RID: 13886 RVA: 0x001D0C11 File Offset: 0x001CF011
		static StorytellerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StorytellerDefOf));
		}
	}
}
