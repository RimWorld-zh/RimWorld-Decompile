using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class MarriageCeremonyUtility
	{
		public static bool AcceptableGameConditionsToStartCeremony(Map map)
		{
			bool result;
			if (!MarriageCeremonyUtility.AcceptableGameConditionsToContinueCeremony(map))
			{
				result = false;
			}
			else if (GenLocalDate.HourInteger(map) < 5 || GenLocalDate.HourInteger(map) > 16)
			{
				result = false;
			}
			else if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
			{
				result = false;
			}
			else if (map.dangerWatcher.DangerRating != 0)
			{
				result = false;
			}
			else
			{
				int num = 0;
				foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
				{
					if (item.Drafted)
					{
						num++;
					}
				}
				result = ((byte)((!((float)num / (float)map.mapPawns.FreeColonistsSpawnedCount >= 0.5)) ? 1 : 0) != 0);
			}
			return result;
		}

		public static bool AcceptableGameConditionsToContinueCeremony(Map map)
		{
			return (byte)((map.dangerWatcher.DangerRating != StoryDanger.High) ? 1 : 0) != 0;
		}

		public static bool FianceReadyToStartCeremony(Pawn pawn)
		{
			return MarriageCeremonyUtility.FianceCanContinueCeremony(pawn) && !(pawn.health.hediffSet.BleedRateTotal > 0.0) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn) && !MarriageCeremonyUtility.IsCurrentlyMarryingSomeone(pawn) && pawn.GetLord() == null && !pawn.Drafted && !pawn.InMentalState && pawn.Awake() && !pawn.IsBurning() && !pawn.InBed();
		}

		public static bool FianceCanContinueCeremony(Pawn pawn)
		{
			bool result;
			if (pawn.health.hediffSet.BleedRateTotal > 0.30000001192092896)
			{
				result = false;
			}
			else if (pawn.IsPrisoner)
			{
				result = false;
			}
			else
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
				result = ((firstHediffOfDef == null || !(firstHediffOfDef.Severity > 0.20000000298023224)) && pawn.Spawned && !pawn.Downed && !pawn.InAggroMentalState);
			}
			return result;
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
			bool result;
			if (!p.Spawned)
			{
				result = false;
			}
			else
			{
				List<Lord> lords = p.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					LordJob_Joinable_MarriageCeremony lordJob_Joinable_MarriageCeremony = lords[i].LordJob as LordJob_Joinable_MarriageCeremony;
					if (lordJob_Joinable_MarriageCeremony != null && (lordJob_Joinable_MarriageCeremony.firstPawn == p || lordJob_Joinable_MarriageCeremony.secondPawn == p))
					{
						goto IL_005c;
					}
				}
				result = false;
			}
			goto IL_007b;
			IL_007b:
			return result;
			IL_005c:
			result = true;
			goto IL_007b;
		}
	}
}
