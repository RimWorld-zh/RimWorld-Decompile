using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class RestUtility
	{
		private static List<ThingDef> bedDefsBestToWorst_RestEffectiveness;

		private static List<ThingDef> bedDefsBestToWorst_Medical;

		public static List<ThingDef> AllBedDefBestToWorst
		{
			get
			{
				return RestUtility.bedDefsBestToWorst_RestEffectiveness;
			}
		}

		public static void Reset()
		{
			RestUtility.bedDefsBestToWorst_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList();
		}

		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, bool ignoreOtherReservations = false)
		{
			Building_Bed building_Bed = bedThing as Building_Bed;
			bool result;
			if (building_Bed == null)
			{
				result = false;
				goto IL_0209;
			}
			LocalTargetInfo target = (Thing)building_Bed;
			PathEndMode peMode = PathEndMode.OnCell;
			Danger maxDanger = Danger.Some;
			int sleepingSlotsCount = building_Bed.SleepingSlotsCount;
			if (!traveler.CanReserveAndReach(target, peMode, maxDanger, sleepingSlotsCount, -1, null, ignoreOtherReservations))
			{
				result = false;
				goto IL_0209;
			}
			if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
			{
				result = false;
				goto IL_0209;
			}
			if (!building_Bed.AnyUnoccupiedSleepingSlot && (!sleeper.InBed() || sleeper.CurrentBed() != building_Bed) && !building_Bed.AssignedPawns.Contains(sleeper))
			{
				result = false;
				goto IL_0209;
			}
			if (building_Bed.IsForbidden(traveler))
			{
				result = false;
				goto IL_0209;
			}
			if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, sleeperWillBePrisoner, false))
			{
				result = false;
				goto IL_0209;
			}
			if (building_Bed.IsBurning())
			{
				result = false;
				goto IL_0209;
			}
			if (sleeperWillBePrisoner)
			{
				if (!building_Bed.ForPrisoners)
				{
					result = false;
					goto IL_0209;
				}
				if (!building_Bed.Position.IsInPrisonCell(building_Bed.Map))
				{
					result = false;
					goto IL_0209;
				}
			}
			else
			{
				if (building_Bed.Faction != traveler.Faction)
				{
					result = false;
					goto IL_0209;
				}
				if (building_Bed.ForPrisoners)
				{
					result = false;
					goto IL_0209;
				}
			}
			if (building_Bed.Medical)
			{
				if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCare(sleeper))
				{
					result = false;
					goto IL_0209;
				}
				if (!HealthAIUtility.ShouldSeekMedicalRest(sleeper))
				{
					result = false;
					goto IL_0209;
				}
			}
			else if (building_Bed.owners.Any() && !building_Bed.owners.Contains(sleeper))
			{
				if (!sleeper.IsPrisoner && !sleeperWillBePrisoner)
				{
					if (RestUtility.IsAnyOwnerLovePartnerOf(building_Bed, sleeper))
					{
						if (!building_Bed.AnyUnownedSleepingSlot)
						{
							result = false;
							goto IL_0209;
						}
						goto IL_0202;
					}
					result = false;
					goto IL_0209;
				}
				if (!building_Bed.AnyUnownedSleepingSlot)
				{
					result = false;
					goto IL_0209;
				}
			}
			goto IL_0202;
			IL_0202:
			result = true;
			goto IL_0209;
			IL_0209:
			return result;
		}

		private static bool IsAnyOwnerLovePartnerOf(Building_Bed bed, Pawn sleeper)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < bed.owners.Count)
				{
					if (LovePartnerRelationUtility.LovePartnerRelationExists(sleeper, bed.owners[num]))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, p.IsPrisoner, true, false);
		}

		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool ignoreOtherReservations = false)
		{
			Building_Bed result;
			Building_Bed building_Bed;
			if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
			{
				if (sleeper.InBed() && sleeper.CurrentBed().Medical)
				{
					Building_Bed bedThing = sleeper.CurrentBed();
					Pawn sleeper2 = sleeper;
					Pawn traveler2 = traveler;
					bool sleeperWillBePrisoner2 = sleeperWillBePrisoner;
					bool checkSocialProperness2 = checkSocialProperness;
					bool ignoreOtherReservations2 = ignoreOtherReservations;
					if (RestUtility.IsValidBedFor(bedThing, sleeper2, traveler2, sleeperWillBePrisoner2, checkSocialProperness2, false, ignoreOtherReservations2))
					{
						result = sleeper.CurrentBed();
						goto IL_0388;
					}
				}
				for (int i = 0; i < RestUtility.bedDefsBestToWorst_Medical.Count; i++)
				{
					ThingDef thingDef = RestUtility.bedDefsBestToWorst_Medical[i];
					if (RestUtility.CanUseBedEver(sleeper, thingDef))
					{
						for (int j = 0; j < 2; j++)
						{
							Danger maxDanger = (Danger)((j == 0) ? 1 : 3);
							building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)delegate(Thing b)
							{
								int result3;
								if (((Building_Bed)b).Medical && (int)b.Position.GetDangerFor(sleeper, sleeper.Map) <= (int)maxDanger)
								{
									Pawn sleeper4 = sleeper;
									Pawn traveler4 = traveler;
									bool sleeperWillBePrisoner4 = sleeperWillBePrisoner;
									bool checkSocialProperness4 = checkSocialProperness;
									bool ignoreOtherReservations4 = ignoreOtherReservations;
									result3 = (RestUtility.IsValidBedFor(b, sleeper4, traveler4, sleeperWillBePrisoner4, checkSocialProperness4, false, ignoreOtherReservations4) ? 1 : 0);
								}
								else
								{
									result3 = 0;
								}
								return (byte)result3 != 0;
							}, null, 0, -1, false, RegionType.Set_Passable, false);
							if (building_Bed != null)
								goto IL_016d;
						}
					}
				}
			}
			if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null)
			{
				Building_Bed bedThing = sleeper.ownership.OwnedBed;
				Pawn traveler2 = sleeper;
				Pawn sleeper2 = traveler;
				bool ignoreOtherReservations2 = sleeperWillBePrisoner;
				bool checkSocialProperness2 = checkSocialProperness;
				bool sleeperWillBePrisoner2 = ignoreOtherReservations;
				if (RestUtility.IsValidBedFor(bedThing, traveler2, sleeper2, ignoreOtherReservations2, checkSocialProperness2, false, sleeperWillBePrisoner2))
				{
					result = sleeper.ownership.OwnedBed;
					goto IL_0388;
				}
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null)
				{
					Building_Bed bedThing = ownedBed;
					Pawn sleeper2 = sleeper;
					Pawn traveler2 = traveler;
					bool sleeperWillBePrisoner2 = sleeperWillBePrisoner;
					bool checkSocialProperness2 = checkSocialProperness;
					bool ignoreOtherReservations2 = ignoreOtherReservations;
					if (RestUtility.IsValidBedFor(bedThing, sleeper2, traveler2, sleeperWillBePrisoner2, checkSocialProperness2, false, ignoreOtherReservations2))
					{
						result = ownedBed;
						goto IL_0388;
					}
				}
			}
			Building_Bed building_Bed2;
			for (int k = 0; k < 2; k++)
			{
				Danger maxDanger2 = (Danger)((k == 0) ? 1 : 3);
				for (int l = 0; l < RestUtility.bedDefsBestToWorst_RestEffectiveness.Count; l++)
				{
					ThingDef thingDef2 = RestUtility.bedDefsBestToWorst_RestEffectiveness[l];
					if (RestUtility.CanUseBedEver(sleeper, thingDef2))
					{
						building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef2), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)delegate(Thing b)
						{
							int result2;
							if (!((Building_Bed)b).Medical && (int)b.Position.GetDangerFor(sleeper, sleeper.Map) <= (int)maxDanger2)
							{
								Pawn sleeper3 = sleeper;
								Pawn traveler3 = traveler;
								bool sleeperWillBePrisoner3 = sleeperWillBePrisoner;
								bool checkSocialProperness3 = checkSocialProperness;
								bool ignoreOtherReservations3 = ignoreOtherReservations;
								result2 = (RestUtility.IsValidBedFor(b, sleeper3, traveler3, sleeperWillBePrisoner3, checkSocialProperness3, false, ignoreOtherReservations3) ? 1 : 0);
							}
							else
							{
								result2 = 0;
							}
							return (byte)result2 != 0;
						}, null, 0, -1, false, RegionType.Set_Passable, false);
						if (building_Bed2 != null)
							goto IL_0350;
					}
				}
			}
			result = null;
			goto IL_0388;
			IL_0350:
			result = building_Bed2;
			goto IL_0388;
			IL_0388:
			return result;
			IL_016d:
			result = building_Bed;
			goto IL_0388;
		}

		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> medBedValidator = (Predicate<Thing>)delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return (byte)((building_Bed2 != null) ? ((building_Bed2.Medical || !building_Bed2.def.building.bed_humanlike) ? (RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, pawn.IsPrisoner, false, true, false) ? 1 : 0) : 0) : 0) != 0;
			};
			Building_Bed result;
			Building_Bed building_Bed;
			if (pawn.InBed() && medBedValidator(pawn.CurrentBed()))
			{
				result = pawn.CurrentBed();
			}
			else
			{
				for (int i = 0; i < 2; i++)
				{
					Danger maxDanger = (Danger)((i == 0) ? 1 : 3);
					building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)((Thing b) => (int)b.Position.GetDangerFor(pawn, pawn.Map) <= (int)maxDanger && medBedValidator(b)), null, 0, -1, false, RegionType.Set_Passable, false);
					if (building_Bed != null)
						goto IL_00da;
				}
				result = RestUtility.FindBedFor(pawn);
			}
			goto IL_00ff;
			IL_00da:
			result = building_Bed;
			goto IL_00ff;
			IL_00ff:
			return result;
		}

		public static IntVec3 GetBedSleepingSlotPosFor(Pawn pawn, Building_Bed bed)
		{
			int num = 0;
			IntVec3 sleepingSlotPos;
			while (true)
			{
				if (num < bed.owners.Count)
				{
					if (bed.owners[num] == pawn)
					{
						sleepingSlotPos = bed.GetSleepingSlotPos(num);
						break;
					}
					num++;
					continue;
				}
				int i;
				for (i = 0; i < bed.SleepingSlotsCount; i++)
				{
					Pawn curOccupant = bed.GetCurOccupant(i);
					if ((i >= bed.owners.Count || bed.owners[i] == null) && curOccupant == pawn)
						goto IL_0077;
				}
				int j;
				for (j = 0; j < bed.SleepingSlotsCount; j++)
				{
					Pawn curOccupant2 = bed.GetCurOccupant(j);
					if ((j >= bed.owners.Count || bed.owners[j] == null) && curOccupant2 == null)
						goto IL_00d4;
				}
				Log.Error("Could not find good sleeping slot position for " + pawn + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.");
				sleepingSlotPos = bed.GetSleepingSlotPos(0);
				break;
				IL_00d4:
				sleepingSlotPos = bed.GetSleepingSlotPos(j);
				break;
				IL_0077:
				sleepingSlotPos = bed.GetSleepingSlotPos(i);
				break;
			}
			return sleepingSlotPos;
		}

		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			return (byte)((!(p.BodySize > bedDef.building.bed_maxBodySize)) ? ((p.RaceProps.Humanlike == bedDef.building.bed_humanlike) ? 1 : 0) : 0) != 0;
		}

		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			return (byte)((pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest && pawn.needs.rest.CurLevel >= 0.20000000298023224) ? 1 : 0) != 0;
		}

		public static bool DisturbancePreventsLyingDown(Pawn pawn)
		{
			return Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400;
		}

		public static float PawnHealthRestEffectivenessFactor(Pawn pawn)
		{
			return pawn.health.capacities.GetLevel(PawnCapacityDefOf.BloodPumping) * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Metabolism) * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Breathing);
		}

		public static bool Awake(this Pawn p)
		{
			return p.health.capacities.CanBeAwake && (!p.Spawned || p.CurJob == null || !p.jobs.curDriver.asleep);
		}

		public static Building_Bed CurrentBed(this Pawn p)
		{
			Building_Bed result;
			Building_Bed building_Bed;
			if (!p.Spawned || p.CurJob == null || p.jobs.curDriver.layingDown != LayingDownState.LayingInBed)
			{
				result = null;
			}
			else
			{
				building_Bed = null;
				List<Thing> thingList = p.Position.GetThingList(p.Map);
				int num = 0;
				while (num < thingList.Count)
				{
					building_Bed = (thingList[num] as Building_Bed);
					if (building_Bed == null)
					{
						num++;
						continue;
					}
					break;
				}
				if (building_Bed == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < building_Bed.SleepingSlotsCount; i++)
					{
						if (building_Bed.GetCurOccupant(i) == p)
							goto IL_009d;
					}
					result = null;
				}
			}
			goto IL_00bf;
			IL_009d:
			result = building_Bed;
			goto IL_00bf;
			IL_00bf:
			return result;
		}

		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		public static void WakeUp(Pawn p)
		{
			if (p.CurJob != null && p.jobs.curDriver.layingDown != 0 && !p.Downed)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
			}
		}

		public static float WakeThreshold(Pawn p)
		{
			Lord lord = p.GetLord();
			return (float)((lord == null || lord.CurLordToil == null || !lord.CurLordToil.CustomWakeThreshold.HasValue) ? 1.0 : lord.CurLordToil.CustomWakeThreshold.Value);
		}

		public static float FallAsleepMaxLevel(Pawn p)
		{
			return Mathf.Min(0.75f, (float)(RestUtility.WakeThreshold(p) - 0.0099999997764825821));
		}
	}
}
