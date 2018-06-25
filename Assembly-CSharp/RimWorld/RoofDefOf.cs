using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092E RID: 2350
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x04002023 RID: 8227
		public static RoofDef RoofConstructed;

		// Token: 0x04002024 RID: 8228
		public static RoofDef RoofRockThick;

		// Token: 0x04002025 RID: 8229
		public static RoofDef RoofRockThin;

		// Token: 0x06003638 RID: 13880 RVA: 0x001D0BA5 File Offset: 0x001CEFA5
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}
	}
}
