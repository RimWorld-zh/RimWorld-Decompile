using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000544 RID: 1348
	public static class WorldReachabilityUtility
	{
		// Token: 0x0600193E RID: 6462 RVA: 0x000DB97C File Offset: 0x000D9D7C
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
