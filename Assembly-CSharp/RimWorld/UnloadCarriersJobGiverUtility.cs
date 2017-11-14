using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class UnloadCarriersJobGiverUtility
	{
		public static bool HasJobOnThing(Pawn pawn, Thing t, bool forced)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2 != pawn && !pawn2.IsFreeColonist && pawn2.inventory.UnloadEverything && (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction) && !t.IsForbidden(pawn) && !t.IsBurning())
			{
				LocalTargetInfo target = t;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
					goto IL_0083;
				return true;
			}
			goto IL_0083;
			IL_0083:
			return false;
		}
	}
}
