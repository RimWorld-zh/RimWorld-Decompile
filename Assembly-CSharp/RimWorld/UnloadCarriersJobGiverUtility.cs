using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000C7 RID: 199
	public static class UnloadCarriersJobGiverUtility
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x000342D0 File Offset: 0x000326D0
		public static bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2 != pawn && !pawn2.IsFreeColonist && pawn2.inventory.UnloadEverything && (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction) && !t.IsForbidden(pawn) && !t.IsBurning())
			{
				LocalTargetInfo target = t;
				if (pawn.CanReserve(target, 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}
	}
}
