using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000901 RID: 2305
	public static class CollectionsMassCalculator
	{
		// Token: 0x04001CF7 RID: 7415
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04001CF8 RID: 7416
		private static List<Thing> thingsInReverse = new List<Thing>();

		// Token: 0x06003578 RID: 13688 RVA: 0x001CD1E0 File Offset: 0x001CB5E0
		public static float Capacity(List<ThingCount> thingCounts, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						num += MassUtility.Capacity(pawn, explanation) * (float)thingCounts[i].Count;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x001CD274 File Offset: 0x001CB674
		public static float MassUsage(List<ThingCount> thingCounts, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				int count = thingCounts[i].Count;
				if (count > 0)
				{
					Thing thing = thingCounts[i].Thing;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						if (includePawnsMass)
						{
							num += pawn.GetStatValue(StatDefOf.Mass, true) * (float)count;
						}
						else
						{
							num += MassUtility.GearAndInventoryMass(pawn) * (float)count;
						}
						if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
						{
							num -= MassUtility.InventoryMass(pawn) * (float)count;
						}
					}
					else
					{
						num += thing.GetStatValue(StatDefOf.Mass, true) * (float)count;
						if (ignoreSpawnedCorpsesGearAndInventory)
						{
							Corpse corpse = thing as Corpse;
							if (corpse != null && corpse.Spawned)
							{
								num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn) * (float)count;
							}
						}
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x001CD38C File Offset: 0x001CB78C
		public static float CapacityTransferables(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing)
				{
					if (transferables[i].AnyThing is Pawn)
					{
						TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
						{
							CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
						}, false, false);
					}
				}
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x001CD448 File Offset: 0x001CB848
		public static float CapacityLeftAfterTransfer(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing)
				{
					if (transferables[i].AnyThing is Pawn)
					{
						CollectionsMassCalculator.thingsInReverse.Clear();
						CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
						CollectionsMassCalculator.thingsInReverse.Reverse();
						TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
						{
							CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
						}, false, false);
					}
				}
			}
			CollectionsMassCalculator.thingsInReverse.Clear();
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x001CD540 File Offset: 0x001CB940
		public static float CapacityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x001CD584 File Offset: 0x001CB984
		public static float Capacity<T>(List<T> things, StringBuilder explanation = null) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount));
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x001CD600 File Offset: 0x001CBA00
		public static float MassUsageTransferables(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x001CD690 File Offset: 0x001CBA90
		public static float MassUsageLeftAfterTransfer(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				CollectionsMassCalculator.thingsInReverse.Clear();
				CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
				CollectionsMassCalculator.thingsInReverse.Reverse();
				TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x001CD750 File Offset: 0x001CBB50
		public static float MassUsage<T>(List<T> things, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount));
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x001CD7D0 File Offset: 0x001CBBD0
		public static float MassUsageLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}
	}
}
