using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000961 RID: 2401
	[DefOf]
	public static class SubcameraDefOf
	{
		// Token: 0x040022C7 RID: 8903
		public static SubcameraDef WaterDepth;

		// Token: 0x0600366B RID: 13931 RVA: 0x001D0F3B File Offset: 0x001CF33B
		static SubcameraDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SubcameraDefOf));
		}
	}
}
