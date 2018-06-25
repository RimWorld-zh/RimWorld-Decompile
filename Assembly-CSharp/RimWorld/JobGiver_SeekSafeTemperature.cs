using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SeekSafeTemperature : ThinkNode_JobGiver
	{
		public JobGiver_SeekSafeTemperature()
		{
		}

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

		[CompilerGenerated]
		private sealed class <ClosestRegionWithinTemperatureRange>c__AnonStorey0
		{
			internal TraverseParms traverseParms;

			internal FloatRange tempRange;

			internal Region foundReg;

			public <ClosestRegionWithinTemperatureRange>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.Allows(this.traverseParms, false);
			}

			internal bool <>m__1(Region r)
			{
				bool result;
				if (r.portal != null)
				{
					result = false;
				}
				else if (this.tempRange.Includes(r.Room.Temperature))
				{
					this.foundReg = r;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
	}
}
