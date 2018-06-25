using System;

namespace RimWorld
{
	// Token: 0x02000937 RID: 2359
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		// Token: 0x04002056 RID: 8278
		public static TimeAssignmentDef Anything;

		// Token: 0x04002057 RID: 8279
		public static TimeAssignmentDef Work;

		// Token: 0x04002058 RID: 8280
		public static TimeAssignmentDef Joy;

		// Token: 0x04002059 RID: 8281
		public static TimeAssignmentDef Sleep;

		// Token: 0x06003641 RID: 13889 RVA: 0x001D0F1B File Offset: 0x001CF31B
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}
	}
}
