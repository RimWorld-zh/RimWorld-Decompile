using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000966 RID: 2406
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x0600366E RID: 13934 RVA: 0x001D0E79 File Offset: 0x001CF279
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x040022F3 RID: 8947
		public static TerrainAffordanceDef Light;

		// Token: 0x040022F4 RID: 8948
		public static TerrainAffordanceDef Medium;

		// Token: 0x040022F5 RID: 8949
		public static TerrainAffordanceDef Heavy;

		// Token: 0x040022F6 RID: 8950
		public static TerrainAffordanceDef GrowSoil;

		// Token: 0x040022F7 RID: 8951
		public static TerrainAffordanceDef Diggable;

		// Token: 0x040022F8 RID: 8952
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x040022F9 RID: 8953
		public static TerrainAffordanceDef MovingFluid;
	}
}
