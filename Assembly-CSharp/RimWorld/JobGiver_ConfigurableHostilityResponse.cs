using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_ConfigurableHostilityResponse : ThinkNode_JobGiver
	{
		private static List<Thing> tmpThreats = new List<Thing>();

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
				{
					result = null;
					break;
				}
				case HostilityResponseMode.Attack:
				{
					result = this.TryGetAttackNearbyEnemyJob(pawn);
					break;
				}
				case HostilityResponseMode.Flee:
				{
					result = this.TryGetFleeJob(pawn);
					break;
				}
				default:
				{
					result = null;
					break;
				}
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
					num = Mathf.Clamp((float)(pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.range * 0.6600000262260437), 2f, 20f);
				}
				TargetScanFlags flags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat;
				float maxDist = num;
				Thing thing = (Thing)AttackTargetFinder.BestAttackTarget(pawn, flags, null, 0f, maxDist, default(IntVec3), 3.40282347E+38f, false);
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
					Job job = new Job(JobDefOf.AttackStatic, thing);
					job.maxNumStaticAttacks = 2;
					job.expiryInterval = 2000;
					job.endIfCantShootTargetFromCurPos = true;
					result = job;
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
					if (!JobGiver_ConfigurableHostilityResponse.tmpThreats.Any())
					{
						Log.Warning(pawn.LabelShort + " decided to flee but there is no any threat around.");
						result = null;
						goto IL_015e;
					}
					c = CellFinderLoose.GetFleeDest(pawn, JobGiver_ConfigurableHostilityResponse.tmpThreats, 23f);
					JobGiver_ConfigurableHostilityResponse.tmpThreats.Clear();
				}
				result = new Job(JobDefOf.FleeAndCower, c);
			}
			goto IL_015e;
			IL_015e:
			return result;
		}
	}
}
