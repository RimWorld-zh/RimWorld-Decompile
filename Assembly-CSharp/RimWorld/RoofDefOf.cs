using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092C RID: 2348
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x04002023 RID: 8227
		public static RoofDef RoofConstructed;

		// Token: 0x04002024 RID: 8228
		public static RoofDef RoofRockThick;

		// Token: 0x04002025 RID: 8229
		public static RoofDef RoofRockThin;

		// Token: 0x06003634 RID: 13876 RVA: 0x001D0A65 File Offset: 0x001CEE65
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}
	}
}
