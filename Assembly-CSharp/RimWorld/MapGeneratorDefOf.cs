using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094F RID: 2383
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x0600365A RID: 13914 RVA: 0x001D0AAB File Offset: 0x001CEEAB
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}

		// Token: 0x04002271 RID: 8817
		public static MapGeneratorDef Encounter;

		// Token: 0x04002272 RID: 8818
		public static MapGeneratorDef Base_Player;

		// Token: 0x04002273 RID: 8819
		public static MapGeneratorDef Base_Faction;

		// Token: 0x04002274 RID: 8820
		public static MapGeneratorDef EscapeShip;
	}
}
