using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FC RID: 1532
	public static class TilesPerDayCalculator
	{
		// Token: 0x06001E71 RID: 7793 RVA: 0x00107698 File Offset: 0x00105A98
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

		// Token: 0x06001E72 RID: 7794 RVA: 0x00107714 File Offset: 0x00105B14
		public static float ApproxTilesPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return TilesPerDayCalculator.ApproxTilesPerDay(caravan.TicksPerMove, caravan.Tile, (!caravan.pather.Moving) ? -1 : caravan.pather.nextTile, explanation, (explanation == null) ? null : caravan.TicksPerMoveExplanation);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00107770 File Offset: 0x00105B70
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

		// Token: 0x06001E74 RID: 7796 RVA: 0x00107870 File Offset: 0x00105C70
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

		// Token: 0x06001E75 RID: 7797 RVA: 0x0010797C File Offset: 0x00105D7C
		public static float ApproxTilesPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, float massUsageLeftAfterTradeableTransfer, float massCapacityLeftAfterTradeableTransfer, int tile, int nextTile, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, TilesPerDayCalculator.tmpThingCounts);
			float result = TilesPerDayCalculator.ApproxTilesPerDay(TilesPerDayCalculator.tmpThingCounts, massUsageLeftAfterTradeableTransfer, massCapacityLeftAfterTradeableTransfer, tile, nextTile, explanation);
			TilesPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x001079C8 File Offset: 0x00105DC8
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

		// Token: 0x04001214 RID: 4628
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04001215 RID: 4629
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();
	}
}
