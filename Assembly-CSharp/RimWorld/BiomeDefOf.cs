using System;

namespace RimWorld
{
	// Token: 0x02000931 RID: 2353
	[DefOf]
	public static class BiomeDefOf
	{
		// Token: 0x0400203B RID: 8251
		public static BiomeDef IceSheet;

		// Token: 0x0400203C RID: 8252
		public static BiomeDef Tundra;

		// Token: 0x0400203D RID: 8253
		public static BiomeDef BorealForest;

		// Token: 0x0400203E RID: 8254
		public static BiomeDef TemperateForest;

		// Token: 0x0400203F RID: 8255
		public static BiomeDef TropicalRainforest;

		// Token: 0x04002040 RID: 8256
		public static BiomeDef Desert;

		// Token: 0x04002041 RID: 8257
		public static BiomeDef AridShrubland;

		// Token: 0x04002042 RID: 8258
		public static BiomeDef SeaIce;

		// Token: 0x04002043 RID: 8259
		public static BiomeDef Ocean;

		// Token: 0x04002044 RID: 8260
		public static BiomeDef Lake;

		// Token: 0x0600363B RID: 13883 RVA: 0x001D0BDB File Offset: 0x001CEFDB
		static BiomeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BiomeDefOf));
		}
	}
}
