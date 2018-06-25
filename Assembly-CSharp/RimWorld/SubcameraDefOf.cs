using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000961 RID: 2401
	[DefOf]
	public static class SubcameraDefOf
	{
		// Token: 0x040022CE RID: 8910
		public static SubcameraDef WaterDepth;

		// Token: 0x0600366B RID: 13931 RVA: 0x001D120F File Offset: 0x001CF60F
		static SubcameraDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SubcameraDefOf));
		}
	}
}
