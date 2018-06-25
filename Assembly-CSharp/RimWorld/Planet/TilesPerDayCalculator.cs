using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FA RID: 1530
	public static class TilesPerDayCalculator
	{
		// Token: 0x04001215 RID: 4629
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04001216 RID: 4630
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x06001E6D RID: 7789 RVA: 0x00107B1C File Offset: 0x00105F1C
		public static float ApproxTilesPerDay(int caravanTicksPerMove, int tile, int nextTile, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null)
		{
			if (nextTile == -1)
			{
				nextTile = Find.WorldGrid.FindMostReasonableAdjacentTileForDisplayedPathCost(tile);
			}
			int end = nextTile;
			float num = (float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, tile, end, null, false, explanation, caravanTicksPerMoveExplanation);
			int num2 = Mathf.CeilToInt(num / 1f);
			float result;
			if (num2 == 0)
			{
				result = 0f;
			}
			else
			{
				result = 60000f / (float)num2;
			}
			return result;
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x00107B98 File Offset: 0x00105F98
		public static float ApproxTilesPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return TilesPerDayCalculator.ApproxTilesPerDay(caravan.TicksPerMove, caravan.Tile, (!caravan.pather.Moving) ? -1 : caravan.pather.nextTile, explanation, (explanation == null) ? null : caravan.TicksPerMoveExplanation);
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x00107BF4 File Offset: 0x00105FF4
		public static float ApproxTilesPerDay(List<TransferableOneWay> transferables, float massUsage, float massCapacity, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result;
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				result = 0f;
			}
			else
			{
				StringBuilder stringBuilder = (explanation == null) ? null : new StringBuilder();
				int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, stringBuilder);
				float num = TilesPerDayCalculator.ApproxTilesPerDay(ticksPerMove, tile, nextTile, explanation, (stringBuilder == null) ? null : stringBuilder.ToString());
				TilesPerDayCalculator.tmpPawns.Clear();
				result = num;
			}
			return result;
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x00107CF4 File Offset: 0x001060F4
		public static float ApproxTilesPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, float massUsageLeftAfterTransfer, float massCapacityLeftAfterTransfer, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result;
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				result = 0f;
			}
			else
			{
				StringBuilder stringBuilder = (explanation == null) ? null : new StringBuilder();
				int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsageLeftAfterTransfer, massCapacityLeftAfterTransfer, stringBuilder);
				float num = TilesPerDayCalculator.ApproxTilesPerDay(ticksPerMove, tile, nextTile, explanation, (stringBuilder == null) ? null : stringBuilder.ToString());
				TilesPerDayCalculator.tmpPawns.Clear();
				result = num;
			}
			return result;
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x00107E00 File Offset: 0x00106200
		public static float ApproxTilesPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, float massUsageLeftAfterTradeableTransfer, float massCapacityLeftAfterTradeableTransfer, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, TilesPerDayCalculator.tmpThingCounts);
			float result = TilesPerDayCalculator.ApproxTilesPerDay(TilesPerDayCalculator.tmpThingCounts, massUsageLeftAfterTradeableTransfer, massCapacityLeftAfterTradeableTransfer, tile, nextTile, explanation);
			TilesPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x00107E4C File Offset: 0x0010624C
		public static float ApproxTilesPerDay(List<ThingCount> thingCounts, float massUsage, float massCapacity, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						TilesPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			float result;
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				result = 0f;
			}
			else
			{
				StringBuilder stringBuilder = (explanation == null) ? null : new StringBuilder();
				int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, stringBuilder);
				float num = TilesPerDayCalculator.ApproxTilesPerDay(ticksPerMove, tile, nextTile, explanation, (stringBuilder == null) ? null : stringBuilder.ToString());
				TilesPerDayCalculator.tmpPawns.Clear();
				result = num;
			}
			return result;
		}
	}
}
