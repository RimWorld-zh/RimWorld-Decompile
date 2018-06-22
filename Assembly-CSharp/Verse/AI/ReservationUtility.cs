using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000AA8 RID: 2728
	public static class ReservationUtility
	{
		// Token: 0x06003CFC RID: 15612 RVA: 0x002047EC File Offset: 0x00202BEC
		public static bool CanReserve(this Pawn p, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x0020482C File Offset: 0x00202C2C
		public static bool CanReserveAndReach(this Pawn p, LocalTargetInfo target, PathEndMode peMode, Danger maxDanger, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.CanReach(target, peMode, maxDanger, false, TraverseMode.ByPawn) && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00204880 File Offset: 0x00202C80
		public static bool Reserve(this Pawn p, LocalTargetInfo target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			return p.Spawned && p.Map.reservationManager.Reserve(p, job, target, maxPawns, stackCount, layer);
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x002048C0 File Offset: 0x00202CC0
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

		// Token: 0x06003D00 RID: 15616 RVA: 0x00204918 File Offset: 0x00202D18
		public static bool HasReserved(this Pawn p, LocalTargetInfo target, Job job = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy(target, p, job);
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00204954 File Offset: 0x00202D54
		public static bool HasReserved<TDriver>(this Pawn p, LocalTargetInfo target, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy<TDriver>(target, p, targetAIsNot, targetBIsNot, targetCIsNot);
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x00204994 File Offset: 0x00202D94
		public static bool CanReserveNew(this Pawn p, LocalTargetInfo target)
		{
			return target.IsValid && !p.HasReserved(target, null) && p.CanReserve(target, 1, -1, null, false);
		}
	}
}
