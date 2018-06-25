using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094D RID: 2381
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x04002270 RID: 8816
		public static MapGeneratorDef Encounter;

		// Token: 0x04002271 RID: 8817
		public static MapGeneratorDef Base_Player;

		// Token: 0x04002272 RID: 8818
		public static MapGeneratorDef Base_Faction;

		// Token: 0x04002273 RID: 8819
		public static MapGeneratorDef EscapeShip;

		// Token: 0x06003657 RID: 13911 RVA: 0x001D0DD3 File Offset: 0x001CF1D3
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}
	}
}
