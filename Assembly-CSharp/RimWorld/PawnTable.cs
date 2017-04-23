using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class PawnTable
	{
		private static List<float> columnWidths = new List<float>();

		private static List<PawnColumnDef> columns = new List<PawnColumnDef>();

		private static List<PawnColumnDef> columnsInWidthPriorityOrder = new List<PawnColumnDef>();

		private static List<bool> columnAtMaxWidth = new List<bool>();

		public static float GetOptimalTableWidth(string table, List<Pawn> pawns, float minTableWidth, float maxTableWidth)
		{
			PawnTable.CalculateColumns(table);
			float num = 0f;
			for (int i = 0; i < PawnTable.columns.Count; i++)
			{
				num += Mathf.Max((float)PawnTable.columns[i].Worker.GetOptimalWidth(pawns), 0f);
			}
			return Mathf.Clamp(num, minTableWidth, maxTableWidth);
		}

		public static void DoTable(string table, Rect rect, List<Pawn> pawns)
		{
			PawnTable.CalculateColumns(table);
			PawnTable.CalculateColumnWidths(pawns, rect.width);
		}

		private static void CalculateColumns(string table)
		{
			PawnTable.columns.Clear();
			List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].tables.Contains(table))
				{
					PawnTable.columns.Add(allDefsListForReading[i]);
				}
			}
			PawnTable.columns.SortBy((PawnColumnDef x) => x.order);
		}

		private static void CalculateColumnWidths(List<Pawn> pawns, float totalWidth)
		{
			PawnTable.columnWidths.Clear();
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < PawnTable.columns.Count; i++)
			{
				float num3 = Mathf.Max((float)PawnTable.columns[i].Worker.GetMinWidth(pawns), 0f);
				PawnTable.columnWidths.Add(num3);
				num += num3;
				num2 += Mathf.Max((float)PawnTable.columns[i].Worker.GetOptimalWidth(pawns), 0f);
			}
			if (num == totalWidth)
			{
				return;
			}
			if (num > totalWidth)
			{
				float num4 = num - totalWidth;
				for (int j = 0; j < PawnTable.columnWidths.Count; j++)
				{
					List<float> list;
					List<float> expr_A7 = list = PawnTable.columnWidths;
					int index;
					int expr_AC = index = j;
					float num5 = list[index];
					expr_A7[expr_AC] = num5 - num4 * Mathf.Max((float)PawnTable.columns[j].Worker.GetOptimalWidth(pawns), 0f) / num2;
				}
				return;
			}
			PawnTable.columnsInWidthPriorityOrder.Clear();
			PawnTable.columnsInWidthPriorityOrder.AddRange(PawnTable.columns);
			PawnTable.columnsInWidthPriorityOrder.SortByDescending((PawnColumnDef x) => x.widthPriority);
			for (int k = 0; k < PawnTable.columnsInWidthPriorityOrder.Count; k++)
			{
				PawnColumnWorker worker = PawnTable.columnsInWidthPriorityOrder[k].Worker;
				float a = Mathf.Max((float)worker.GetOptimalWidth(pawns), 0f) - Mathf.Max((float)worker.GetMinWidth(pawns), 0f);
				float num6 = Mathf.Min(a, totalWidth - num);
				List<float> list2;
				List<float> expr_196 = list2 = PawnTable.columnWidths;
				int index;
				int expr_1AF = index = PawnTable.columns.IndexOf(PawnTable.columnsInWidthPriorityOrder[k]);
				float num5 = list2[index];
				expr_196[expr_1AF] = num5 + num6;
				num += num6;
				if (num >= totalWidth)
				{
					return;
				}
			}
			float num7 = totalWidth - num;
			int num8 = 0;
			while (true)
			{
				num8++;
				if (num8 >= 10000)
				{
					break;
				}
				float num9 = num7;
				bool flag = false;
				for (int l = 0; l < PawnTable.columnWidths.Count; l++)
				{
					if (!PawnTable.columnAtMaxWidth[l])
					{
						float num10 = num7 * Mathf.Max((float)PawnTable.columns[l].Worker.GetOptimalWidth(pawns), 0f) / num2;
						float num11 = Mathf.Max((float)PawnTable.columns[l].Worker.GetMaxWidth(pawns), 0f) - PawnTable.columnWidths[l];
						if (num10 >= num11)
						{
							num10 = num11;
							PawnTable.columnAtMaxWidth[l] = true;
						}
						else
						{
							flag = true;
						}
						if (num10 > 0f)
						{
							List<float> list3;
							List<float> expr_2C6 = list3 = PawnTable.columnWidths;
							int index;
							int expr_2CB = index = l;
							float num5 = list3[index];
							expr_2C6[expr_2CB] = num5 + num10;
							num9 -= num10;
						}
					}
				}
				num7 = num9;
				if (num7 <= 0f)
				{
					goto Block_13;
				}
				if (!flag)
				{
					goto Block_14;
				}
				num2 = 0f;
				for (int m = 0; m < PawnTable.columnWidths.Count; m++)
				{
					if (!PawnTable.columnAtMaxWidth[m])
					{
						num2 += Mathf.Max((float)PawnTable.columns[m].Worker.GetOptimalWidth(pawns), 0f);
					}
				}
				if (num2 <= 0f)
				{
					return;
				}
			}
			Log.Error("Too many iterations.");
			Block_13:
			return;
			Block_14:
			PawnTable.DistributeRemainingWidthProportionallyAboveMax(num7, pawns);
		}

		private static void DistributeRemainingWidthProportionallyAboveMax(float toDistribute, List<Pawn> pawns)
		{
			float num = 0f;
			for (int i = 0; i < PawnTable.columns.Count; i++)
			{
				num += Mathf.Max((float)PawnTable.columns[i].Worker.GetOptimalWidth(pawns), 0f);
			}
			for (int j = 0; j < PawnTable.columns.Count; j++)
			{
				List<float> list;
				List<float> expr_51 = list = PawnTable.columnWidths;
				int index;
				int expr_54 = index = j;
				float num2 = list[index];
				expr_51[expr_54] = num2 + toDistribute * Mathf.Max((float)PawnTable.columns[j].Worker.GetOptimalWidth(pawns), 0f) / num;
			}
		}
	}
}
