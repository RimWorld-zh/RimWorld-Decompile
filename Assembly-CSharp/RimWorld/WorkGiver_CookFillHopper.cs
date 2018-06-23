using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200013C RID: 316
	public class WorkGiver_CookFillHopper : WorkGiver_Scanner
	{
		// Token: 0x04000314 RID: 788
		private static string TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans;

		// Token: 0x04000315 RID: 789
		private static string NoFoodToFillHopperTrans;

		// Token: 0x06000671 RID: 1649 RVA: 0x00042E4D File Offset: 0x0004124D
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

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00042E88 File Offset: 0x00041288
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Hopper);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00042EA8 File Offset: 0x000412A8
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00042EC0 File Offset: 0x000412C0
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
					JobFailReason.Is("AlreadyFilledLower".Translate(), null);
					result = null;
				}
				else
				{
					result = WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, slotGroupParent);
				}
			}
			return result;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00042F8C File Offset: 0x0004138C
		public static Job HopperFillFoodJob(Pawn pawn, ISlotGroupParent hopperSgp)
		{
			Building building = (Building)hopperSgp;
			Job result;
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
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(firstItem.def))
					{
						thingDef = firstItem.def;
					}
					else
					{
						if (firstItem.IsForbidden(pawn))
						{
							return null;
						}
						return HaulAIUtility.HaulAsideJobFor(pawn, firstItem);
					}
				}
				List<Thing> list;
				if (thingDef == null)
				{
					list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
				}
				else
				{
					list = pawn.Map.listerThings.ThingsOfDef(thingDef);
				}
				bool flag = false;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (thing.def.IsNutritionGivingIngestible)
					{
						if (thing.def.ingestible.preferability == FoodPreferability.RawBad || thing.def.ingestible.preferability == FoodPreferability.RawTasty)
						{
							if (HaulAIUtility.PawnCanAutomaticallyHaul(pawn, thing, false))
							{
								if (pawn.Map.haulDestinationManager.SlotGroupAt(building.Position).Settings.AllowedToAccept(thing))
								{
									StoragePriority storagePriority = StoreUtility.CurrentStoragePriorityOf(thing);
									if (storagePriority >= hopperSgp.GetSlotGroup().Settings.Priority)
									{
										flag = true;
										JobFailReason.Is(WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans, null);
									}
									else
									{
										Job job = HaulAIUtility.HaulToCellStorageJob(pawn, thing, building.Position, true);
										if (job != null)
										{
											return job;
										}
									}
								}
							}
						}
					}
				}
				if (!flag)
				{
					JobFailReason.Is(WorkGiver_CookFillHopper.NoFoodToFillHopperTrans, null);
				}
				result = null;
			}
			return result;
		}
	}
}
