using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200062D RID: 1581
	public static class WorldPawnsUtility
	{
		// Token: 0x06002091 RID: 8337 RVA: 0x00116F40 File Offset: 0x00115340
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
