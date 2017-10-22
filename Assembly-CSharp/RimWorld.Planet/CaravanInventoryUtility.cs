using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanInventoryUtility
	{
		private static List<Thing> inventoryItems = new List<Thing>();

		private static List<Thing> inventoryToMove = new List<Thing>();

		private static List<Apparel> tmpApparel = new List<Apparel>();

		private static List<ThingWithComps> tmpEquipment = new List<ThingWithComps>();

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

		public static Pawn GetOwnerOf(Caravan caravan, Thing item)
		{
			IThingHolder parentHolder = item.ParentHolder;
			Pawn result;
			if (parentHolder is Pawn_InventoryTracker)
			{
				Pawn pawn = (Pawn)parentHolder.ParentHolder;
				if (caravan.ContainsPawn(pawn))
				{
					result = pawn;
					goto IL_003b;
				}
			}
			result = null;
			goto IL_003b;
			IL_003b:
			return result;
		}

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
						if (compDrug != null && compDrug.Props.chemical != null && compDrug.Props.chemical.addictionHediff == addictionHediff.def && (forPawn.drugs == null || forPawn.drugs.CurrentPolicy[thing2.def].allowedForAddiction || forPawn.story == null || forPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0))
						{
							thing = thing2;
							break;
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

		public static bool TryGetBestMedicine(Caravan caravan, Pawn patient, out Medicine medicine, out Pawn owner)
		{
			bool result;
			if (patient.playerSettings == null || (int)patient.playerSettings.medCare <= 1)
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
					if (thing.def.IsMedicine && patient.playerSettings.medCare.AllowsMedicine(thing.def))
					{
						float statValue = thing.GetStatValue(StatDefOf.MedicalPotency, true);
						if (statValue > num || medicine2 == null)
						{
							num = statValue;
							medicine2 = (Medicine)thing;
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

		public static bool TryGetThingOfDef(Caravan caravan, ThingDef thingDef, out Thing thing, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < list.Count)
				{
					Thing thing2 = list[num];
					if (thing2.def == thingDef)
					{
						thing = thing2;
						owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing2);
						result = true;
						break;
					}
					num++;
					continue;
				}
				thing = null;
				owner = null;
				result = false;
				break;
			}
			return result;
		}

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

		public static void MoveInventoryToSomeoneElse(Pawn itemOwner, Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, int numToMove)
		{
			if (numToMove < 0 || numToMove > item.stackCount)
			{
				Log.Warning("Tried to move item " + item + " with numToMove=" + numToMove + " (item stack count = " + item.stackCount + ")");
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

		public static Pawn FindPawnToMoveInventoryTo(Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, Pawn currentItemOwner = null)
		{
			Pawn result;
			if (item is Pawn)
			{
				Log.Error("Called FindPawnToMoveInventoryTo but the item is a pawn.");
				result = null;
			}
			else
			{
				Pawn pawn = default(Pawn);
				result = ((!(from x in candidates
				where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner && !MassUtility.IsOverEncumbered(x)
				select x).TryRandomElement<Pawn>(out pawn)) ? ((!(from x in candidates
				where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
				select x).TryRandomElement<Pawn>(out pawn)) ? ((!(from x in candidates
				where (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
				select x).TryRandomElement<Pawn>(out pawn)) ? null : pawn) : pawn) : pawn);
			}
			return result;
		}

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

		private static bool CanMoveInventoryTo(Pawn pawn)
		{
			return MassUtility.CanEverCarryAnything(pawn);
		}

		public static List<Thing> TakeThings(Caravan caravan, Func<Thing, int> takeQuantity)
		{
			List<Thing> list = new List<Thing>();
			foreach (Thing item in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				int num = takeQuantity(item);
				if (num > 0)
				{
					list.Add(item.holdingOwner.Take(item, num));
				}
			}
			return list;
		}

		public static void GiveThing(Caravan caravan, Thing thing)
		{
			if (CaravanInventoryUtility.AllInventoryItems(caravan).Contains(thing))
			{
				Log.Error("Tried to give the same item twice (" + thing + ") to a caravan (" + caravan + ").");
			}
			else
			{
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
				if (pawn == null)
				{
					Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan));
					thing.Destroy(DestroyMode.Vanish);
				}
				else if (!pawn.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan));
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		public static bool HasThings(Caravan caravan, ThingDef thingDef, int count, Func<Thing, bool> validator = null)
		{
			int num = 0;
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def == thingDef && ((object)validator == null || validator(thing)))
				{
					num += thing.stackCount;
				}
			}
			return num >= count;
		}
	}
}
