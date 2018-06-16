using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000AAC RID: 2732
	public static class ReservationUtility
	{
		// Token: 0x06003CFF RID: 15615 RVA: 0x002043F4 File Offset: 0x002027F4
		public static bool CanReserve(this Pawn p, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x00204434 File Offset: 0x00202834
		public static bool CanReserveAndReach(this Pawn p, LocalTargetInfo target, PathEndMode peMode, Danger maxDanger, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.CanReach(target, peMode, maxDanger, false, TraverseMode.ByPawn) && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00204488 File Offset: 0x00202888
		public static bool Reserve(this Pawn p, LocalTargetInfo target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			return p.Spawned && p.Map.reservationManager.Reserve(p, job, target, maxPawns, stackCount, layer);
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x002044C8 File Offset: 0x002028C8
		public static void ReserveAsManyAsPossible(this Pawn p, List<LocalTargetInfo> target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			if (p.Spawned)
			{
				for (int i = 0; i < target.Count; i++)
				{
					p.Map.reservationManager.Reserve(p, job, target[i], maxPawns, stackCount, layer);
				}
			}
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x00204520 File Offset: 0x00202920
		public static bool HasReserved(this Pawn p, LocalTargetInfo target, Job job = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy(target, p, job);
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x0020455C File Offset: 0x0020295C
		public static bool HasReserved<TDriver>(this Pawn p, LocalTargetInfo target, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy<TDriver>(target, p, targetAIsNot, targetBIsNot, targetCIsNot);
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0020459C File Offset: 0x0020299C
		public static bool CanReserveNew(this Pawn p, LocalTargetInfo target)
		{
			return target.IsValid && !p.HasReserved(target, null) && p.CanReserve(target, 1, -1, null, false);
		}
	}
}
