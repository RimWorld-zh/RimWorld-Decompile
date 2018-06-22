using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C81 RID: 3201
	public static class ReachabilityUtility
	{
		// Token: 0x0600462D RID: 17965 RVA: 0x0024FF2C File Offset: 0x0024E32C
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x0024FF78 File Offset: 0x0024E378
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x0024FFC4 File Offset: 0x0024E3C4
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false));
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x0025000C File Offset: 0x0024E40C
		public static void ClearCache()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCache();
			}
		}
	}
}
