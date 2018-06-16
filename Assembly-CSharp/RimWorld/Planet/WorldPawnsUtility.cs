using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000631 RID: 1585
	public static class WorldPawnsUtility
	{
		// Token: 0x06002097 RID: 8343 RVA: 0x00116E1C File Offset: 0x0011521C
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
