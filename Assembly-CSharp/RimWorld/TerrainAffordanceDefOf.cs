using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000968 RID: 2408
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x040022FB RID: 8955
		public static TerrainAffordanceDef Light;

		// Token: 0x040022FC RID: 8956
		public static TerrainAffordanceDef Medium;

		// Token: 0x040022FD RID: 8957
		public static TerrainAffordanceDef Heavy;

		// Token: 0x040022FE RID: 8958
		public static TerrainAffordanceDef GrowSoil;

		// Token: 0x040022FF RID: 8959
		public static TerrainAffordanceDef Diggable;

		// Token: 0x04002300 RID: 8960
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x04002301 RID: 8961
		public static TerrainAffordanceDef MovingFluid;

		// Token: 0x06003672 RID: 13938 RVA: 0x001D128D File Offset: 0x001CF68D
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}
	}
}
