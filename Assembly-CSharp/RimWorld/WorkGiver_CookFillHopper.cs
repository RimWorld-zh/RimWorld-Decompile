using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_CookFillHopper : WorkGiver_Scanner
	{
		private static string TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans;

		private static string NoFoodToFillHopperTrans;

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Hopper);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public WorkGiver_CookFillHopper()
		{
			if (WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans == null)
			{
				WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans = "TheOnlyAvailableFoodIsInStorageOfHigherPriority".Translate();
			}
			if (WorkGiver_CookFillHopper.NoFoodToFillHopperTrans == null)
			{
				WorkGiver_CookFillHopper.NoFoodToFillHopperTrans = "NoFoodToFillHopper".Translate();
			}
		}

		public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			ISlotGroupParent slotGroupParent = thing as ISlotGroupParent;
			Job result;
			if (slotGroupParent == null)
			{
				result = null;
			}
			else if (!pawn.CanReserve(thing.Position, 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				int num = 0;
				List<Thing> list = pawn.Map.thingGrid.ThingsListAt(thing.Position);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing2.def))
					{
						num += thing2.stackCount;
					}
				}
				if (num > 25)
				{
					JobFailReason.Is("AlreadyFilledLower".Translate());
					result = null;
				}
				else
				{
					result = WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, slotGroupParent);
				}
			}
			return result;
		}

		public static Job HopperFillFoodJob(Pawn pawn, ISlotGroupParent hopperSgp)
		{
			Building building = hopperSgp as Building;
			Job result;
			Job job;
			if (!pawn.CanReserveAndReach(building.Position, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				ThingDef thingDef = null;
				Thing firstItem = building.Position.GetFirstItem(building.Map);
				if (firstItem != null)
				{
					if (!Building_NutrientPasteDispenser.IsAcceptableFeedstock(firstItem.def))
					{
						result = ((!firstItem.IsForbidden(pawn)) ? HaulAIUtility.HaulAsideJobFor(pawn, firstItem) : null);
						goto IL_01e0;
					}
					thingDef = firstItem.def;
				}
				List<Thing> list = (thingDef != null) ? pawn.Map.listerThings.ThingsOfDef(thingDef) : pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
				bool flag = false;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (thing.def.IsNutritionGivingIngestible && (thing.def.ingestible.preferability == FoodPreferability.RawBad || thing.def.ingestible.preferability == FoodPreferability.RawTasty) && HaulAIUtility.PawnCanAutomaticallyHaul(pawn, thing, false) && pawn.Map.slotGroupManager.SlotGroupAt(building.Position).Settings.AllowedToAccept(thing))
					{
						StoragePriority storagePriority = HaulAIUtility.StoragePriorityAtFor(thing.Position, thing);
						if ((int)storagePriority >= (int)hopperSgp.GetSlotGroup().Settings.Priority)
						{
							flag = true;
							JobFailReason.Is(WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans);
						}
						else
						{
							job = HaulAIUtility.HaulMaxNumToCellJob(pawn, thing, building.Position, true);
							if (job != null)
								goto IL_01ab;
						}
					}
				}
				if (!flag)
				{
					JobFailReason.Is(WorkGiver_CookFillHopper.NoFoodToFillHopperTrans);
				}
				result = null;
			}
			goto IL_01e0;
			IL_01ab:
			result = job;
			goto IL_01e0;
			IL_01e0:
			return result;
		}
	}
}
