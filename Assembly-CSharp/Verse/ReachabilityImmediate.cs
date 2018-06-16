using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C86 RID: 3206
	public static class ReachabilityImmediate
	{
		// Token: 0x0600462A RID: 17962 RVA: 0x0024ECA4 File Offset: 0x0024D0A4
		public static bool CanReachImmediate(IntVec3 start, LocalTargetInfo target, Map map, PathEndMode peMode, Pawn pawn)
		{
			bool result;
			if (!target.IsValid)
			{
				result = false;
			}
			else
			{
				target = (LocalTargetInfo)GenPath.ResolvePathMode(pawn, target.ToTargetInfo(map), ref peMode);
				if (target.HasThing)
				{
					Thing thing = target.Thing;
					if (!thing.Spawned)
					{
						if (pawn != null)
						{
							if (pawn.carryTracker.innerContainer.Contains(thing))
							{
								return true;
							}
							if (pawn.inventory.innerContainer.Contains(thing))
							{
								return true;
							}
							if (pawn.apparel != null && pawn.apparel.Contains(thing))
							{
								return true;
							}
							if (pawn.equipment != null && pawn.equipment.Contains(thing))
							{
								return true;
							}
						}
						return false;
					}
					if (thing.Map != map)
					{
						return false;
					}
				}
				if (!target.HasThing || (target.Thing.def.size.x == 1 && target.Thing.def.size.z == 1))
				{
					if (start == target.Cell)
					{
						return true;
					}
				}
				else if (start.IsInside(target.Thing))
				{
					return true;
				}
				result = (peMode == PathEndMode.Touch && TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(start, target, map));
			}
			return result;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0024EE54 File Offset: 0x0024D254
		public static bool CanReachImmediate(this Pawn pawn, LocalTargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && ReachabilityImmediate.CanReachImmediate(pawn.Position, target, pawn.Map, peMode, pawn);
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x0024EE90 File Offset: 0x0024D290
		public static bool CanReachImmediateNonLocal(this Pawn pawn, TargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && (target.Map == null || target.Map == pawn.Map) && pawn.CanReachImmediate((LocalTargetInfo)target, peMode);
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x0024EEEC File Offset: 0x0024D2EC
		public static bool CanReachImmediate(IntVec3 start, CellRect rect, Map map, PathEndMode peMode, Pawn pawn)
		{
			IntVec3 c = rect.ClosestCellTo(start);
			return ReachabilityImmediate.CanReachImmediate(start, c, map, peMode, pawn);
		}
	}
}
