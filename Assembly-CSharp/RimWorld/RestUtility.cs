using System;
using System.Collections.Generic;
using System.Linq;
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
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
		}

		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false)
		{
			Building_Bed building_Bed = bedThing as Building_Bed;
			if (building_Bed == null)
			{
				return false;
			}
			if (!traveler.CanReserveAndReach(building_Bed, PathEndMode.OnCell, Danger.Some, building_Bed.SleepingSlotsCount, -1, null, false))
			{
				return false;
			}
			if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
			{
				return false;
			}
			if (!building_Bed.AnyUnoccupiedSleepingSlot && (!sleeper.InBed() || sleeper.CurrentBed() != building_Bed))
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
				if (!building_Bed.AnyUnoccupiedSleepingSlot && (!sleeper.InBed() || sleeper.CurrentBed() != building_Bed))
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
					if (!building_Bed.owners.Any((Pawn x) => LovePartnerRelationUtility.LovePartnerRelationExists(sleeper, x)))
					{
						return false;
					}
					if (!building_Bed.AnyUnownedSleepingSlot)
					{
						return false;
					}
				}
			}
			return (!checkSocialProperness || building_Bed.IsSociallyProper(sleeper, sleeperWillBePrisoner, false)) && !building_Bed.IsForbidden(traveler) && !building_Bed.IsBurning();
		}

		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, p.IsPrisoner, true);
		}

		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness)
		{
			if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
			{
				if (sleeper.InBed() && sleeper.CurrentBed().Medical && RestUtility.IsValidBedFor(sleeper.CurrentBed(), sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false))
				{
					return sleeper.CurrentBed();
				}
				for (int i = 0; i < RestUtility.bedDefsBestToWorst_Medical.Count; i++)
				{
					Predicate<Thing> validator = (Thing b) => ((Building_Bed)b).Medical && RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false);
					Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(RestUtility.bedDefsBestToWorst_Medical[i]), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, -1, false, RegionType.Set_Passable);
					if (building_Bed != null)
					{
						return building_Bed;
					}
				}
			}
			if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null && RestUtility.IsValidBedFor(sleeper.ownership.OwnedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false))
			{
				return sleeper.ownership.OwnedBed;
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null && RestUtility.IsValidBedFor(ownedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false))
				{
					return ownedBed;
				}
			}
			for (int j = 0; j < RestUtility.bedDefsBestToWorst_RestEffectiveness.Count; j++)
			{
				ThingDef thingDef = RestUtility.bedDefsBestToWorst_RestEffectiveness[j];
				if (RestUtility.CanUseBedEver(sleeper, thingDef))
				{
					Predicate<Thing> validator = (Thing b) => !((Building_Bed)b).Medical && RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false);
					Building_Bed building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, -1, false, RegionType.Set_Passable);
					if (building_Bed2 != null)
					{
						if (sleeper.ownership != null)
						{
							sleeper.ownership.UnclaimBed();
						}
						return building_Bed2;
					}
				}
			}
			return null;
		}

		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> predicate = delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return building_Bed2 != null && (building_Bed2.Medical || !building_Bed2.def.building.bed_humanlike) && RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, pawn.IsPrisoner, false, true);
			};
			if (pawn.InBed() && predicate(pawn.CurrentBed()))
			{
				return pawn.CurrentBed();
			}
			Predicate<Thing> validator = predicate;
			Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, -1, false, RegionType.Set_Passable);
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
			return p.BodySize <= bedDef.building.bed_maxBodySize && p.RaceProps.Humanlike == bedDef.building.bed_humanlike;
		}

		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			return pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest && pawn.needs.rest.CurLevel >= 0.2f;
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
			if (!p.Spawned || p.CurJob == null || p.jobs.curDriver.layingDown != LayingDownState.LayingInBed)
			{
				return null;
			}
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
				return null;
			}
			for (int j = 0; j < building_Bed.SleepingSlotsCount; j++)
			{
				if (building_Bed.GetCurOccupant(j) == p)
				{
					return building_Bed;
				}
			}
			return null;
		}

		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		public static void WakeUp(Pawn p)
		{
			if (p.CurJob != null && p.jobs.curDriver.layingDown != LayingDownState.NotLaying && !p.Downed)
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
	}
}
