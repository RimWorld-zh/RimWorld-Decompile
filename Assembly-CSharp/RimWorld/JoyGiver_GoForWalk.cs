using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000FD RID: 253
	public class JoyGiver_GoForWalk : JoyGiver
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x00039DC0 File Offset: 0x000381C0
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
				Predicate<IntVec3> cellValidator = (IntVec3 x) => !PawnUtility.KnownDangerAt(x, pawn.Map, pawn) && !x.GetTerrain(pawn.Map).avoidWander && x.Standable(pawn.Map) && !x.Roofed(pawn.Map);
				Predicate<Region> validator = delegate(Region x)
				{
					IntVec3 intVec;
					return x.Room.PsychologicallyOutdoors && !x.IsForbiddenEntirely(pawn) && x.TryFindRandomCellInRegionUnforbidden(pawn, cellValidator, out intVec);
				};
				Region reg;
				IntVec3 root;
				List<IntVec3> list;
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
					Job job = new Job(this.def.jobDef, list[0]);
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
