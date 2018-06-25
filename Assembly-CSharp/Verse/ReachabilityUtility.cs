using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C83 RID: 3203
	public static class ReachabilityUtility
	{
		// Token: 0x06004630 RID: 17968 RVA: 0x00250008 File Offset: 0x0024E408
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x00250054 File Offset: 0x0024E454
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBash = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBash));
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x002500A0 File Offset: 0x0024E4A0
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false));
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x002500E8 File Offset: 0x0024E4E8
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
