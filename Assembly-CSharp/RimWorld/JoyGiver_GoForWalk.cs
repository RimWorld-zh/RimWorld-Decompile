using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_GoForWalk : JoyGiver
	{
		public override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null))
			{
				result = null;
			}
			else if (PawnUtility.WillSoonHaveBasicNeed(pawn))
			{
				result = null;
			}
			else
			{
				Predicate<IntVec3> cellValidator = (Predicate<IntVec3>)((IntVec3 x) => !PawnUtility.KnownDangerAt(x, pawn));
				IntVec3 intVec = default(IntVec3);
				Predicate<Region> validator = (Predicate<Region>)((Region x) => x.Room.PsychologicallyOutdoors && !x.IsForbiddenEntirely(pawn) && x.TryFindRandomCellInRegionUnforbidden(pawn, cellValidator, out intVec));
				Region reg = default(Region);
				IntVec3 root = default(IntVec3);
				List<IntVec3> list = default(List<IntVec3>);
				if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), validator, 100, out reg, RegionType.Set_Passable))
				{
					result = null;
				}
				else if (!reg.TryFindRandomCellInRegionUnforbidden(pawn, cellValidator, out root))
				{
					result = null;
				}
				else if (!WalkPathFinder.TryFindWalkPath(pawn, root, out list))
				{
					result = null;
				}
				else
				{
					Job job = new Job(base.def.jobDef, list[0]);
					job.targetQueueA = new List<LocalTargetInfo>();
					for (int i = 1; i < list.Count; i++)
					{
						job.targetQueueA.Add(list[i]);
					}
					job.locomotionUrgency = LocomotionUrgency.Walk;
					result = job;
				}
			}
			return result;
		}
	}
}
