using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnTable
	{
		private string table;

		private IEnumerable<Pawn> pawns;

		private int minTableWidth;

		private int maxTableWidth;

		private int minTableHeight;

		private int maxTableHeight;

		private Vector2 fixedSize;

		private bool hasFixedSize;

		private bool dirty;

		private List<bool> columnAtMaxWidth = new List<bool>();

		private Vector2 scrollPosition;

		private Vector2 cachedSize;

		private List<Pawn> cachedPawns = new List<Pawn>();

		private List<float> cachedColumnWidths = new List<float>();

		private List<float> cachedRowHeights = new List<float>();

		private List<PawnColumnDef> cachedColumns = new List<PawnColumnDef>();

		private List<PawnColumnDef> cachedColumnsInWidthPriorityOrder = new List<PawnColumnDef>();

		private float cachedHeaderHeight;

		private float cachedHeightNoScrollbar;

		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		public PawnTable(string table, IEnumerable<Pawn> pawns, int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.table = table;
			this.pawns = pawns;
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.SetDirty();
		}

		public PawnTable(string table, IEnumerable<Pawn> pawns, Vector2 size)
		{
			this.table = table;
			this.pawns = pawns;
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		public void PawnTableOnGUI(Vector2 position)
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.RecacheIfDirty();
			int num = 0;
			for (int i = 0; i < this.cachedColumns.Count; i++)
			{
				int num2;
				if (i == this.cachedColumns.Count - 1)
				{
					num2 = (int)(this.cachedSize.x - (float)num);
				}
				else
				{
					num2 = (int)this.cachedColumnWidths[i];
				}
				Rect rect = new Rect((float)((int)position.x + num), (float)((int)position.y), (float)num2, (float)((int)this.cachedHeaderHeight));
				this.cachedColumns[i].Worker.DoHeader(rect, this.cachedPawns);
				num += num2;
			}
			Rect outRect = new Rect((float)((int)position.x), (float)((int)position.y + (int)this.cachedHeaderHeight), (float)((int)this.cachedSize.x), (float)((int)this.cachedSize.y - (int)this.cachedHeaderHeight));
			Rect viewRect = new Rect(0f, 0f, outRect.width, (float)((int)this.cachedHeightNoScrollbar));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect);
			int num3 = 0;
			for (int j = 0; j < this.cachedPawns.Count; j++)
			{
				num = 0;
				for (int k = 0; k < this.cachedColumns.Count; k++)
				{
					int num4;
					if (k == this.cachedColumns.Count - 1)
					{
						num4 = (int)(this.cachedSize.x - (float)num);
					}
					else
					{
						num4 = (int)this.cachedColumnWidths[k];
					}
					Rect rect2 = new Rect((float)num, (float)num3, (float)num4, (float)((int)this.cachedRowHeights[k]));
					this.cachedColumns[k].Worker.DoCell(rect2, this.cachedPawns[j]);
					num += num4;
				}
				num3 += (int)this.cachedRowHeights[j];
			}
			Widgets.EndScrollView();
		}

		public void SetDirty()
		{
			this.dirty = true;
		}

		private void RecacheIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.RecachePawns();
			this.RecacheColumns();
			this.RecacheColumnWidths();
			this.RecacheRowHeights();
			this.cachedHeaderHeight = this.CalculateHeaderHeight();
			this.cachedHeightNoScrollbar = this.CalculateTotalRequiredHeight();
			this.RecacheSize();
		}

		private void RecachePawns()
		{
			this.cachedPawns.AddRange(this.pawns);
		}

		private void RecacheColumns()
		{
			this.cachedColumns.Clear();
			List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].tables.Contains(this.table))
				{
					this.cachedColumns.Add(allDefsListForReading[i]);
				}
			}
			this.cachedColumns.SortBy((PawnColumnDef x) => x.order);
		}

		private void RecacheColumnWidths()
		{
			this.cachedColumnWidths.Clear();
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this.cachedColumns.Count; i++)
			{
				float minWidth = this.GetMinWidth(this.cachedColumns[i]);
				this.cachedColumnWidths.Add(minWidth);
				num += minWidth;
				num2 += this.GetOptimalWidth(this.cachedColumns[i]);
			}
			if (num == this.cachedSize.x)
			{
				return;
			}
			if (num > this.cachedSize.x)
			{
				float num3 = num - this.cachedSize.x;
				for (int j = 0; j < this.cachedColumnWidths.Count; j++)
				{
					List<float> list;
					List<float> expr_AB = list = this.cachedColumnWidths;
					int index;
					int expr_B0 = index = j;
					float num4 = list[index];
					expr_AB[expr_B0] = num4 - num3 * this.GetOptimalWidth(this.cachedColumns[j]) / num2;
				}
				return;
			}
			this.cachedColumnsInWidthPriorityOrder.Clear();
			this.cachedColumnsInWidthPriorityOrder.AddRange(this.cachedColumns);
			this.cachedColumnsInWidthPriorityOrder.SortByDescending((PawnColumnDef x) => x.widthPriority);
			for (int k = 0; k < this.cachedColumnsInWidthPriorityOrder.Count; k++)
			{
				float a = this.GetOptimalWidth(this.cachedColumnsInWidthPriorityOrder[k]) - this.GetMinWidth(this.cachedColumnsInWidthPriorityOrder[k]);
				float num5 = Mathf.Min(a, this.cachedSize.x - num);
				List<float> list2;
				List<float> expr_188 = list2 = this.cachedColumnWidths;
				int index;
				int expr_1A3 = index = this.cachedColumns.IndexOf(this.cachedColumnsInWidthPriorityOrder[k]);
				float num4 = list2[index];
				expr_188[expr_1A3] = num4 + num5;
				num += num5;
				if (num >= this.cachedSize.x)
				{
					return;
				}
			}
			float num6 = this.cachedSize.x - num;
			int num7 = 0;
			while (true)
			{
				num7++;
				if (num7 >= 10000)
				{
					break;
				}
				float num8 = num6;
				bool flag = false;
				for (int l = 0; l < this.cachedColumnWidths.Count; l++)
				{
					if (!this.columnAtMaxWidth[l])
					{
						float num9 = num6 * this.GetOptimalWidth(this.cachedColumns[l]) / num2;
						float num10 = this.GetMaxWidth(this.cachedColumns[l]) - this.cachedColumnWidths[l];
						if (num9 >= num10)
						{
							num9 = num10;
							this.columnAtMaxWidth[l] = true;
						}
						else
						{
							flag = true;
						}
						if (num9 > 0f)
						{
							List<float> list3;
							List<float> expr_2B5 = list3 = this.cachedColumnWidths;
							int index;
							int expr_2BA = index = l;
							float num4 = list3[index];
							expr_2B5[expr_2BA] = num4 + num9;
							num8 -= num9;
						}
					}
				}
				num6 = num8;
				if (num6 <= 0f)
				{
					goto Block_13;
				}
				if (!flag)
				{
					goto Block_14;
				}
				num2 = 0f;
				for (int m = 0; m < this.cachedColumnWidths.Count; m++)
				{
					if (!this.columnAtMaxWidth[m])
					{
						num2 += this.GetOptimalWidth(this.cachedColumns[m]);
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
			this.DistributeRemainingWidthProportionallyAboveMax(num6);
		}

		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		private void RecacheSize()
		{
			if (this.hasFixedSize)
			{
				this.cachedSize = this.fixedSize;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < this.cachedColumns.Count; i++)
				{
					num += this.GetOptimalWidth(this.cachedColumns[i]);
				}
				float num2 = Mathf.Clamp(num, (float)this.minTableWidth, (float)this.maxTableWidth);
				float num3 = Mathf.Clamp(this.cachedHeightNoScrollbar, (float)this.minTableHeight, (float)this.maxTableHeight);
				num2 = Mathf.Min(num2, (float)UI.screenWidth);
				num3 = Mathf.Min(num3, (float)UI.screenHeight);
				this.cachedSize = new Vector2(num2, num3);
			}
		}

		private void DistributeRemainingWidthProportionallyAboveMax(float toDistribute)
		{
			float num = 0f;
			for (int i = 0; i < this.cachedColumns.Count; i++)
			{
				num += this.GetOptimalWidth(this.cachedColumns[i]);
			}
			for (int j = 0; j < this.cachedColumns.Count; j++)
			{
				List<float> list;
				List<float> expr_44 = list = this.cachedColumnWidths;
				int index;
				int expr_47 = index = j;
				float num2 = list[index];
				expr_44[expr_47] = num2 + toDistribute * this.GetOptimalWidth(this.cachedColumns[j]) / num;
			}
		}

		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this.cachedPawns), 0f);
		}

		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this.cachedPawns), 0f);
		}

		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this.cachedPawns), 0f);
		}

		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			int num2 = 0;
			if (num2 >= this.cachedColumns.Count)
			{
				return num;
			}
			return Mathf.Max(num, (float)this.cachedColumns[num2].Worker.GetHeight(pawn));
		}

		private float CalculateHeaderHeight()
		{
			float num = 0f;
			int num2 = 0;
			if (num2 >= this.cachedColumns.Count)
			{
				return num;
			}
			return Mathf.Max(num, (float)this.cachedColumns[num2].Worker.GetHeaderHeight(this.cachedPawns));
		}

		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}
	}
}
