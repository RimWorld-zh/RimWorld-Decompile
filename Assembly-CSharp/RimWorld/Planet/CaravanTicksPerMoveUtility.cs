using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanTicksPerMoveUtility
	{
		private const int MaxPawnTicksPerMove = 150;

		private const int DownedPawnMoveTicks = 450;

		public const float CellToTilesConversionRatio = 380f;

		public const int DefaultTicksPerMove = 3500;

		private const float MoveSpeedFactorAtZeroMass = 1.6f;

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

		public static int GetTicksPerMove(CaravanTicksPerMoveUtility.CaravanInfo caravanInfo, StringBuilder explanation = null)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.pawns, caravanInfo.massUsage, caravanInfo.massCapacity, explanation);
		}

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
					float num2 = (float)((!pawns[i].Downed && !pawns[i].CarriedByCaravan()) ? pawns[i].TicksPerMoveCardinal : 450);
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
						else if (pawns[i].CarriedByCaravan())
						{
							explanation.Append(" (" + "Carried".Translate() + ")");
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

		public struct CaravanInfo
		{
			public List<Pawn> pawns;

			public float massUsage;

			public float massCapacity;

			public CaravanInfo(Caravan caravan)
			{
				this.pawns = caravan.PawnsListForReading;
				this.massUsage = caravan.MassUsage;
				this.massCapacity = caravan.MassCapacity;
			}

			public CaravanInfo(Dialog_FormCaravan formCaravanDialog)
			{
				this.pawns = TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables);
				this.massUsage = formCaravanDialog.MassUsage;
				this.massCapacity = formCaravanDialog.MassCapacity;
			}
		}
	}
}
