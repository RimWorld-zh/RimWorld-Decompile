using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200017B RID: 379
	public static class MarriageCeremonyUtility
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x0004CEF0 File Offset: 0x0004B2F0
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
			else if (map.dangerWatcher.DangerRating != StoryDanger.None)
			{
				result = false;
			}
			else
			{
				int num = 0;
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					if (pawn.Drafted)
					{
						num++;
					}
				}
				result = ((float)num / (float)map.mapPawns.FreeColonistsSpawnedCount < 0.5f);
			}
			return result;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0004CFE0 File Offset: 0x0004B3E0
		public static bool AcceptableGameConditionsToContinueCeremony(Map map)
		{
			return map.dangerWatcher.DangerRating != StoryDanger.High;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0004D010 File Offset: 0x0004B410
		public static bool FianceReadyToStartCeremony(Pawn pawn)
		{
			return MarriageCeremonyUtility.FianceCanContinueCeremony(pawn) && pawn.health.hediffSet.BleedRateTotal <= 0f && !HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn) && !MarriageCeremonyUtility.IsCurrentlyMarryingSomeone(pawn) && pawn.GetLord() == null && (!pawn.Drafted && !pawn.InMentalState && pawn.Awake() && !pawn.IsBurning()) && !pawn.InBed();
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0004D0D8 File Offset: 0x0004B4D8
		public static bool FianceCanContinueCeremony(Pawn pawn)
		{
			bool result;
			if (pawn.health.hediffSet.BleedRateTotal > 0.3f)
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
				result = ((firstHediffOfDef == null || firstHediffOfDef.Severity <= 0.2f) && (pawn.Spawned && !pawn.Downed) && !pawn.InAggroMentalState);
			}
			return result;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0004D178 File Offset: 0x0004B578
		public static bool ShouldGuestKeepAttendingCeremony(Pawn p)
		{
			return GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0004D194 File Offset: 0x0004B594
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
			TaleRecorder.RecordTale(TaleDefOf.Marriage, new object[]
			{
				firstPawn,
				secondPawn
			});
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0004D250 File Offset: 0x0004B650
		private static void AddNewlyMarriedThoughts(Pawn pawn, Pawn otherPawn)
		{
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.GotMarried, otherPawn);
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HoneymoonPhase, otherPawn);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0004D2A0 File Offset: 0x0004B6A0
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
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
