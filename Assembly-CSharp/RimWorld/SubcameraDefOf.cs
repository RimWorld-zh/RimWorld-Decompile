using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095F RID: 2399
	[DefOf]
	public static class SubcameraDefOf
	{
		// Token: 0x040022C6 RID: 8902
		public static SubcameraDef WaterDepth;

		// Token: 0x06003667 RID: 13927 RVA: 0x001D0DFB File Offset: 0x001CF1FB
		static SubcameraDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SubcameraDefOf));
		}
	}
}
