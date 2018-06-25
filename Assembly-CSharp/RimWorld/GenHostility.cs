using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000562 RID: 1378
	public static class GenHostility
	{
		// Token: 0x06001A01 RID: 6657 RVA: 0x000E1E34 File Offset: 0x000E0234
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
				result = ((pawn != null && pawn.MentalState != null && pawn.MentalState.ForceHostileTo(b)) || (pawn2 != null && pawn2.MentalState != null && pawn2.MentalState.ForceHostileTo(a)) || (pawn != null && pawn2 != null && (GenHostility.IsPredatorHostileTo(pawn, pawn2) || GenHostility.IsPredatorHostileTo(pawn2, pawn))) || ((a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction && (pawn == null || pawn.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn2)) || (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction && (pawn2 == null || pawn2.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn))) || ((a.Faction == null || pawn2 == null || pawn2.HostFaction != a.Faction) && (b.Faction == null || pawn == null || pawn.HostFaction != b.Faction) && (pawn == null || !pawn.IsPrisoner || pawn2 == null || !pawn2.IsPrisoner) && (pawn == null || pawn2 == null || ((!pawn.IsPrisoner || pawn.HostFaction != pawn2.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (!pawn2.IsPrisoner || pawn2.HostFaction != pawn.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && (pawn == null || pawn2 == null || ((pawn.HostFaction == null || pawn2.Faction == null || pawn.HostFaction.HostileTo(pawn2.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (pawn2.HostFaction == null || pawn.Faction == null || pawn2.HostFaction.HostileTo(pawn.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && a.Faction != null && b.Faction != null && a.Faction.HostileTo(b.Faction)));
			}
			return result;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x000E2100 File Offset: 0x000E0500
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
				result = (t.Faction != null && t.Faction.HostileTo(fac));
			}
			return result;
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x000E21E8 File Offset: 0x000E05E8
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
				result = (preyOfMyFaction != null && predator.Position.InHorDistOf(preyOfMyFaction.Position, 12f));
			}
			return result;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000E2260 File Offset: 0x000E0660
		private static bool IsPredatorHostileTo(Pawn predator, Faction toFaction)
		{
			return toFaction.HasPredatorRecentlyAttackedAnyone(predator) || GenHostility.GetPreyOfMyFaction(predator, toFaction) != null;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000E229C File Offset: 0x000E069C
		private static Pawn GetPreyOfMyFaction(Pawn predator, Faction myFaction)
		{
			Job curJob = predator.CurJob;
			if (curJob != null && curJob.def == JobDefOf.PredatorHunt && !predator.jobs.curDriver.ended)
			{
				Pawn pawn = curJob.GetTarget(TargetIndex.A).Thing as Pawn;
				if (pawn != null && !pawn.Dead && pawn.Faction == myFaction)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000E2320 File Offset: 0x000E0720
		public static bool AnyHostileActiveThreatToPlayer(Map map)
		{
			return GenHostility.AnyHostileActiveThreatTo(map, Faction.OfPlayer);
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x000E2340 File Offset: 0x000E0740
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction)
		{
			HashSet<IAttackTarget> hashSet = map.attackTargetsCache.TargetsHostileToFaction(faction);
			foreach (IAttackTarget target in hashSet)
			{
				if (GenHostility.IsActiveThreatTo(target, faction))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x000E23BC File Offset: 0x000E07BC
		public static bool IsActiveThreatToPlayer(IAttackTarget target)
		{
			return GenHostility.IsActiveThreatTo(target, Faction.OfPlayer);
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000E23DC File Offset: 0x000E07DC
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
			else if (target.ThreatDisabled(null))
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
						return false;
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
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x000E2529 File Offset: 0x000E0929
		public static void Notify_PawnLostForTutor(Pawn pawn, Map map)
		{
			if (!map.IsPlayerHome && map.mapPawns.FreeColonistsSpawnedCount != 0 && !GenHostility.AnyHostileActiveThreatToPlayer(map))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ReformCaravan, OpportunityType.Important);
			}
		}
	}
}
