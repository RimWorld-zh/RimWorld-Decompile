using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E0 RID: 1504
	public static class CaravanInventoryUtility
	{
		// Token: 0x06001D9F RID: 7583 RVA: 0x000FF344 File Offset: 0x000FD744
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

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000FF3D4 File Offset: 0x000FD7D4
		public static void CaravanInventoryUtilityStaticUpdate()
		{
			CaravanInventoryUtility.inventoryItems.Clear();
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000FF3E4 File Offset: 0x000FD7E4
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

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000FF430 File Offset: 0x000FD830
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

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000FF4D0 File Offset: 0x000FD8D0
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

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000FF620 File Offset: 0x000FDA20
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

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000FF718 File Offset: 0x000FDB18
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

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000FF77C File Offset: 0x000FDB7C
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

		// Token: 0x06001DA7 RID: 7591 RVA: 0x000FF7F4 File Offset: 0x000FDBF4
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

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000FF8A0 File Offset: 0x000FDCA0
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

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000FF95C File Offset: 0x000FDD5C
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

		// Token: 0x06001DAA RID: 7594 RVA: 0x000FFA10 File Offset: 0x000FDE10
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

		// Token: 0x06001DAB RID: 7595 RVA: 0x000FFAC4 File Offset: 0x000FDEC4
		private static bool CanMoveInventoryTo(Pawn pawn)
		{
			return MassUtility.CanEverCarryAnything(pawn);
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000FFAE0 File Offset: 0x000FDEE0
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

		// Token: 0x06001DAD RID: 7597 RVA: 0x000FFB70 File Offset: 0x000FDF70
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

		// Token: 0x06001DAE RID: 7598 RVA: 0x000FFC30 File Offset: 0x000FE030
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

		// Token: 0x04001196 RID: 4502
		private static List<Thing> inventoryItems = new List<Thing>();

		// Token: 0x04001197 RID: 4503
		private static List<Thing> inventoryToMove = new List<Thing>();

		// Token: 0x04001198 RID: 4504
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x04001199 RID: 4505
		private static List<ThingWithComps> tmpEquipment = new List<ThingWithComps>();
	}
}
