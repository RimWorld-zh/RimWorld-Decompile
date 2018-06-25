using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000968 RID: 2408
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x040022F4 RID: 8948
		public static TerrainAffordanceDef Light;

		// Token: 0x040022F5 RID: 8949
		public static TerrainAffordanceDef Medium;

		// Token: 0x040022F6 RID: 8950
		public static TerrainAffordanceDef Heavy;

		// Token: 0x040022F7 RID: 8951
		public static TerrainAffordanceDef GrowSoil;

		// Token: 0x040022F8 RID: 8952
		public static TerrainAffordanceDef Diggable;

		// Token: 0x040022F9 RID: 8953
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x040022FA RID: 8954
		public static TerrainAffordanceDef MovingFluid;

		// Token: 0x06003672 RID: 13938 RVA: 0x001D0FB9 File Offset: 0x001CF3B9
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}
	}
}
