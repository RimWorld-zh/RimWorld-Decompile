using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001B3 RID: 435
	public static class RestUtility
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x000544A8 File Offset: 0x000528A8
		public static List<ThingDef> AllBedDefBestToWorst
		{
			get
			{
				return RestUtility.bedDefsBestToWorst_RestEffectiveness;
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x000544C4 File Offset: 0x000528C4
		public static void Reset()
		{
			RestUtility.bedDefsBestToWorst_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000545E0 File Offset: 0x000529E0
		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, bool ignoreOtherReservations = false)
		{
			Building_Bed building_Bed = bedThing as Building_Bed;
			bool result;
			if (building_Bed == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = building_Bed;
				PathEndMode peMode = PathEndMode.OnCell;
				Danger maxDanger = Danger.Some;
				int sleepingSlotsCount = building_Bed.SleepingSlotsCount;
				if (!traveler.CanReserveAndReach(target, peMode, maxDanger, sleepingSlotsCount, -1, null, ignoreOtherReservations))
				{
					result = false;
				}
				else if (traveler.HasReserved(building_Bed, new LocalTargetInfo?(sleeper), null, null))
				{
					result = false;
				}
				else if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
				{
					result = false;
				}
				else if (!building_Bed.AnyUnoccupiedSleepingSlot && (!sleeper.InBed() || sleeper.CurrentBed() != building_Bed) && !building_Bed.AssignedPawns.Contains(sleeper))
				{
					result = false;
				}
				else if (building_Bed.IsForbidden(traveler))
				{
					result = false;
				}
				else if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, sleeperWillBePrisoner, false))
				{
					result = false;
				}
				else if (building_Bed.IsBurning())
				{
					result = false;
				}
				else
				{
					if (sleeperWillBePrisoner)
					{
						if (!building_Bed.ForPrisoners)
						{
							return false;
						}
						if (!building_Bed.Position.IsInPrisonCell(building_Bed.Map))
						{
							return false;
						}
					}
					else
					{
						if (building_Bed.Faction != traveler.Faction)
						{
							return false;
						}
						if (building_Bed.ForPrisoners)
						{
							return false;
						}
					}
					if (building_Bed.Medical)
					{
						if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(sleeper))
						{
							return false;
						}
						if (!HealthAIUtility.ShouldSeekMedicalRest(sleeper))
						{
							return false;
						}
					}
					else if (building_Bed.owners.Any<Pawn>() && !building_Bed.owners.Contains(sleeper))
					{
						if (sleeper.IsPrisoner || sleeperWillBePrisoner)
						{
							if (!building_Bed.AnyUnownedSleepingSlot)
							{
								return false;
							}
						}
						else
						{
							if (!RestUtility.IsAnyOwnerLovePartnerOf(building_Bed, sleeper))
							{
								return false;
							}
							if (!building_Bed.AnyUnownedSleepingSlot)
							{
								return false;
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00054830 File Offset: 0x00052C30
		private static bool IsAnyOwnerLovePartnerOf(Building_Bed bed, Pawn sleeper)
		{
			for (int i = 0; i < bed.owners.Count; i++)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(sleeper, bed.owners[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00054884 File Offset: 0x00052C84
		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, p.IsPrisoner, true, false);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x000548A8 File Offset: 0x00052CA8
		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool ignoreOtherReservations = false)
		{
			if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
			{
				if (sleeper.InBed() && sleeper.CurrentBed().Medical)
				{
					Building_Bed bedThing = sleeper.CurrentBed();
					Pawn pawn = sleeper;
					Pawn pawn2 = traveler;
					bool flag = sleeperWillBePrisoner;
					bool checkSocialProperness2 = checkSocialProperness;
					bool flag2 = ignoreOtherReservations;
					if (RestUtility.IsValidBedFor(bedThing, pawn, pawn2, flag, checkSocialProperness2, false, flag2))
					{
						return sleeper.CurrentBed();
					}
				}
				for (int i = 0; i < RestUtility.bedDefsBestToWorst_Medical.Count; i++)
				{
					ThingDef thingDef = RestUtility.bedDefsBestToWorst_Medical[i];
					if (RestUtility.CanUseBedEver(sleeper, thingDef))
					{
						for (int j = 0; j < 2; j++)
						{
							Danger maxDanger = (j != 0) ? Danger.Deadly : Danger.None;
							Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing b)
							{
								bool result;
								if (((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger)
								{
									Pawn sleeper2 = sleeper;
									Pawn traveler2 = traveler;
									bool sleeperWillBePrisoner2 = sleeperWillBePrisoner;
									bool checkSocialProperness3 = checkSocialProperness;
									bool ignoreOtherReservations2 = ignoreOtherReservations;
									result = RestUtility.IsValidBedFor(b, sleeper2, traveler2, sleeperWillBePrisoner2, checkSocialProperness3, false, ignoreOtherReservations2);
								}
								else
								{
									result = false;
								}
								return result;
							}, null, 0, -1, false, RegionType.Set_Passable, false);
							if (building_Bed != null)
							{
								return building_Bed;
							}
						}
					}
				}
			}
			if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null)
			{
				Building_Bed bedThing = sleeper.ownership.OwnedBed;
				Pawn pawn2 = sleeper;
				Pawn pawn = traveler;
				bool flag2 = sleeperWillBePrisoner;
				bool checkSocialProperness2 = checkSocialProperness;
				bool flag = ignoreOtherReservations;
				if (RestUtility.IsValidBedFor(bedThing, pawn2, pawn, flag2, checkSocialProperness2, false, flag))
				{
					return sleeper.ownership.OwnedBed;
				}
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null)
				{
					Building_Bed bedThing = ownedBed;
					Pawn pawn = sleeper;
					Pawn pawn2 = traveler;
					bool flag = sleeperWillBePrisoner;
					bool checkSocialProperness2 = checkSocialProperness;
					bool flag2 = ignoreOtherReservations;
					if (RestUtility.IsValidBedFor(bedThing, pawn, pawn2, flag, checkSocialProperness2, false, flag2))
					{
						return ownedBed;
					}
				}
			}
			for (int k = 0; k < 2; k++)
			{
				Danger maxDanger = (k != 0) ? Danger.Deadly : Danger.None;
				for (int l = 0; l < RestUtility.bedDefsBestToWorst_RestEffectiveness.Count; l++)
				{
					ThingDef thingDef2 = RestUtility.bedDefsBestToWorst_RestEffectiveness[l];
					if (RestUtility.CanUseBedEver(sleeper, thingDef2))
					{
						Building_Bed building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef2), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing b)
						{
							bool result;
							if (!((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger)
							{
								Pawn sleeper2 = sleeper;
								Pawn traveler2 = traveler;
								bool sleeperWillBePrisoner2 = sleeperWillBePrisoner;
								bool checkSocialProperness3 = checkSocialProperness;
								bool ignoreOtherReservations2 = ignoreOtherReservations;
								result = RestUtility.IsValidBedFor(b, sleeper2, traveler2, sleeperWillBePrisoner2, checkSocialProperness3, false, ignoreOtherReservations2);
							}
							else
							{
								result = false;
							}
							return result;
						}, null, 0, -1, false, RegionType.Set_Passable, false);
						if (building_Bed2 != null)
						{
							return building_Bed2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00054C40 File Offset: 0x00053040
		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> medBedValidator = delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return building_Bed2 != null && (building_Bed2.Medical || !building_Bed2.def.building.bed_humanlike) && RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, pawn.IsPrisoner, false, true, false);
			};
			Building_Bed result;
			if (pawn.InBed() && medBedValidator(pawn.CurrentBed()))
			{
				result = pawn.CurrentBed();
			}
			else
			{
				for (int i = 0; i < 2; i++)
				{
					Danger maxDanger = (i != 0) ? Danger.Deadly : Danger.None;
					Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing b) => b.Position.GetDangerFor(pawn, pawn.Map) <= maxDanger && medBedValidator(b), null, 0, -1, false, RegionType.Set_Passable, false);
					if (building_Bed != null)
					{
						return building_Bed;
					}
				}
				result = RestUtility.FindBedFor(pawn);
			}
			return result;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00054D50 File Offset: 0x00053150
		public static IntVec3 GetBedSleepingSlotPosFor(Pawn pawn, Building_Bed bed)
		{
			for (int i = 0; i < bed.owners.Count; i++)
			{
				if (bed.owners[i] == pawn)
				{
					return bed.GetSleepingSlotPos(i);
				}
			}
			for (int j = 0; j < bed.SleepingSlotsCount; j++)
			{
				Pawn curOccupant = bed.GetCurOccupant(j);
				if ((j >= bed.owners.Count || bed.owners[j] == null) && curOccupant == pawn)
				{
					return bed.GetSleepingSlotPos(j);
				}
			}
			for (int k = 0; k < bed.SleepingSlotsCount; k++)
			{
				Pawn curOccupant2 = bed.GetCurOccupant(k);
				if ((k >= bed.owners.Count || bed.owners[k] == null) && curOccupant2 == null)
				{
					return bed.GetSleepingSlotPos(k);
				}
			}
			Log.Error("Could not find good sleeping slot position for " + pawn + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.", false);
			return bed.GetSleepingSlotPos(0);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00054E78 File Offset: 0x00053278
		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			return p.BodySize <= bedDef.building.bed_maxBodySize && p.RaceProps.Humanlike == bedDef.building.bed_humanlike;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00054ED0 File Offset: 0x000532D0
		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			return pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest && pawn.needs.rest.CurLevel >= 0.2f;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00054F28 File Offset: 0x00053328
		public static bool DisturbancePreventsLyingDown(Pawn pawn)
		{
			return !pawn.Downed && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00054F6C File Offset: 0x0005336C
		public static float PawnHealthRestEffectivenessFactor(Pawn pawn)
		{
			return pawn.health.capacities.GetLevel(PawnCapacityDefOf.BloodPumping) * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Metabolism) * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Breathing);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00054FC4 File Offset: 0x000533C4
		public static bool Awake(this Pawn p)
		{
			return p.health.capacities.CanBeAwake && (!p.Spawned || p.CurJob == null || p.jobs.curDriver == null || !p.jobs.curDriver.asleep);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00055038 File Offset: 0x00053438
		public static Building_Bed CurrentBed(this Pawn p)
		{
			Building_Bed result;
			if (!p.Spawned || p.CurJob == null || p.GetPosture() != PawnPosture.LayingInBed)
			{
				result = null;
			}
			else
			{
				Building_Bed building_Bed = null;
				List<Thing> thingList = p.Position.GetThingList(p.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					building_Bed = (thingList[i] as Building_Bed);
					if (building_Bed != null)
					{
						break;
					}
				}
				if (building_Bed == null)
				{
					result = null;
				}
				else
				{
					for (int j = 0; j < building_Bed.SleepingSlotsCount; j++)
					{
						if (building_Bed.GetCurOccupant(j) == p)
						{
							return building_Bed;
						}
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x000550FC File Offset: 0x000534FC
		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0005511D File Offset: 0x0005351D
		public static void WakeUp(Pawn p)
		{
			if (p.CurJob != null && p.GetPosture().Laying() && !p.Downed)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
			}
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00055154 File Offset: 0x00053554
		public static float WakeThreshold(Pawn p)
		{
			Lord lord = p.GetLord();
			float result;
			if (lord != null && lord.CurLordToil != null && lord.CurLordToil.CustomWakeThreshold != null)
			{
				result = lord.CurLordToil.CustomWakeThreshold.Value;
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x000551B8 File Offset: 0x000535B8
		public static float FallAsleepMaxLevel(Pawn p)
		{
			return Mathf.Min(0.75f, RestUtility.WakeThreshold(p) - 0.01f);
		}

		// Token: 0x040003CF RID: 975
		private static List<ThingDef> bedDefsBestToWorst_RestEffectiveness;

		// Token: 0x040003D0 RID: 976
		private static List<ThingDef> bedDefsBestToWorst_Medical;
	}
}
