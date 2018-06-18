using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E6 RID: 1510
	public static class CaravanTicksPerMoveUtility
	{
		// Token: 0x06001DD8 RID: 7640 RVA: 0x00100F3C File Offset: 0x000FF33C
		public static int GetTicksPerMove(Caravan caravan, StringBuilder explanation = null)
		{
			int result;
			if (caravan == null)
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				result = 3500;
			}
			else
			{
				result = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(caravan), explanation);
			}
			return result;
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x00100F7C File Offset: 0x000FF37C
		public static int GetTicksPerMove(CaravanTicksPerMoveUtility.CaravanInfo caravanInfo, StringBuilder explanation = null)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.pawns, caravanInfo.massUsage, caravanInfo.massCapacity, explanation);
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x00100FAC File Offset: 0x000FF3AC
		public static int GetTicksPerMove(List<Pawn> pawns, float massUsage, float massCapacity, StringBuilder explanation = null)
		{
			int result;
			if (pawns.Any<Pawn>())
			{
				if (explanation != null)
				{
					explanation.Append("CaravanMovementSpeedFull".Translate() + ":");
				}
				float num = 0f;
				for (int i = 0; i < pawns.Count; i++)
				{
					float num2 = (float)((!pawns[i].Downed) ? pawns[i].TicksPerMoveCardinal : 450);
					num2 = Mathf.Min(num2, 150f) * 380f;
					float num3 = 60000f / num2;
					if (explanation != null)
					{
						explanation.AppendLine();
						explanation.Append(string.Concat(new string[]
						{
							"  - ",
							pawns[i].LabelShortCap,
							": ",
							num3.ToString("0.#"),
							" ",
							"TilesPerDay".Translate()
						}));
						if (pawns[i].Downed)
						{
							explanation.Append(" (" + "DownedLower".Translate() + ")");
						}
					}
					num += num2 / (float)pawns.Count;
				}
				float moveSpeedFactorFromMass = CaravanTicksPerMoveUtility.GetMoveSpeedFactorFromMass(massUsage, massCapacity);
				if (explanation != null)
				{
					float num4 = 60000f / num;
					explanation.AppendLine();
					explanation.Append(string.Concat(new string[]
					{
						"  ",
						"Average".Translate(),
						": ",
						num4.ToString("0.#"),
						" ",
						"TilesPerDay".Translate()
					}));
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierForCarriedMass".Translate(new object[]
					{
						moveSpeedFactorFromMass.ToStringPercent()
					}));
				}
				int num5 = Mathf.Max(Mathf.RoundToInt(num / moveSpeedFactorFromMass), 1);
				if (explanation != null)
				{
					float num6 = 60000f / (float)num5;
					explanation.AppendLine();
					explanation.Append(string.Concat(new string[]
					{
						"  ",
						"FinalCaravanPawnsMovementSpeed".Translate(),
						": ",
						num6.ToString("0.#"),
						" ",
						"TilesPerDay".Translate()
					}));
				}
				result = num5;
			}
			else
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				result = 3500;
			}
			return result;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x00101234 File Offset: 0x000FF634
		private static float GetMoveSpeedFactorFromMass(float massUsage, float massCapacity)
		{
			float result;
			if (massCapacity <= 0f)
			{
				result = 1f;
			}
			else
			{
				float t = massUsage / massCapacity;
				result = Mathf.Lerp(1.6f, 1f, t);
			}
			return result;
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x00101274 File Offset: 0x000FF674
		private static void AppendUsingDefaultTicksPerMoveInfo(StringBuilder sb)
		{
			sb.Append("CaravanMovementSpeedFull".Translate() + ":");
			float num = 17.1428566f;
			sb.AppendLine();
			sb.Append(string.Concat(new string[]
			{
				"  ",
				"Default".Translate(),
				": ",
				num.ToString("0.#"),
				" ",
				"TilesPerDay".Translate()
			}));
		}

		// Token: 0x040011A7 RID: 4519
		private const int MaxPawnTicksPerMove = 150;

		// Token: 0x040011A8 RID: 4520
		private const int DownedPawnMoveTicks = 450;

		// Token: 0x040011A9 RID: 4521
		public const float CellToTilesConversionRatio = 380f;

		// Token: 0x040011AA RID: 4522
		public const int DefaultTicksPerMove = 3500;

		// Token: 0x040011AB RID: 4523
		private const float MoveSpeedFactorAtZeroMass = 1.6f;

		// Token: 0x020005E7 RID: 1511
		public struct CaravanInfo
		{
			// Token: 0x06001DDD RID: 7645 RVA: 0x001012FD File Offset: 0x000FF6FD
			public CaravanInfo(Caravan caravan)
			{
				this.pawns = caravan.PawnsListForReading;
				this.massUsage = caravan.MassUsage;
				this.massCapacity = caravan.MassCapacity;
			}

			// Token: 0x06001DDE RID: 7646 RVA: 0x00101324 File Offset: 0x000FF724
			public CaravanInfo(Dialog_FormCaravan formCaravanDialog)
			{
				this.pawns = TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables);
				this.massUsage = formCaravanDialog.MassUsage;
				this.massCapacity = formCaravanDialog.MassCapacity;
			}

			// Token: 0x040011AC RID: 4524
			public List<Pawn> pawns;

			// Token: 0x040011AD RID: 4525
			public float massUsage;

			// Token: 0x040011AE RID: 4526
			public float massCapacity;
		}
	}
}
