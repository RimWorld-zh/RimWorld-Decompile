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
			Job result;
			if (pawn.playerSettings == null || !pawn.playerSettings.UsesConfigurableHostilityResponse)
			{
				result = null;
			}
			else if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				result = null;
			}
			else
			{
				switch (pawn.playerSettings.hostilityResponse)
				{
				case HostilityResponseMode.Ignore:
					result = null;
					break;
				case HostilityResponseMode.Attack:
					result = this.TryGetAttackNearbyEnemyJob(pawn);
					break;
				case HostilityResponseMode.Flee:
					result = this.TryGetFleeJob(pawn);
					break;
				default:
					result = null;
					break;
				}
			}
			return result;
		}

		private Job TryGetAttackNearbyEnemyJob(Pawn pawn)
		{
			Job result;
			if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = null;
			}
			else
			{
				bool flag = pawn.equipment.Primary == null || pawn.equipment.Primary.def.IsMeleeWeapon;
				float num = 8f;
				if (!flag)
				{
					num = Mathf.Clamp(pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.range * 0.66f, 2f, 20f);
				}
				TargetScanFlags flags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat;
				float maxDist = num;
				Thing thing = (Thing)AttackTargetFinder.BestAttackTarget(pawn, flags, null, 0f, maxDist, default(IntVec3), float.MaxValue, false);
				if (thing == null)
				{
					result = null;
				}
				else if (flag || pawn.CanReachImmediate(thing, PathEndMode.Touch))
				{
					result = new Job(JobDefOf.AttackMelee, thing);
				}
				else
				{
					result = new Job(JobDefOf.AttackStatic, thing)
					{
						maxNumStaticAttacks = 2,
						expiryInterval = 2000,
						endIfCantShootTargetFromCurPos = true
					};
				}
			}
			return result;
		}

		private Job TryGetFleeJob(Pawn pawn)
		{
			Job result;
			if (!SelfDefenseUtility.ShouldStartFleeing(pawn))
			{
				result = null;
			}
			else
			{
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
						RegionTraverser.BreadthFirstTraverse(region, (Region from, Region reg) => reg.portal == null || reg.portal.Open, delegate(Region reg)
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
				result = new Job(JobDefOf.FleeAndCower, c);
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JobGiver_ConfigurableHostilityResponse()
		{
		}

		[CompilerGenerated]
		private static bool <TryGetFleeJob>m__0(Region from, Region reg)
		{
			return reg.portal == null || reg.portal.Open;
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
