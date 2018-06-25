using System;

namespace RimWorld
{
	// Token: 0x02000931 RID: 2353
	[DefOf]
	public static class BiomeDefOf
	{
		// Token: 0x04002042 RID: 8258
		public static BiomeDef IceSheet;

		// Token: 0x04002043 RID: 8259
		public static BiomeDef Tundra;

		// Token: 0x04002044 RID: 8260
		public static BiomeDef BorealForest;

		// Token: 0x04002045 RID: 8261
		public static BiomeDef TemperateForest;

		// Token: 0x04002046 RID: 8262
		public static BiomeDef TropicalRainforest;

		// Token: 0x04002047 RID: 8263
		public static BiomeDef Desert;

		// Token: 0x04002048 RID: 8264
		public static BiomeDef AridShrubland;

		// Token: 0x04002049 RID: 8265
		public static BiomeDef SeaIce;

		// Token: 0x0400204A RID: 8266
		public static BiomeDef Ocean;

		// Token: 0x0400204B RID: 8267
		public static BiomeDef Lake;

		// Token: 0x0600363B RID: 13883 RVA: 0x001D0EAF File Offset: 0x001CF2AF
		static BiomeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BiomeDefOf));
		}
	}
}
