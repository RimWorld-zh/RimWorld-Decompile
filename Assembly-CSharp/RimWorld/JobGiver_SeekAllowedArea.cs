using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SeekAllowedArea : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.Position.IsForbidden(pawn))
			{
				result = null;
			}
			else
			{
				Region region = pawn.GetRegion(RegionType.Set_Passable);
				if (region == null)
				{
					result = null;
				}
				else
				{
					TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
					RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParms, false));
					Region reg = null;
					RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
					{
						bool result2;
						if (r.portal != null)
						{
							result2 = false;
						}
						else if (!r.IsForbiddenEntirely(pawn))
						{
							reg = r;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
						return result2;
					};
					RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
					IntVec3 c = default(IntVec3);
					result = ((reg == null) ? null : (reg.TryFindRandomCellInRegionUnforbidden(pawn, (Predicate<IntVec3>)null, out c) ? new Job(JobDefOf.Goto, c) : null));
				}
			}
			return result;
		}
	}
}
