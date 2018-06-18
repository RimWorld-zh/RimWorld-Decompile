using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000930 RID: 2352
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x0600363B RID: 13883 RVA: 0x001D087D File Offset: 0x001CEC7D
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
