using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class GenHostility
	{
		public static bool HostileTo(this Thing a, Thing b)
		{
			bool result;
			if (a.Destroyed || b.Destroyed || a == b)
			{
				result = false;
			}
			else
			{
				Pawn pawn = a as Pawn;
				Pawn pawn2 = b as Pawn;
				if (pawn != null && pawn.MentalState != null && pawn.MentalState.ForceHostileTo(b))
				{
					goto IL_0077;
				}
				if (pawn2 != null && pawn2.MentalState != null && pawn2.MentalState.ForceHostileTo(a))
					goto IL_0077;
				if (pawn != null && pawn2 != null && (GenHostility.IsPredatorHostileTo(pawn, pawn2) || GenHostility.IsPredatorHostileTo(pawn2, pawn)))
				{
					result = true;
				}
				else
				{
					if (a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction && (pawn == null || pawn.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn2))
					{
						goto IL_0127;
					}
					if (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction && (pawn2 == null || pawn2.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn))
						goto IL_0127;
					if (a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction)
					{
						goto IL_0173;
					}
					if (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction)
						goto IL_0173;
					if (pawn != null && pawn.IsPrisoner && pawn2 != null && pawn2.IsPrisoner)
					{
						result = false;
					}
					else
					{
						if (pawn != null && pawn2 != null)
						{
							if (pawn.IsPrisoner && pawn.HostFaction == pawn2.HostFaction && !PrisonBreakUtility.IsPrisonBreaking(pawn))
							{
								goto IL_01ff;
							}
							if (pawn2.IsPrisoner && pawn2.HostFaction == pawn.HostFaction && !PrisonBreakUtility.IsPrisonBreaking(pawn2))
								goto IL_01ff;
						}
						if (pawn != null && pawn2 != null)
						{
							if (pawn.HostFaction != null && pawn2.Faction != null && !pawn.HostFaction.HostileTo(pawn2.Faction) && !PrisonBreakUtility.IsPrisonBreaking(pawn))
							{
								goto IL_0281;
							}
							if (pawn2.HostFaction != null && pawn.Faction != null && !pawn2.HostFaction.HostileTo(pawn.Faction) && !PrisonBreakUtility.IsPrisonBreaking(pawn2))
								goto IL_0281;
						}
						result = (a.Faction != null && b.Faction != null && a.Faction.HostileTo(b.Faction));
					}
				}
			}
			goto IL_02bd;
			IL_02bd:
			return result;
			IL_0077:
			result = true;
			goto IL_02bd;
			IL_01ff:
			result = false;
			goto IL_02bd;
			IL_0127:
			result = true;
			goto IL_02bd;
			IL_0173:
			result = false;
			goto IL_02bd;
			IL_0281:
			result = false;
			goto IL_02bd;
		}

		public static bool HostileTo(this Thing t, Faction fac)
		{
			bool result;
			if (t.Destroyed)
			{
				result = false;
			}
			else
			{
				Pawn pawn = t as Pawn;
				if (pawn != null)
				{
					MentalState mentalState = pawn.MentalState;
					if (mentalState != null && mentalState.ForceHostileTo(fac))
					{
						result = true;
						goto IL_00d8;
					}
					if (GenHostility.IsPredatorHostileTo(pawn, fac))
					{
						result = true;
						goto IL_00d8;
					}
					if (pawn.HostFaction == fac && PrisonBreakUtility.IsPrisonBreaking(pawn))
					{
						result = true;
						goto IL_00d8;
					}
					if (pawn.HostFaction == fac)
					{
						result = false;
						goto IL_00d8;
					}
					if (pawn.HostFaction != null && !pawn.HostFaction.HostileTo(fac) && !PrisonBreakUtility.IsPrisonBreaking(pawn))
					{
						result = false;
						goto IL_00d8;
					}
				}
				result = (t.Faction != null && t.Faction.HostileTo(fac));
			}
			goto IL_00d8;
			IL_00d8:
			return result;
		}

		private static bool IsPredatorHostileTo(Pawn predator, Pawn toPawn)
		{
			bool result;
			if (toPawn.Faction == null)
			{
				result = false;
			}
			else if (toPawn.Faction.HasPredatorRecentlyAttackedAnyone(predator))
			{
				result = true;
			}
			else
			{
				Pawn preyOfMyFaction = GenHostility.GetPreyOfMyFaction(predator, toPawn.Faction);
				result = ((byte)((preyOfMyFaction != null && predator.Position.InHorDistOf(preyOfMyFaction.Position, 12f)) ? 1 : 0) != 0);
			}
			return result;
		}

		private static bool IsPredatorHostileTo(Pawn predator, Faction toFaction)
		{
			return (byte)(toFaction.HasPredatorRecentlyAttackedAnyone(predator) ? 1 : ((GenHostility.GetPreyOfMyFaction(predator, toFaction) != null) ? 1 : 0)) != 0;
		}

		private static Pawn GetPreyOfMyFaction(Pawn predator, Faction myFaction)
		{
			Job curJob = predator.CurJob;
			Pawn result;
			if (curJob != null && curJob.def == JobDefOf.PredatorHunt && !predator.jobs.curDriver.ended)
			{
				Pawn pawn = curJob.GetTarget(TargetIndex.A).Thing as Pawn;
				if (pawn != null && pawn.Faction == myFaction)
				{
					result = pawn;
					goto IL_006a;
				}
			}
			result = null;
			goto IL_006a;
			IL_006a:
			return result;
		}

		public static bool AnyHostileActiveThreatToPlayer(Map map)
		{
			return GenHostility.AnyHostileActiveThreatTo(map, Faction.OfPlayer);
		}

		public static bool AnyHostileActiveThreatTo(Map map, Faction faction)
		{
			HashSet<IAttackTarget> hashSet = map.attackTargetsCache.TargetsHostileToFaction(faction);
			foreach (IAttackTarget item in hashSet)
			{
				if (GenHostility.IsActiveThreatTo(item, faction))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsActiveThreatToPlayer(IAttackTarget target)
		{
			return GenHostility.IsActiveThreatTo(target, Faction.OfPlayer);
		}

		public static bool IsActiveThreatTo(IAttackTarget target, Faction faction)
		{
			bool result;
			if (!target.Thing.HostileTo(faction))
			{
				result = false;
			}
			else if (!(target.Thing is IAttackTargetSearcher))
			{
				result = false;
			}
			else if (target.ThreatDisabled())
			{
				result = false;
			}
			else
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					Lord lord = pawn.GetLord();
					if (lord != null && lord.LordJob is LordJob_DefendAndExpandHive && (pawn.mindState.duty == null || pawn.mindState.duty.def != DutyDefOf.AssaultColony))
					{
						result = false;
						goto IL_013e;
					}
				}
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null && (pawn2.MentalStateDef == MentalStateDefOf.PanicFlee || pawn2.IsPrisoner))
				{
					result = false;
				}
				else
				{
					if (target.Thing.Spawned)
					{
						TraverseParms traverseParms = (pawn2 == null) ? TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false) : TraverseParms.For(pawn2, Danger.Deadly, TraverseMode.ByPawn, false);
						if (!target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms))
						{
							result = false;
							goto IL_013e;
						}
					}
					result = true;
				}
			}
			goto IL_013e;
			IL_013e:
			return result;
		}

		public static void Notify_PawnLostForTutor(Pawn pawn, Map map)
		{
			if (!map.IsPlayerHome && map.mapPawns.FreeColonistsSpawnedCount != 0 && !GenHostility.AnyHostileActiveThreatToPlayer(map))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ReformCaravan, OpportunityType.Important);
			}
		}
	}
}
