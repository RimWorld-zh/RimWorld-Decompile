using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class MarriageCeremonyUtility
	{
		public static bool AcceptableGameConditionsToStartCeremony(Map map)
		{
			if (!MarriageCeremonyUtility.AcceptableGameConditionsToContinueCeremony(map))
			{
				return false;
			}
			if (GenLocalDate.HourInteger(map) >= 5 && GenLocalDate.HourInteger(map) <= 16)
			{
				if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
				{
					return false;
				}
				if (map.dangerWatcher.DangerRating != 0)
				{
					return false;
				}
				int num = 0;
				foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
				{
					if (item.Drafted)
					{
						num++;
					}
				}
				if ((float)num / (float)map.mapPawns.FreeColonistsSpawnedCount >= 0.5)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool AcceptableGameConditionsToContinueCeremony(Map map)
		{
			if (map.dangerWatcher.DangerRating == StoryDanger.High)
			{
				return false;
			}
			return true;
		}

		public static bool FianceReadyToStartCeremony(Pawn pawn)
		{
			if (!MarriageCeremonyUtility.FianceCanContinueCeremony(pawn))
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.0)
			{
				return false;
			}
			if (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
			{
				return false;
			}
			if (PawnUtility.WillSoonHaveBasicNeed(pawn))
			{
				return false;
			}
			if (MarriageCeremonyUtility.IsCurrentlyMarryingSomeone(pawn))
			{
				return false;
			}
			if (pawn.GetLord() != null)
			{
				return false;
			}
			return !pawn.Drafted && !pawn.InMentalState && pawn.Awake() && !pawn.IsBurning() && !pawn.InBed();
		}

		public static bool FianceCanContinueCeremony(Pawn pawn)
		{
			if (pawn.health.hediffSet.BleedRateTotal > 0.30000001192092896)
			{
				return false;
			}
			if (pawn.IsPrisoner)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.20000000298023224)
			{
				return false;
			}
			return pawn.Spawned && !pawn.Downed && !pawn.InAggroMentalState;
		}

		public static bool ShouldGuestKeepAttendingCeremony(Pawn p)
		{
			return GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		public static void Married(Pawn firstPawn, Pawn secondPawn)
		{
			LovePartnerRelationUtility.ChangeSpouseRelationsToExSpouse(firstPawn);
			LovePartnerRelationUtility.ChangeSpouseRelationsToExSpouse(secondPawn);
			firstPawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, secondPawn);
			firstPawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.ExSpouse, secondPawn);
			firstPawn.relations.AddDirectRelation(PawnRelationDefOf.Spouse, secondPawn);
			MarriageCeremonyUtility.AddNewlyMarriedThoughts(firstPawn, secondPawn);
			MarriageCeremonyUtility.AddNewlyMarriedThoughts(secondPawn, firstPawn);
			firstPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.DivorcedMe, secondPawn);
			secondPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.DivorcedMe, firstPawn);
			LovePartnerRelationUtility.TryToShareBed(firstPawn, secondPawn);
			TaleRecorder.RecordTale(TaleDefOf.Marriage, firstPawn, secondPawn);
		}

		private static void AddNewlyMarriedThoughts(Pawn pawn, Pawn otherPawn)
		{
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.GotMarried, otherPawn);
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HoneymoonPhase, otherPawn);
		}

		private static bool IsCurrentlyMarryingSomeone(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Lord> lords = p.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_Joinable_MarriageCeremony lordJob_Joinable_MarriageCeremony = lords[i].LordJob as LordJob_Joinable_MarriageCeremony;
				if (lordJob_Joinable_MarriageCeremony != null && (lordJob_Joinable_MarriageCeremony.firstPawn == p || lordJob_Joinable_MarriageCeremony.secondPawn == p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
