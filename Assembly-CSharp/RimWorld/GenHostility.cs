using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class GenHostility
	{
		public static bool HostileTo(this Thing a, Thing b)
		{
			if (!a.Destroyed && !b.Destroyed && a != b)
			{
				Pawn pawn = a as Pawn;
				Pawn pawn2 = b as Pawn;
				if (pawn != null && pawn.MentalState != null && pawn.MentalState.ForceHostileTo(b))
				{
					goto IL_0071;
				}
				if (pawn2 != null && pawn2.MentalState != null && pawn2.MentalState.ForceHostileTo(a))
					goto IL_0071;
				if (pawn != null && pawn2 != null && (GenHostility.IsPredatorHostileTo(pawn, pawn2) || GenHostility.IsPredatorHostileTo(pawn2, pawn)))
				{
					return true;
				}
				if (a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction && (pawn == null || pawn.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn2))
				{
					goto IL_0115;
				}
				if (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction && (pawn2 == null || pawn2.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn))
					goto IL_0115;
				if (a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction)
				{
					goto IL_015b;
				}
				if (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction)
					goto IL_015b;
				if (pawn != null && pawn.IsPrisoner && pawn2 != null && pawn2.IsPrisoner)
				{
					return false;
				}
				if (pawn != null && pawn2 != null)
				{
					if (pawn.IsPrisoner && pawn.HostFaction == pawn2.HostFaction && !PrisonBreakUtility.IsPrisonBreaking(pawn))
					{
						goto IL_01db;
					}
					if (pawn2.IsPrisoner && pawn2.HostFaction == pawn.HostFaction && !PrisonBreakUtility.IsPrisonBreaking(pawn2))
						goto IL_01db;
				}
				if (pawn != null && pawn2 != null)
				{
					if (pawn.HostFaction != null && pawn2.Faction != null && !pawn.HostFaction.HostileTo(pawn2.Faction) && !PrisonBreakUtility.IsPrisonBreaking(pawn))
					{
						goto IL_0257;
					}
					if (pawn2.HostFaction != null && pawn.Faction != null && !pawn2.HostFaction.HostileTo(pawn.Faction) && !PrisonBreakUtility.IsPrisonBreaking(pawn2))
						goto IL_0257;
				}
				if (a.Faction != null && b.Faction != null)
				{
					return a.Faction.HostileTo(b.Faction);
				}
				return false;
			}
			return false;
			IL_0257:
			return false;
			IL_0115:
			return true;
			IL_0071:
			return true;
			IL_015b:
			return false;
			IL_01db:
			return false;
		}

		public static bool HostileTo(this Thing t, Faction fac)
		{
			if (t.Destroyed)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				MentalState mentalState = pawn.MentalState;
				if (mentalState != null && mentalState.ForceHostileTo(fac))
				{
					return true;
				}
				if (GenHostility.IsPredatorHostileTo(pawn, fac))
				{
					return true;
				}
				if (pawn.HostFaction == fac && PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return true;
				}
				if (pawn.HostFaction == fac)
				{
					return false;
				}
				if (pawn.HostFaction != null && !pawn.HostFaction.HostileTo(fac) && !PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return false;
				}
			}
			if (t.Faction == null)
			{
				return false;
			}
			return t.Faction.HostileTo(fac);
		}

		private static bool IsPredatorHostileTo(Pawn predator, Pawn toPawn)
		{
			if (toPawn.Faction == null)
			{
				return false;
			}
			if (toPawn.Faction.HasPredatorRecentlyAttackedAnyone(predator))
			{
				return true;
			}
			Pawn preyOfMyFaction = GenHostility.GetPreyOfMyFaction(predator, toPawn.Faction);
			if (preyOfMyFaction != null && predator.Position.InHorDistOf(preyOfMyFaction.Position, 12f))
			{
				return true;
			}
			return false;
		}

		private static bool IsPredatorHostileTo(Pawn predator, Faction toFaction)
		{
			if (toFaction.HasPredatorRecentlyAttackedAnyone(predator))
			{
				return true;
			}
			if (GenHostility.GetPreyOfMyFaction(predator, toFaction) != null)
			{
				return true;
			}
			return false;
		}

		private static Pawn GetPreyOfMyFaction(Pawn predator, Faction myFaction)
		{
			Job curJob = predator.CurJob;
			if (curJob != null && curJob.def == JobDefOf.PredatorHunt && !predator.jobs.curDriver.ended)
			{
				Pawn pawn = curJob.GetTarget(TargetIndex.A).Thing as Pawn;
				if (pawn != null && pawn.Faction == myFaction)
				{
					return pawn;
				}
			}
			return null;
		}

		public static bool AnyHostileActiveThreat(Map map)
		{
			HashSet<IAttackTarget> targetsHostileToColony = map.attackTargetsCache.TargetsHostileToColony;
			HashSet<IAttackTarget>.Enumerator enumerator = targetsHostileToColony.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IAttackTarget current = enumerator.Current;
					if (GenHostility.IsActiveThreat(current))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return false;
		}

		public static bool IsActiveThreat(IAttackTarget target)
		{
			if (!(target.Thing is IAttackTargetSearcher))
			{
				return false;
			}
			if (target.ThreatDisabled())
			{
				return false;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null && (pawn.MentalStateDef == MentalStateDefOf.PanicFlee || pawn.IsPrisoner))
			{
				return false;
			}
			if (target.Thing.Spawned)
			{
				TraverseParms traverseParms = (pawn == null) ? TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false) : TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
				if (!target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms))
				{
					return false;
				}
			}
			return true;
		}

		public static void Notify_PawnLostForTutor(Pawn pawn, Map map)
		{
			if (!map.IsPlayerHome && map.mapPawns.FreeColonistsSpawnedCount != 0 && !GenHostility.AnyHostileActiveThreat(map))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ReformCaravan, OpportunityType.Important);
			}
		}
	}
}
