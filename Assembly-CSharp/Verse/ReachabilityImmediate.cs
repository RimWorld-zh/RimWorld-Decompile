using Verse.AI;

namespace Verse
{
	public static class ReachabilityImmediate
	{
		public static bool CanReachImmediate(IntVec3 start, LocalTargetInfo target, Map map, PathEndMode peMode, Pawn pawn)
		{
			bool result;
			if (!target.IsValid)
			{
				result = false;
				goto IL_01a0;
			}
			target = (LocalTargetInfo)GenPath.ResolvePathMode(pawn, target.ToTargetInfo(map), ref peMode);
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					if (thing.Map != map)
					{
						result = false;
						goto IL_01a0;
					}
					goto IL_00fe;
				}
				if (pawn != null)
				{
					if (pawn.carryTracker.innerContainer.Contains(thing))
					{
						result = true;
						goto IL_01a0;
					}
					if (pawn.inventory.innerContainer.Contains(thing))
					{
						result = true;
						goto IL_01a0;
					}
					if (pawn.apparel != null && pawn.apparel.Contains(thing))
					{
						result = true;
						goto IL_01a0;
					}
					if (pawn.equipment != null && pawn.equipment.Contains(thing))
					{
						result = true;
						goto IL_01a0;
					}
				}
				result = false;
				goto IL_01a0;
			}
			goto IL_00fe;
			IL_00fe:
			if (!target.HasThing || (target.Thing.def.size.x == 1 && target.Thing.def.size.z == 1))
			{
				if (start == target.Cell)
				{
					result = true;
					goto IL_01a0;
				}
			}
			else if (start.IsInside(target.Thing))
			{
				result = true;
				goto IL_01a0;
			}
			result = ((byte)((peMode == PathEndMode.Touch && TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(start, target, map)) ? 1 : 0) != 0);
			goto IL_01a0;
			IL_01a0:
			return result;
		}

		public static bool CanReachImmediate(this Pawn pawn, LocalTargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && ReachabilityImmediate.CanReachImmediate(pawn.Position, target, pawn.Map, peMode, pawn);
		}

		public static bool CanReachImmediateNonLocal(this Pawn pawn, TargetInfo target, PathEndMode peMode)
		{
			return pawn.Spawned && (target.Map == null || target.Map == pawn.Map) && pawn.CanReachImmediate((LocalTargetInfo)target, peMode);
		}

		public static bool CanReachImmediate(IntVec3 start, CellRect rect, Map map, PathEndMode peMode, Pawn pawn)
		{
			IntVec3 c = rect.ClosestCellTo(start);
			return ReachabilityImmediate.CanReachImmediate(start, c, map, peMode, pawn);
		}
	}
}
