using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000631 RID: 1585
	public static class WorldPawnsUtility
	{
		// Token: 0x06002099 RID: 8345 RVA: 0x00116E94 File Offset: 0x00115294
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
