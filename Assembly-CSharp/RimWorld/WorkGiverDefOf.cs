using System;

namespace RimWorld
{
	// Token: 0x02000969 RID: 2409
	[DefOf]
	public static class WorkGiverDefOf
	{
		// Token: 0x040022FB RID: 8955
		public static WorkGiverDef Refuel;

		// Token: 0x06003673 RID: 13939 RVA: 0x001D0FCB File Offset: 0x001CF3CB
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}
	}
}
