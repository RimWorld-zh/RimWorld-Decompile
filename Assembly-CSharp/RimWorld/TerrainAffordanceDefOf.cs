using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096A RID: 2410
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x06003673 RID: 13939 RVA: 0x001D0BC9 File Offset: 0x001CEFC9
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x040022F5 RID: 8949
		public static TerrainAffordanceDef Light;

		// Token: 0x040022F6 RID: 8950
		public static TerrainAffordanceDef Medium;

		// Token: 0x040022F7 RID: 8951
		public static TerrainAffordanceDef Heavy;

		// Token: 0x040022F8 RID: 8952
		public static TerrainAffordanceDef GrowSoil;

		// Token: 0x040022F9 RID: 8953
		public static TerrainAffordanceDef Diggable;

		// Token: 0x040022FA RID: 8954
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x040022FB RID: 8955
		public static TerrainAffordanceDef MovingFluid;
	}
}
