using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SeekSafeTemperature : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Serious))
			{
				result = null;
			}
			else
			{
				FloatRange tempRange = pawn.ComfortableTemperatureRange();
				if (tempRange.Includes(pawn.AmbientTemperature))
				{
					result = new Job(JobDefOf.WaitSafeTemperature, 500, true);
				}
				else
				{
					Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable);
					result = ((region == null) ? null : new Job(JobDefOf.GotoSafeTemperature, region.RandomCell));
				}
			}
			return result;
		}

		private static Region ClosestRegionWithinTemperatureRange(IntVec3 root, Map map, FloatRange tempRange, TraverseParms traverseParms, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = root.GetRegion(map, traversableRegionTypes);
			Region result;
			if (region == null)
			{
				result = null;
			}
			else
			{
				RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParms, false));
				Region foundReg = null;
				RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
				{
					bool result2;
					if (r.portal != null)
					{
						result2 = false;
					}
					else if (tempRange.Includes(r.Room.Temperature))
					{
						foundReg = r;
						result2 = true;
					}
					else
					{
						result2 = false;
					}
					return result2;
				};
				RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, traversableRegionTypes);
				result = foundReg;
			}
			return result;
		}
	}
}
