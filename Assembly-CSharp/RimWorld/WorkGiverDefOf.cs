using System;

namespace RimWorld
{
	// Token: 0x02000967 RID: 2407
	[DefOf]
	public static class WorkGiverDefOf
	{
		// Token: 0x040022FA RID: 8954
		public static WorkGiverDef Refuel;

		// Token: 0x0600366F RID: 13935 RVA: 0x001D0E8B File Offset: 0x001CF28B
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}
	}
}
