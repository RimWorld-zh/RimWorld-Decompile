using System;

namespace RimWorld
{
	// Token: 0x02000937 RID: 2359
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		// Token: 0x0400204F RID: 8271
		public static TimeAssignmentDef Anything;

		// Token: 0x04002050 RID: 8272
		public static TimeAssignmentDef Work;

		// Token: 0x04002051 RID: 8273
		public static TimeAssignmentDef Joy;

		// Token: 0x04002052 RID: 8274
		public static TimeAssignmentDef Sleep;

		// Token: 0x06003641 RID: 13889 RVA: 0x001D0C47 File Offset: 0x001CF047
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}
	}
}
