using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class DaysUntilRotCalculator
	{
		public const float InfiniteDaysUntilRot = 1000f;

		private static List<Thing> tmpThings = new List<Thing>();

		private static List<ThingStackPart> tmpThingStackParts = new List<ThingStackPart>();

		public static float ApproxDaysUntilRot(List<Thing> potentiallyFood, int assumingTile)
		{
			float num = 1000f;
			for (int i = 0; i < potentiallyFood.Count; i++)
			{
				Thing thing = potentiallyFood[i];
				if (thing.def.IsNutritionGivingIngestible)
				{
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						num = Mathf.Min(num, (float)((float)compRottable.ApproxTicksUntilRotWhenAtTempOfTile(assumingTile) / 60000.0));
					}
				}
			}
			return num;
		}

		public static float ApproxDaysUntilRot(Caravan caravan)
		{
			return DaysUntilRotCalculator.ApproxDaysUntilRot(CaravanInventoryUtility.AllInventoryItems(caravan), caravan.Tile);
		}

		public static float ApproxDaysUntilRot(List<TransferableOneWay> transferables, int assumingTile, IgnorePawnsInventoryMode ignoreInventory)
		{
			DaysUntilRotCalculator.tmpThings.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[j];
							if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
							{
								ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
								for (int k = 0; k < innerContainer.Count; k++)
								{
									DaysUntilRotCalculator.tmpThings.Add(innerContainer[k]);
								}
							}
						}
					}
					else if (transferableOneWay.CountToTransfer > 0)
					{
						DaysUntilRotCalculator.tmpThings.AddRange(transferableOneWay.things);
					}
				}
			}
			float result = DaysUntilRotCalculator.ApproxDaysUntilRot(DaysUntilRotCalculator.tmpThings, assumingTile);
			DaysUntilRotCalculator.tmpThings.Clear();
			return result;
		}

		public static float ApproxDaysUntilRotLeftAfterTransfer(List<TransferableOneWay> transferables, int assumingTile, IgnorePawnsInventoryMode ignoreInventory)
		{
			DaysUntilRotCalculator.tmpThings.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int num = transferableOneWay.things.Count - 1; num >= transferableOneWay.CountToTransfer; num--)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[num];
							if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
							{
								ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
								for (int j = 0; j < innerContainer.Count; j++)
								{
									DaysUntilRotCalculator.tmpThings.Add(innerContainer[j]);
								}
							}
						}
					}
					else if (transferableOneWay.MaxCount - transferableOneWay.CountToTransfer > 0)
					{
						DaysUntilRotCalculator.tmpThings.AddRange(transferableOneWay.things);
					}
				}
			}
			float result = DaysUntilRotCalculator.ApproxDaysUntilRot(DaysUntilRotCalculator.tmpThings, assumingTile);
			DaysUntilRotCalculator.tmpThings.Clear();
			return result;
		}

		public static float ApproxDaysUntilRotLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, int assumingTile, IgnorePawnsInventoryMode ignoreInventory)
		{
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, DaysUntilRotCalculator.tmpThingStackParts);
			DaysUntilRotCalculator.tmpThings.Clear();
			for (int num = DaysUntilRotCalculator.tmpThingStackParts.Count - 1; num >= 0; num--)
			{
				if (DaysUntilRotCalculator.tmpThingStackParts[num].Count > 0)
				{
					Pawn pawn = DaysUntilRotCalculator.tmpThingStackParts[num].Thing as Pawn;
					if (pawn != null)
					{
						if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
						{
							ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
							for (int i = 0; i < innerContainer.Count; i++)
							{
								DaysUntilRotCalculator.tmpThings.Add(innerContainer[i]);
							}
						}
					}
					else
					{
						DaysUntilRotCalculator.tmpThings.Add(DaysUntilRotCalculator.tmpThingStackParts[num].Thing);
					}
				}
			}
			DaysUntilRotCalculator.tmpThingStackParts.Clear();
			float result = DaysUntilRotCalculator.ApproxDaysUntilRot(DaysUntilRotCalculator.tmpThings, assumingTile);
			DaysUntilRotCalculator.tmpThings.Clear();
			return result;
		}
	}
}
