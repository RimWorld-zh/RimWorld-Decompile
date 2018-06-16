using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000548 RID: 1352
	public static class WorldReachabilityUtility
	{
		// Token: 0x06001946 RID: 6470 RVA: 0x000DB918 File Offset: 0x000D9D18
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
