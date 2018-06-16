using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000930 RID: 2352
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x06003639 RID: 13881 RVA: 0x001D07B5 File Offset: 0x001CEBB5
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}

		// Token: 0x04002025 RID: 8229
		public static RoofDef RoofConstructed;

		// Token: 0x04002026 RID: 8230
		public static RoofDef RoofRockThick;

		// Token: 0x04002027 RID: 8231
		public static RoofDef RoofRockThin;
	}
}
