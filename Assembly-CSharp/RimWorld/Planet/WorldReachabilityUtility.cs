using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000548 RID: 1352
	public static class WorldReachabilityUtility
	{
		// Token: 0x06001947 RID: 6471 RVA: 0x000DB96C File Offset: 0x000D9D6C
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
