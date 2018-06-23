using System;

namespace RimWorld
{
	// Token: 0x02000935 RID: 2357
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

		// Token: 0x0600363D RID: 13885 RVA: 0x001D0B07 File Offset: 0x001CEF07
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}
	}
}
