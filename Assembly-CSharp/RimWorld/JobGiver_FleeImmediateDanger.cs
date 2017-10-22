using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_FleeImmediateDanger : ThinkNode_JobGiver
	{
		private const int FleeDistance = 18;

		private const int DistToDangerToFlee = 18;

		private const int DistToFireToFlee = 10;

		private const int MinFiresNearbyToFlee = 60;

		private const int MinFiresNearbyRadius = 20;

		private const int MinFiresNearbyRegionsToScan = 18;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			Job job;
			Job job2;
			if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
			{
				result = null;
			}
			else
			{
				List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
				for (int i = 0; i < list.Count; i++)
				{
					if (pawn.Position.InHorDistOf(list[i].Position, 18f))
					{
						job = this.FleeJob(pawn, list[i]);
						if (job != null)
							goto IL_007e;
					}
				}
				if (pawn.RaceProps.Animal && pawn.Faction == null)
				{
					List<Thing> list2 = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.WildAnimalsFlee);
					for (int j = 0; j < list2.Count; j++)
					{
						if (pawn.Position.InHorDistOf(list2[j].Position, 18f))
						{
							job2 = this.FleeJob(pawn, list2[j]);
							if (job2 != null)
								goto IL_0115;
						}
					}
					Job job3 = this.FleeLargeFireJob(pawn);
					if (job3 != null)
					{
						result = job3;
						goto IL_0152;
					}
				}
				result = null;
			}
			goto IL_0152;
			IL_0152:
			return result;
			IL_007e:
			result = job;
			goto IL_0152;
			IL_0115:
			result = job2;
			goto IL_0152;
		}

		private Job FleeJob(Pawn pawn, Thing danger)
		{
			IntVec3 intVec = (pawn.CurJob == null || pawn.CurJob.def != JobDefOf.Flee) ? CellFinderLoose.GetFleeDest(pawn, new List<Thing>
			{
				danger
			}, 18f) : pawn.CurJob.targetA.Cell;
			return (!(intVec != pawn.Position)) ? null : new Job(JobDefOf.Flee, intVec, danger);
		}

		private Job FleeLargeFireJob(Pawn pawn)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Fire);
			Job result;
			if (list.Count < 60)
			{
				result = null;
			}
			else
			{
				TraverseParms tp = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
				Fire closestFire = null;
				float closestDistSq = -1f;
				int firesCount = 0;
				RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (RegionEntryPredicate)((Region from, Region to) => to.Allows(tp, false)), (RegionProcessor)delegate(Region x)
				{
					List<Thing> list2 = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
					for (int i = 0; i < list2.Count; i++)
					{
						float num = (float)pawn.Position.DistanceToSquared(list2[i].Position);
						if (!(num > 400.0))
						{
							if (closestFire == null || num < closestDistSq)
							{
								closestDistSq = num;
								closestFire = (Fire)list2[i];
							}
							firesCount++;
						}
					}
					return closestDistSq <= 100.0 && firesCount >= 60;
				}, 18, RegionType.Set_Passable);
				if (closestDistSq <= 100.0 && firesCount >= 60)
				{
					Job job = this.FleeJob(pawn, closestFire);
					if (job != null)
					{
						result = job;
						goto IL_00e3;
					}
				}
				result = null;
			}
			goto IL_00e3;
			IL_00e3:
			return result;
		}
	}
}
