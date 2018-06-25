using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000546 RID: 1350
	public static class WorldReachabilityUtility
	{
		// Token: 0x06001941 RID: 6465 RVA: 0x000DBD34 File Offset: 0x000DA134
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
