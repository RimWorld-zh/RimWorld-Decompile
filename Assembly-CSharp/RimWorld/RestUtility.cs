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
			if (building_Bed == null)
			{
				return false;
			}
			if (!traveler.CanReserveAndReach((Thing)building_Bed, PathEndMode.OnCell, Danger.Some, building_Bed.SleepingSlotsCount, -1, null, ignoreOtherReservations))
			{
				return false;
			}
			if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
			{
				return false;
			}
			if (!building_Bed.AnyUnoccupiedSleepingSlot && (!sleeper.InBed() || sleeper.CurrentBed() != building_Bed) && !building_Bed.AssignedPawns.Contains(sleeper))
			{
				return false;
			}
			if (building_Bed.IsForbidden(traveler))
			{
				return false;
			}
			if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, sleeperWillBePrisoner, false))
			{
				return false;
			}
			if (building_Bed.IsBurning())
			{
				return false;
			}
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
				if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCare(sleeper))
				{
					return false;
				}
				if (!HealthAIUtility.ShouldSeekMedicalRest(sleeper))
				{
					return false;
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
							return false;
						}
						goto IL_018f;
					}
					return false;
				}
				if (!building_Bed.AnyUnownedSleepingSlot)
				{
					return false;
				}
			}
			goto IL_018f;
			IL_018f:
			return true;
		}

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

		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, p.IsPrisoner, true, false);
		}

		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool ignoreOtherReservations = false)
		{
			if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
			{
				if (sleeper.InBed() && sleeper.CurrentBed().Medical)
				{
					bool ignoreOtherReservations2 = ignoreOtherReservations;
					if (RestUtility.IsValidBedFor(sleeper.CurrentBed(), sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations2))
					{
						return sleeper.CurrentBed();
					}
				}
				for (int i = 0; i < RestUtility.bedDefsBestToWorst_Medical.Count; i++)
				{
					Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing b)
					{
						int result2;
						if (((Building_Bed)b).Medical)
						{
							bool ignoreOtherReservations4 = ignoreOtherReservations;
							result2 = (RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations4) ? 1 : 0);
						}
						else
						{
							result2 = 0;
						}
						return (byte)result2 != 0;
					};
					Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(RestUtility.bedDefsBestToWorst_Medical[i]), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
					if (building_Bed != null)
					{
						return building_Bed;
					}
				}
			}
			if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null)
			{
				bool ignoreOtherReservations2 = ignoreOtherReservations;
				if (RestUtility.IsValidBedFor(sleeper.ownership.OwnedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations2))
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
					bool ignoreOtherReservations2 = ignoreOtherReservations;
					if (RestUtility.IsValidBedFor(ownedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations2))
					{
						return ownedBed;
					}
				}
			}
			for (int j = 0; j < RestUtility.bedDefsBestToWorst_RestEffectiveness.Count; j++)
			{
				ThingDef thingDef = RestUtility.bedDefsBestToWorst_RestEffectiveness[j];
				if (RestUtility.CanUseBedEver(sleeper, thingDef))
				{
					Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing b)
					{
						int result;
						if (!((Building_Bed)b).Medical)
						{
							bool ignoreOtherReservations3 = ignoreOtherReservations;
							result = (RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations3) ? 1 : 0);
						}
						else
						{
							result = 0;
						}
						return (byte)result != 0;
					};
					Building_Bed building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
					if (building_Bed2 != null)
					{
						return building_Bed2;
					}
				}
			}
			return null;
		}

		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> predicate = (Predicate<Thing>)delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				if (building_Bed2 == null)
				{
					return false;
				}
				if (!building_Bed2.Medical && building_Bed2.def.building.bed_humanlike)
				{
					return false;
				}
				if (!RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, pawn.IsPrisoner, false, true, false))
				{
					return false;
				}
				return true;
			};
			if (pawn.InBed() && predicate(pawn.CurrentBed()))
			{
				return pawn.CurrentBed();
			}
			Predicate<Thing> validator = predicate;
			Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (building_Bed != null)
			{
				return building_Bed;
			}
			return RestUtility.FindBedFor(pawn);
		}

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
			Log.Error("Could not find good sleeping slot position for " + pawn + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.");
			return bed.GetSleepingSlotPos(0);
		}

		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			if (p.BodySize > bedDef.building.bed_maxBodySize)
			{
				return false;
			}
			if (p.RaceProps.Humanlike != bedDef.building.bed_humanlike)
			{
				return false;
			}
			return true;
		}

		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			if (pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest && pawn.needs.rest.CurLevel >= 0.20000000298023224)
			{
				return true;
			}
			return false;
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
			if (!p.health.capacities.CanBeAwake)
			{
				return false;
			}
			if (!p.Spawned)
			{
				return true;
			}
			return p.CurJob == null || !p.jobs.curDriver.asleep;
		}

		public static Building_Bed CurrentBed(this Pawn p)
		{
			if (p.Spawned && p.CurJob != null && p.jobs.curDriver.layingDown == LayingDownState.LayingInBed)
			{
				Building_Bed building_Bed = null;
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
					return null;
				}
				for (int i = 0; i < building_Bed.SleepingSlotsCount; i++)
				{
					if (building_Bed.GetCurOccupant(i) == p)
					{
						return building_Bed;
					}
				}
				return null;
			}
			return null;
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
			if (lord != null && lord.CurLordToil != null && lord.CurLordToil.CustomWakeThreshold.HasValue)
			{
				return lord.CurLordToil.CustomWakeThreshold.Value;
			}
			return 1f;
		}

		public static float FallAsleepMaxLevel(Pawn p)
		{
			return Mathf.Min(0.75f, (float)(RestUtility.WakeThreshold(p) - 0.0099999997764825821));
		}
	}
}
