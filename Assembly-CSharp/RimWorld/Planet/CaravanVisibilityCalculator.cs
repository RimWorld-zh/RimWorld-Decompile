using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E9 RID: 1513
	public static class CaravanVisibilityCalculator
	{
		// Token: 0x06001DE7 RID: 7655 RVA: 0x001015D0 File Offset: 0x000FF9D0
		public static float Visibility(float bodySizeSum, bool caravanMovingNow, StringBuilder explanation = null)
		{
			float num = CaravanVisibilityCalculator.BodySizeSumToVisibility.Evaluate(bodySizeSum);
			if (explanation != null)
			{
				if (explanation.Length > 0)
				{
					explanation.AppendLine();
				}
				explanation.Append("TotalBodySize".Translate() + ": " + bodySizeSum.ToString("0.##"));
			}
			if (!caravanMovingNow)
			{
				num *= 0.3f;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("CaravanNotMoving".Translate() + ": " + 0.3f.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x00101678 File Offset: 0x000FFA78
		public static float Visibility(Caravan caravan, StringBuilder explanation = null)
		{
			return CaravanVisibilityCalculator.Visibility(caravan.PawnsListForReading, caravan.pather.MovingNow, explanation);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x001016A4 File Offset: 0x000FFAA4
		public static float Visibility(List<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < pawns.Count; i++)
			{
				num += pawns[i].BodySize;
			}
			return CaravanVisibilityCalculator.Visibility(num, caravanMovingNow, explanation);
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x001016F0 File Offset: 0x000FFAF0
		public static float Visibility(IEnumerable<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			CaravanVisibilityCalculator.tmpPawns.AddRange(pawns);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, caravanMovingNow, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x00101734 File Offset: 0x000FFB34
		public static float Visibility(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						CaravanVisibilityCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x001017E4 File Offset: 0x000FFBE4
		public static float VisibilityLeftAfterTransfer(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						CaravanVisibilityCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x001018A0 File Offset: 0x000FFCA0
		public static float VisibilityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CaravanVisibilityCalculator.tmpThingCounts);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpThingCounts, explanation);
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x001018E4 File Offset: 0x000FFCE4
		public static float Visibility(List<ThingCount> thingCounts, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						CaravanVisibilityCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, true, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x040011AF RID: 4527
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x040011B0 RID: 4528
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040011B1 RID: 4529
		private static readonly SimpleCurve BodySizeSumToVisibility = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.2f),
				true
			},
			{
				new CurvePoint(6f, 1f),
				true
			},
			{
				new CurvePoint(12f, 1.12f),
				true
			}
		};

		// Token: 0x040011B2 RID: 4530
		public const float NotMovingFactor = 0.3f;
	}
}
