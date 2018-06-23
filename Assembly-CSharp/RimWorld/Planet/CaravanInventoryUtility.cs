using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005DC RID: 1500
	public static class CaravanInventoryUtility
	{
		// Token: 0x04001193 RID: 4499
		private static List<Thing> inventoryItems = new List<Thing>();

		// Token: 0x04001194 RID: 4500
		private static List<Thing> inventoryToMove = new List<Thing>();

		// Token: 0x04001195 RID: 4501
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x04001196 RID: 4502
		private static List<ThingWithComps> tmpEquipment = new List<ThingWithComps>();

		// Token: 0x06001D98 RID: 7576 RVA: 0x000FF410 File Offset: 0x000FD810
		public static List<Thing> AllInventoryItems(Caravan caravan)
		{
			CaravanInventoryUtility.inventoryItems.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				for (int j = 0; j < pawn.inventory.innerContainer.Count; j++)
				{
					Thing item = pawn.inventory.innerContainer[j];
					CaravanInventoryUtility.inventoryItems.Add(item);
				}
			}
			return CaravanInventoryUtility.inventoryItems;
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000FF4A0 File Offset: 0x000FD8A0
		public static void CaravanInventoryUtilityStaticUpdate()
		{
			CaravanInventoryUtility.inventoryItems.Clear();
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x000FF4B0 File Offset: 0x000FD8B0
		public static Pawn GetOwnerOf(Caravan caravan, Thing item)
		{
			IThingHolder parentHolder = item.ParentHolder;
			if (parentHolder is Pawn_InventoryTracker)
			{
				Pawn pawn = (Pawn)parentHolder.ParentHolder;
				if (caravan.ContainsPawn(pawn))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x000FF4FC File Offset: 0x000FD8FC
		public static bool TryGetBestFood(Caravan caravan, Pawn forPawn, out Thing food, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Thing thing = null;
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (CaravanPawnsNeedsUtility.CanNowEatForNutrition(thing2, forPawn))
				{
					float foodScore = CaravanPawnsNeedsUtility.GetFoodScore(thing2, forPawn);
					if (thing == null || foodScore > num)
					{
						thing = thing2;
						num = foodScore;
					}
				}
			}
			bool result;
			if (thing != null)
			{
				food = thing;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				result = true;
			}
			else
			{
				food = null;
				owner = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000FF59C File Offset: 0x000FD99C
		public static bool TryGetBestDrug(Caravan caravan, Pawn forPawn, Need_Chemical chemical, out Thing drug, out Pawn owner)
		{
			Hediff_Addiction addictionHediff = chemical.AddictionHediff;
			bool result;
			if (addictionHediff == null)
			{
				drug = null;
				owner = null;
				result = false;
			}
			else
			{
				List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
				Thing thing = null;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					if (thing2.IngestibleNow && thing2.def.IsDrug)
					{
						CompDrug compDrug = thing2.TryGetComp<CompDrug>();
						if (compDrug != null && compDrug.Props.chemical != null)
						{
							if (compDrug.Props.chemical.addictionHediff == addictionHediff.def)
							{
								if (forPawn.drugs == null || forPawn.drugs.CurrentPolicy[thing2.def].allowedForAddiction || forPawn.story == null || forPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0)
								{
									thing = thing2;
									break;
								}
							}
						}
					}
				}
				if (thing != null)
				{
					drug = thing;
					owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					result = true;
				}
				else
				{
					drug = null;
					owner = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000FF6EC File Offset: 0x000FDAEC
		public static bool TryGetBestMedicine(Caravan caravan, Pawn patient, out Medicine medicine, out Pawn owner)
		{
			bool result;
			if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				medicine = null;
				owner = null;
				result = false;
			}
			else
			{
				List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
				Medicine medicine2 = null;
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (thing.def.IsMedicine)
					{
						if (patient.playerSettings.medCare.AllowsMedicine(thing.def))
						{
							float statValue = thing.GetStatValue(StatDefOf.MedicalPotency, true);
							if (statValue > num || medicine2 == null)
							{
								num = statValue;
								medicine2 = (Medicine)thing;
							}
						}
					}
				}
				if (medicine2 != null)
				{
					medicine = medicine2;
					owner = CaravanInventoryUtility.GetOwnerOf(caravan, medicine2);
					result = true;
				}
				else
				{
					medicine = null;
					owner = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000FF7E4 File Offset: 0x000FDBE4
		public static bool TryGetThingOfDef(Caravan caravan, ThingDef thingDef, out Thing thing, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.def == thingDef)
				{
					thing = thing2;
					owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing2);
					return true;
				}
			}
			thing = null;
			owner = null;
			return false;
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000FF848 File Offset: 0x000FDC48
		public static void MoveAllInventoryToSomeoneElse(Pawn from, List<Pawn> candidates, List<Pawn> ignoreCandidates = null)
		{
			CaravanInventoryUtility.inventoryToMove.Clear();
			CaravanInventoryUtility.inventoryToMove.AddRange(from.inventory.innerContainer);
			for (int i = 0; i < CaravanInventoryUtility.inventoryToMove.Count; i++)
			{
				CaravanInventoryUtility.MoveInventoryToSomeoneElse(from, CaravanInventoryUtility.inventoryToMove[i], candidates, ignoreCandidates, CaravanInventoryUtility.inventoryToMove[i].stackCount);
			}
			CaravanInventoryUtility.inventoryToMove.Clear();
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000FF8C0 File Offset: 0x000FDCC0
		public static void MoveInventoryToSomeoneElse(Pawn itemOwner, Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, int numToMove)
		{
			if (numToMove < 0 || numToMove > item.stackCount)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to move item ",
					item,
					" with numToMove=",
					numToMove,
					" (item stack count = ",
					item.stackCount,
					")"
				}), false);
			}
			else
			{
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, candidates, ignoreCandidates, itemOwner);
				if (pawn != null)
				{
					itemOwner.inventory.innerContainer.TryTransferToContainer(item, pawn.inventory.innerContainer, numToMove, true);
				}
			}
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000FF96C File Offset: 0x000FDD6C
		public static Pawn FindPawnToMoveInventoryTo(Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, Pawn currentItemOwner = null)
		{
			Pawn result;
			Pawn pawn;
			if (item is Pawn)
			{
				Log.Error("Called FindPawnToMoveInventoryTo but the item is a pawn.", false);
				result = null;
			}
			else if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner && !MassUtility.IsOverEncumbered(x)
			select x).TryRandomElement(out pawn))
			{
				result = pawn;
			}
			else if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out pawn))
			{
				result = pawn;
			}
			else if ((from x in candidates
			where (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out pawn))
			{
				result = pawn;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000FFA28 File Offset: 0x000FDE28
		public static void MoveAllApparelToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates)
		{
			if (moveFrom.apparel != null)
			{
				CaravanInventoryUtility.tmpApparel.Clear();
				CaravanInventoryUtility.tmpApparel.AddRange(moveFrom.apparel.WornApparel);
				for (int i = 0; i < CaravanInventoryUtility.tmpApparel.Count; i++)
				{
					moveFrom.apparel.Remove(CaravanInventoryUtility.tmpApparel[i]);
					Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpApparel[i], candidates, null, moveFrom);
					if (pawn != null)
					{
						pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpApparel[i], true);
					}
				}
				CaravanInventoryUtility.tmpApparel.Clear();
			}
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000FFADC File Offset: 0x000FDEDC
		public static void MoveAllEquipmentToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates)
		{
			if (moveFrom.equipment != null)
			{
				CaravanInventoryUtility.tmpEquipment.Clear();
				CaravanInventoryUtility.tmpEquipment.AddRange(moveFrom.equipment.AllEquipmentListForReading);
				for (int i = 0; i < CaravanInventoryUtility.tmpEquipment.Count; i++)
				{
					moveFrom.equipment.Remove(CaravanInventoryUtility.tmpEquipment[i]);
					Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpEquipment[i], candidates, null, moveFrom);
					if (pawn != null)
					{
						pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpEquipment[i], true);
					}
				}
				CaravanInventoryUtility.tmpEquipment.Clear();
			}
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000FFB90 File Offset: 0x000FDF90
		private static bool CanMoveInventoryTo(Pawn pawn)
		{
			return MassUtility.CanEverCarryAnything(pawn);
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000FFBAC File Offset: 0x000FDFAC
		public static List<Thing> TakeThings(Caravan caravan, Func<Thing, int> takeQuantity)
		{
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan).ToList<Thing>())
			{
				int num = takeQuantity(thing);
				if (num > 0)
				{
					list.Add(thing.holdingOwner.Take(thing, num));
				}
			}
			return list;
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000FFC3C File Offset: 0x000FE03C
		public static void GiveThing(Caravan caravan, Thing thing)
		{
			if (CaravanInventoryUtility.AllInventoryItems(caravan).Contains(thing))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to give the same item twice (",
					thing,
					") to a caravan (",
					caravan,
					")."
				}), false);
			}
			else
			{
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
				if (pawn == null)
				{
					Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan), false);
					thing.Destroy(DestroyMode.Vanish);
				}
				else if (!pawn.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan), false);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x000FFCFC File Offset: 0x000FE0FC
		public static bool HasThings(Caravan caravan, ThingDef thingDef, int count, Func<Thing, bool> validator = null)
		{
			int num = 0;
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def == thingDef && (validator == null || validator(thing)))
				{
					num += thing.stackCount;
				}
			}
			return num >= count;
		}
	}
}
