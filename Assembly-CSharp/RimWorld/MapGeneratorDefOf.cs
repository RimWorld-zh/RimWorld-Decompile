using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094D RID: 2381
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x04002277 RID: 8823
		public static MapGeneratorDef Encounter;

		// Token: 0x04002278 RID: 8824
		public static MapGeneratorDef Base_Player;

		// Token: 0x04002279 RID: 8825
		public static MapGeneratorDef Base_Faction;

		// Token: 0x0400227A RID: 8826
		public static MapGeneratorDef EscapeShip;

		// Token: 0x06003657 RID: 13911 RVA: 0x001D10A7 File Offset: 0x001CF4A7
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}
	}
}
