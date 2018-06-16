using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000EF RID: 239
	public class JobGiver_SeekSafeTemperature : ThinkNode_JobGiver
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00038554 File Offset: 0x00036954
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
					result = new Job(JobDefOf.Wait_SafeTemperature, 500, true);
				}
				else
				{
					Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable);
					if (region != null)
					{
						result = new Job(JobDefOf.GotoSafeTemperature, region.RandomCell);
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000385F8 File Offset: 0x000369F8
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
				RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
				Region foundReg = null;
				RegionProcessor regionProcessor = delegate(Region r)
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
