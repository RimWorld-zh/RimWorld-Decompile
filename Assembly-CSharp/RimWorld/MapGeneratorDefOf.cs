using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094B RID: 2379
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x0400226F RID: 8815
		public static MapGeneratorDef Encounter;

		// Token: 0x04002270 RID: 8816
		public static MapGeneratorDef Base_Player;

		// Token: 0x04002271 RID: 8817
		public static MapGeneratorDef Base_Faction;

		// Token: 0x04002272 RID: 8818
		public static MapGeneratorDef EscapeShip;

		// Token: 0x06003653 RID: 13907 RVA: 0x001D0C93 File Offset: 0x001CF093
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}
	}
}
