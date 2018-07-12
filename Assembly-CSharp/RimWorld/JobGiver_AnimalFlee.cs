using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_AnimalFlee : ThinkNode_JobGiver
	{
		private const int FleeDistance = 24;

		private const int DistToDangerToFlee = 18;

		private const int DistToFireToFlee = 10;

		private const int MinFiresNearbyToFlee = 60;

		private const int MinFiresNearbyRadius = 20;

		private const int MinFiresNearbyRegionsToScan = 18;

		private static List<Thing> tmpThings = new List<Thing>();

		public JobGiver_AnimalFlee()
		{
		}

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
				else if (pawn.GetLord() == null && (pawn.Faction != Faction.OfPlayer || !pawn.Map.IsPlayerHome))
				{
					List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
					for (int j = 0; j < potentialTargetsFor.Count; j++)
					{
						Thing thing = potentialTargetsFor[j].Thing;
						if (pawn.Position.InHorDistOf(thing.Position, 18f))
						{
							if (SelfDefenseUtility.ShouldFleeFrom(thing, pawn, false, true))
							{
								Pawn pawn2 = thing as Pawn;
								if (pawn2 == null || !pawn2.AnimalOrWildMan() || pawn2.Faction != null)
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
				}
				result = null;
			}
			return result;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static JobGiver_AnimalFlee()
		{
		}

		[CompilerGenerated]
		private sealed class <FleeLargeFireJob>c__AnonStorey0
		{
			internal TraverseParms tp;

			internal Pawn pawn;

			internal Fire closestFire;

			internal float closestDistSq;

			internal int firesCount;

			public <FleeLargeFireJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region from, Region to)
			{
				return to.Allows(this.tp, false);
			}

			internal bool <>m__1(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
				for (int i = 0; i < list.Count; i++)
				{
					float num = (float)this.pawn.Position.DistanceToSquared(list[i].Position);
					if (num <= 400f)
					{
						if (this.closestFire == null || num < this.closestDistSq)
						{
							this.closestDistSq = num;
							this.closestFire = (Fire)list[i];
						}
						this.firesCount++;
					}
				}
				return this.closestDistSq <= 100f && this.firesCount >= 60;
			}
		}
	}
}
