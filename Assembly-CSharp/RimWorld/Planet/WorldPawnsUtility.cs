using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200062F RID: 1583
	public static class WorldPawnsUtility
	{
		// Token: 0x06002094 RID: 8340 RVA: 0x001172F8 File Offset: 0x001156F8
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
