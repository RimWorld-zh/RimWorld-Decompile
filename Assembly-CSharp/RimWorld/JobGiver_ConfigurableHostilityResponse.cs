using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_ConfigurableHostilityResponse : ThinkNode_JobGiver
	{
		private static List<Thing> tmpThreats = new List<Thing>();

		[CompilerGenerated]
		private static RegionEntryPredicate <>f__am$cache0;

		public JobGiver_ConfigurableHostilityResponse()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.playerSettings == null || !pawn.playerSettings.UsesConfigurableHostilityResponse)
			{
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				return null;
			case HostilityResponseMode.Attack:
				return this.TryGetAttackNearbyEnemyJob(pawn);
			case HostilityResponseMode.Flee:
				return this.TryGetFleeJob(pawn);
			default:
				return null;
			}
		}

		private Job TryGetAttackNearbyEnemyJob(Pawn pawn)
		{
			if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			bool isMeleeAttack = pawn.CurrentEffectiveVerb.IsMeleeAttack;
			float num = 8f;
			if (!isMeleeAttack)
			{
				num = Mathf.Clamp(pawn.CurrentEffectiveVerb.verbProps.range * 0.66f, 2f, 20f);
			}
			TargetScanFlags flags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat;
			float maxDist = num;
			Thing thing = (Thing)AttackTargetFinder.BestAttackTarget(pawn, flags, null, 0f, maxDist, default(IntVec3), float.MaxValue, false, true);
			if (thing == null)
			{
				return null;
			}
			if (isMeleeAttack || pawn.CanReachImmediate(thing, PathEndMode.Touch))
			{
				return new Job(JobDefOf.AttackMelee, thing);
			}
			return new Job(JobDefOf.AttackStatic, thing)
			{
				maxNumStaticAttacks = 2,
				expiryInterval = 2000,
				endIfCantShootTargetFromCurPos = true
			};
		}

		private Job TryGetFleeJob(Pawn pawn)
		{
			if (!SelfDefenseUtility.ShouldStartFleeing(pawn))
			{
				return null;
			}
			IntVec3 c;
			if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.FleeAndCower)
			{
				c = pawn.CurJob.targetA.Cell;
			}
			else
			{
				JobGiver_ConfigurableHostilityResponse.tmpThreats.Clear();
				List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
				for (int i = 0; i < potentialTargetsFor.Count; i++)
				{
					Thing thing = potentialTargetsFor[i].Thing;
					if (SelfDefenseUtility.ShouldFleeFrom(thing, pawn, false, false))
					{
						JobGiver_ConfigurableHostilityResponse.tmpThreats.Add(thing);
					}
				}
				List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing2 = list[j];
					if (SelfDefenseUtility.ShouldFleeFrom(thing2, pawn, false, false))
					{
						JobGiver_ConfigurableHostilityResponse.tmpThreats.Add(thing2);
					}
				}
				if (!JobGiver_ConfigurableHostilityResponse.tmpThreats.Any<Thing>())
				{
					Log.Error(pawn.LabelShort + " decided to flee but there is not any threat around.", false);
					Region region = pawn.GetRegion(RegionType.Set_Passable);
					if (region == null)
					{
						return null;
					}
					RegionTraverser.BreadthFirstTraverse(region, (Region from, Region reg) => reg.door == null || reg.door.Open, delegate(Region reg)
					{
						List<Thing> list2 = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
						for (int k = 0; k < list2.Count; k++)
						{
							Thing thing3 = list2[k];
							if (SelfDefenseUtility.ShouldFleeFrom(thing3, pawn, false, false))
							{
								JobGiver_ConfigurableHostilityResponse.tmpThreats.Add(thing3);
								Log.Warning(string.Format("  Found a viable threat {0}; tests are {1}, {2}, {3}", new object[]
								{
									thing3.LabelShort,
									thing3.Map.attackTargetsCache.Debug_CheckIfInAllTargets(thing3 as IAttackTarget),
									thing3.Map.attackTargetsCache.Debug_CheckIfHostileToFaction(pawn.Faction, thing3 as IAttackTarget),
									thing3 is IAttackTarget
								}), false);
							}
						}
						return false;
					}, 9, RegionType.Set_Passable);
					if (!JobGiver_ConfigurableHostilityResponse.tmpThreats.Any<Thing>())
					{
						return null;
					}
				}
				c = CellFinderLoose.GetFleeDest(pawn, JobGiver_ConfigurableHostilityResponse.tmpThreats, 23f);
				JobGiver_ConfigurableHostilityResponse.tmpThreats.Clear();
			}
			return new Job(JobDefOf.FleeAndCower, c);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JobGiver_ConfigurableHostilityResponse()
		{
		}

		[CompilerGenerated]
		private static bool <TryGetFleeJob>m__0(Region from, Region reg)
		{
			return reg.door == null || reg.door.Open;
		}

		[CompilerGenerated]
		private sealed class <TryGetFleeJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGetFleeJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region reg)
			{
				List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (SelfDefenseUtility.ShouldFleeFrom(thing, this.pawn, false, false))
					{
						JobGiver_ConfigurableHostilityResponse.tmpThreats.Add(thing);
						Log.Warning(string.Format("  Found a viable threat {0}; tests are {1}, {2}, {3}", new object[]
						{
							thing.LabelShort,
							thing.Map.attackTargetsCache.Debug_CheckIfInAllTargets(thing as IAttackTarget),
							thing.Map.attackTargetsCache.Debug_CheckIfHostileToFaction(this.pawn.Faction, thing as IAttackTarget),
							thing is IAttackTarget
						}), false);
					}
				}
				return false;
			}
		}
	}
}
