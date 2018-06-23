using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E5 RID: 1509
	public static class CaravanVisibilityCalculator
	{
		// Token: 0x040011AC RID: 4524
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x040011AD RID: 4525
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040011AE RID: 4526
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

		// Token: 0x040011AF RID: 4527
		public const float NotMovingFactor = 0.3f;

		// Token: 0x06001DDE RID: 7646 RVA: 0x00101624 File Offset: 0x000FFA24
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

		// Token: 0x06001DDF RID: 7647 RVA: 0x001016CC File Offset: 0x000FFACC
		public static float Visibility(Caravan caravan, StringBuilder explanation = null)
		{
			return CaravanVisibilityCalculator.Visibility(caravan.PawnsListForReading, caravan.pather.MovingNow, explanation);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x001016F8 File Offset: 0x000FFAF8
		public static float Visibility(List<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < pawns.Count; i++)
			{
				num += pawns[i].BodySize;
			}
			return CaravanVisibilityCalculator.Visibility(num, caravanMovingNow, explanation);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x00101744 File Offset: 0x000FFB44
		public static float Visibility(IEnumerable<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpPawns.Clear();
			CaravanVisibilityCalculator.tmpPawns.AddRange(pawns);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpPawns, caravanMovingNow, explanation);
			CaravanVisibilityCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x00101788 File Offset: 0x000FFB88
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

		// Token: 0x06001DE3 RID: 7651 RVA: 0x00101838 File Offset: 0x000FFC38
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

		// Token: 0x06001DE4 RID: 7652 RVA: 0x001018F4 File Offset: 0x000FFCF4
		public static float VisibilityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CaravanVisibilityCalculator.tmpThingCounts);
			float result = CaravanVisibilityCalculator.Visibility(CaravanVisibilityCalculator.tmpThingCounts, explanation);
			CaravanVisibilityCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x00101938 File Offset: 0x000FFD38
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
	}
}
