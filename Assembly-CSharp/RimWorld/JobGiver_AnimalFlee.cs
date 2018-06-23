using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000AA RID: 170
	public class JobGiver_AnimalFlee : ThinkNode_JobGiver
	{
		// Token: 0x04000275 RID: 629
		private const int FleeDistance = 24;

		// Token: 0x04000276 RID: 630
		private const int DistToDangerToFlee = 18;

		// Token: 0x04000277 RID: 631
		private const int DistToFireToFlee = 10;

		// Token: 0x04000278 RID: 632
		private const int MinFiresNearbyToFlee = 60;

		// Token: 0x04000279 RID: 633
		private const int MinFiresNearbyRadius = 20;

		// Token: 0x0400027A RID: 634
		private const int MinFiresNearbyRegionsToScan = 18;

		// Token: 0x0400027B RID: 635
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06000421 RID: 1057 RVA: 0x00031630 File Offset: 0x0002FA30
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
			{
				result = null;
			}
			else if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
			{
				result = null;
			}
			else
			{
				if (pawn.Faction == null)
				{
					List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
					for (int i = 0; i < list.Count; i++)
					{
						if (pawn.Position.InHorDistOf(list[i].Position, 18f))
						{
							if (SelfDefenseUtility.ShouldFleeFrom(list[i], pawn, false, false))
							{
								Job job = this.FleeJob(pawn, list[i]);
								if (job != null)
								{
									return job;
								}
							}
						}
					}
					Job job2 = this.FleeLargeFireJob(pawn);
					if (job2 != null)
					{
						return job2;
					}
				}
				else if (pawn.GetLord() == null)
				{
					List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
					for (int j = 0; j < potentialTargetsFor.Count; j++)
					{
						Thing thing = potentialTargetsFor[j].Thing;
						if (pawn.Position.InHorDistOf(thing.Position, 18f))
						{
							if (SelfDefenseUtility.ShouldFleeFrom(thing, pawn, false, true))
							{
								Job job3 = this.FleeJob(pawn, thing);
								if (job3 != null)
								{
									return job3;
								}
							}
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000317D4 File Offset: 0x0002FBD4
		private Job FleeJob(Pawn pawn, Thing danger)
		{
			IntVec3 intVec;
			if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Flee)
			{
				intVec = pawn.CurJob.targetA.Cell;
			}
			else
			{
				JobGiver_AnimalFlee.tmpThings.Clear();
				JobGiver_AnimalFlee.tmpThings.Add(danger);
				intVec = CellFinderLoose.GetFleeDest(pawn, JobGiver_AnimalFlee.tmpThings, 24f);
				JobGiver_AnimalFlee.tmpThings.Clear();
			}
			Job result;
			if (intVec != pawn.Position)
			{
				result = new Job(JobDefOf.Flee, intVec, danger);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00031880 File Offset: 0x0002FC80
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
				RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region x)
				{
					List<Thing> list2 = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
					for (int i = 0; i < list2.Count; i++)
					{
						float num = (float)pawn.Position.DistanceToSquared(list2[i].Position);
						if (num <= 400f)
						{
							if (closestFire == null || num < closestDistSq)
							{
								closestDistSq = num;
								closestFire = (Fire)list2[i];
							}
							firesCount++;
						}
					}
					return closestDistSq <= 100f && firesCount >= 60;
				}, 18, RegionType.Set_Passable);
				if (closestDistSq <= 100f && firesCount >= 60)
				{
					Job job = this.FleeJob(pawn, closestFire);
					if (job != null)
					{
						return job;
					}
				}
				result = null;
			}
			return result;
		}
	}
}
