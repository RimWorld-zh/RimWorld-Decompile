using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092E RID: 2350
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x0400202A RID: 8234
		public static RoofDef RoofConstructed;

		// Token: 0x0400202B RID: 8235
		public static RoofDef RoofRockThick;

		// Token: 0x0400202C RID: 8236
		public static RoofDef RoofRockThin;

		// Token: 0x06003638 RID: 13880 RVA: 0x001D0E79 File Offset: 0x001CF279
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}
	}
}
