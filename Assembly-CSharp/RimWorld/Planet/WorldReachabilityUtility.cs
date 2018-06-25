using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000546 RID: 1350
	public static class WorldReachabilityUtility
	{
		// Token: 0x06001942 RID: 6466 RVA: 0x000DBACC File Offset: 0x000D9ECC
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
