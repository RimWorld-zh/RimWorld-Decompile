using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200062F RID: 1583
	public static class WorldPawnsUtility
	{
		// Token: 0x06002095 RID: 8341 RVA: 0x00117090 File Offset: 0x00115490
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
