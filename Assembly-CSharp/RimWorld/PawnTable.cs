using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000898 RID: 2200
	public class PawnTable
	{
		// Token: 0x06003255 RID: 12885 RVA: 0x001B17D8 File Offset: 0x001AFBD8
		public PawnTable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
		{
			this.def = def;
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(def.minWidth, uiWidth, 0, uiHeight);
			this.SetDirty();
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06003256 RID: 12886 RVA: 0x001B1848 File Offset: 0x001AFC48
		public List<PawnColumnDef> ColumnsListForReading
		{
			get
			{
				return this.def.columns;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x001B1868 File Offset: 0x001AFC68
		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06003258 RID: 12888 RVA: 0x001B1884 File Offset: 0x001AFC84
		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x001B18B0 File Offset: 0x001AFCB0
		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x001B18D4 File Offset: 0x001AFCD4
		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x001B18F8 File Offset: 0x001AFCF8
		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x001B191C File Offset: 0x001AFD1C
		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x001B1940 File Offset: 0x001AFD40
		public void PawnTableOnGUI(Vector2 position)
		{
			if (Event.current.type != EventType.Layout)
			{
				this.RecacheIfDirty();
				float num = this.cachedSize.x - 16f;
				int num2 = 0;
				for (int i = 0; i < this.def.columns.Count; i++)
				{
					int num3;
					if (i == this.def.columns.Count - 1)
					{
						num3 = (int)(num - (float)num2);
					}
					else
					{
						num3 = (int)this.cachedColumnWidths[i];
					}
					Rect rect = new Rect((float)((int)position.x + num2), (float)((int)position.y), (float)num3, (float)((int)this.cachedHeaderHeight));
					this.def.columns[i].Worker.DoHeader(rect, this);
					num2 += num3;
				}
				Rect outRect = new Rect((float)((int)position.x), (float)((int)position.y + (int)this.cachedHeaderHeight), (float)((int)this.cachedSize.x), (float)((int)this.cachedSize.y - (int)this.cachedHeaderHeight));
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)((int)this.cachedHeightNoScrollbar - (int)this.cachedHeaderHeight));
				Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
				int num4 = 0;
				for (int j = 0; j < this.cachedPawns.Count; j++)
				{
					num2 = 0;
					if ((float)num4 - this.scrollPosition.y + (float)((int)this.cachedRowHeights[j]) >= 0f && (float)num4 - this.scrollPosition.y <= outRect.height)
					{
						GUI.color = new Color(1f, 1f, 1f, 0.2f);
						Widgets.DrawLineHorizontal(0f, (float)num4, viewRect.width);
						GUI.color = Color.white;
						Rect rect2 = new Rect(0f, (float)num4, viewRect.width, (float)((int)this.cachedRowHeights[j]));
						if (Mouse.IsOver(rect2))
						{
							GUI.DrawTexture(rect2, TexUI.HighlightTex);
						}
						for (int k = 0; k < this.def.columns.Count; k++)
						{
							int num5;
							if (k == this.def.columns.Count - 1)
							{
								num5 = (int)(num - (float)num2);
							}
							else
							{
								num5 = (int)this.cachedColumnWidths[k];
							}
							Rect rect3 = new Rect((float)num2, (float)num4, (float)num5, (float)((int)this.cachedRowHeights[j]));
							this.def.columns[k].Worker.DoCell(rect3, this.cachedPawns[j], this);
							num2 += num5;
						}
						if (this.cachedPawns[j].Downed)
						{
							GUI.color = new Color(1f, 0f, 0f, 0.5f);
							Widgets.DrawLineHorizontal(0f, rect2.center.y, viewRect.width);
							GUI.color = Color.white;
						}
					}
					num4 += (int)this.cachedRowHeights[j];
				}
				Widgets.EndScrollView();
			}
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x001B1CAF File Offset: 0x001B00AF
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x001B1CB9 File Offset: 0x001B00B9
		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x001B1CE6 File Offset: 0x001B00E6
		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x001B1CFD File Offset: 0x001B00FD
		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x001B1D14 File Offset: 0x001B0114
		private void RecacheIfDirty()
		{
			if (this.dirty)
			{
				this.dirty = false;
				this.RecachePawns();
				this.RecacheRowHeights();
				this.cachedHeaderHeight = this.CalculateHeaderHeight();
				this.cachedHeightNoScrollbar = this.CalculateTotalRequiredHeight();
				this.RecacheSize();
				this.RecacheColumnWidths();
			}
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x001B1D6C File Offset: 0x001B016C
		private void RecachePawns()
		{
			this.cachedPawns.Clear();
			this.cachedPawns.AddRange(this.pawnsGetter());
			this.cachedPawns = this.LabelSortFunction(this.cachedPawns).ToList<Pawn>();
			if (this.sortByColumn != null)
			{
				if (this.sortDescending)
				{
					this.cachedPawns.SortStable(new Func<Pawn, Pawn, int>(this.sortByColumn.Worker.Compare));
				}
				else
				{
					this.cachedPawns.SortStable((Pawn a, Pawn b) => this.sortByColumn.Worker.Compare(b, a));
				}
			}
			this.cachedPawns = this.PrimarySortFunction(this.cachedPawns).ToList<Pawn>();
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x001B1E20 File Offset: 0x001B0220
		protected virtual IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Label
			select p;
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x001B1E58 File Offset: 0x001B0258
		protected virtual IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return input;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x001B1E70 File Offset: 0x001B0270
		private void RecacheColumnWidths()
		{
			float num = this.cachedSize.x - 16f;
			float num2 = 0f;
			this.RecacheColumnWidths_StartWithMinWidths(out num2);
			if (num2 != num)
			{
				if (num2 > num)
				{
					this.SubtractProportionally(num2 - num, num2);
				}
				else
				{
					bool flag;
					this.RecacheColumnWidths_DistributeUntilOptimal(num, ref num2, out flag);
					if (!flag)
					{
						this.RecacheColumnWidths_DistributeAboveOptimal(num, ref num2);
					}
				}
			}
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x001B1EE4 File Offset: 0x001B02E4
		private void RecacheColumnWidths_StartWithMinWidths(out float minWidthsSum)
		{
			minWidthsSum = 0f;
			this.cachedColumnWidths.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				float minWidth = this.GetMinWidth(this.def.columns[i]);
				this.cachedColumnWidths.Add(minWidth);
				minWidthsSum += minWidth;
			}
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x001B1F54 File Offset: 0x001B0354
		private void RecacheColumnWidths_DistributeUntilOptimal(float totalAvailableSpaceForColumns, ref float usedWidth, out bool noMoreFreeSpace)
		{
			this.columnAtOptimalWidth.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				this.columnAtOptimalWidth.Add(this.cachedColumnWidths[i] >= this.GetOptimalWidth(this.def.columns[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = float.MinValue;
				for (int j = 0; j < this.def.columns.Count; j++)
				{
					if (!this.columnAtOptimalWidth[j])
					{
						num2 = Mathf.Max(num2, (float)this.def.columns[j].widthPriority);
					}
				}
				float num3 = 0f;
				for (int k = 0; k < this.cachedColumnWidths.Count; k++)
				{
					if (!this.columnAtOptimalWidth[k])
					{
						if ((float)this.def.columns[k].widthPriority == num2)
						{
							num3 += this.GetOptimalWidth(this.def.columns[k]);
						}
					}
				}
				float num4 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				bool flag2 = false;
				for (int l = 0; l < this.cachedColumnWidths.Count; l++)
				{
					if (!this.columnAtOptimalWidth[l])
					{
						if ((float)this.def.columns[l].widthPriority != num2)
						{
							flag = true;
						}
						else
						{
							float num5 = num4 * this.GetOptimalWidth(this.def.columns[l]) / num3;
							float num6 = this.GetOptimalWidth(this.def.columns[l]) - this.cachedColumnWidths[l];
							if (num5 >= num6)
							{
								num5 = num6;
								this.columnAtOptimalWidth[l] = true;
								flag2 = true;
							}
							else
							{
								flag = true;
							}
							if (num5 > 0f)
							{
								List<float> list;
								int index;
								(list = this.cachedColumnWidths)[index = l] = list[index] + num5;
								usedWidth += num5;
							}
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					goto Block_13;
				}
				if (!flag)
				{
					goto Block_14;
				}
				if (!flag2)
				{
					goto Block_15;
				}
			}
			Log.Error("Too many iterations.", false);
			goto IL_2A8;
			Block_13:
			noMoreFreeSpace = true;
			Block_14:
			Block_15:
			IL_2A8:
			noMoreFreeSpace = false;
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x001B220C File Offset: 0x001B060C
		private void RecacheColumnWidths_DistributeAboveOptimal(float totalAvailableSpaceForColumns, ref float usedWidth)
		{
			this.columnAtMaxWidth.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				this.columnAtMaxWidth.Add(this.cachedColumnWidths[i] >= this.GetMaxWidth(this.def.columns[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = 0f;
				for (int j = 0; j < this.def.columns.Count; j++)
				{
					if (!this.columnAtMaxWidth[j])
					{
						num2 += Mathf.Max(this.GetOptimalWidth(this.def.columns[j]), 1f);
					}
				}
				float num3 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				for (int k = 0; k < this.def.columns.Count; k++)
				{
					if (!this.columnAtMaxWidth[k])
					{
						float num4 = num3 * Mathf.Max(this.GetOptimalWidth(this.def.columns[k]), 1f) / num2;
						float num5 = this.GetMaxWidth(this.def.columns[k]) - this.cachedColumnWidths[k];
						if (num4 >= num5)
						{
							num4 = num5;
							this.columnAtMaxWidth[k] = true;
						}
						else
						{
							flag = true;
						}
						if (num4 > 0f)
						{
							List<float> list;
							int index;
							(list = this.cachedColumnWidths)[index = k] = list[index] + num4;
							usedWidth += num4;
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					goto Block_9;
				}
				if (!flag)
				{
					goto Block_10;
				}
			}
			Log.Error("Too many iterations.", false);
			Block_9:
			return;
			Block_10:
			this.DistributeRemainingWidthProportionallyAboveMax(totalAvailableSpaceForColumns - usedWidth);
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x001B2420 File Offset: 0x001B0820
		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x001B2474 File Offset: 0x001B0874
		private void RecacheSize()
		{
			if (this.hasFixedSize)
			{
				this.cachedSize = this.fixedSize;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < this.def.columns.Count; i++)
				{
					if (!this.def.columns[i].ignoreWhenCalculatingOptimalTableSize)
					{
						num += this.GetOptimalWidth(this.def.columns[i]);
					}
				}
				float num2 = Mathf.Clamp(num + 16f, (float)this.minTableWidth, (float)this.maxTableWidth);
				float num3 = Mathf.Clamp(this.cachedHeightNoScrollbar, (float)this.minTableHeight, (float)this.maxTableHeight);
				num2 = Mathf.Min(num2, (float)UI.screenWidth);
				num3 = Mathf.Min(num3, (float)UI.screenHeight);
				this.cachedSize = new Vector2(num2, num3);
			}
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x001B2560 File Offset: 0x001B0960
		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list;
				int index;
				(list = this.cachedColumnWidths)[index = i] = list[index] - toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x001B25B4 File Offset: 0x001B09B4
		private void DistributeRemainingWidthProportionallyAboveMax(float toDistribute)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num += Mathf.Max(this.GetOptimalWidth(this.def.columns[i]), 1f);
			}
			for (int j = 0; j < this.def.columns.Count; j++)
			{
				List<float> list;
				int index;
				(list = this.cachedColumnWidths)[index = j] = list[index] + toDistribute * Mathf.Max(this.GetOptimalWidth(this.def.columns[j]), 1f) / num;
			}
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x001B2674 File Offset: 0x001B0A74
		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x001B26A0 File Offset: 0x001B0AA0
		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x001B26CC File Offset: 0x001B0ACC
		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x001B26F8 File Offset: 0x001B0AF8
		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x001B275C File Offset: 0x001B0B5C
		private float CalculateHeaderHeight()
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x001B27C0 File Offset: 0x001B0BC0
		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}

		// Token: 0x04001AE5 RID: 6885
		private PawnTableDef def;

		// Token: 0x04001AE6 RID: 6886
		private Func<IEnumerable<Pawn>> pawnsGetter;

		// Token: 0x04001AE7 RID: 6887
		private int minTableWidth;

		// Token: 0x04001AE8 RID: 6888
		private int maxTableWidth;

		// Token: 0x04001AE9 RID: 6889
		private int minTableHeight;

		// Token: 0x04001AEA RID: 6890
		private int maxTableHeight;

		// Token: 0x04001AEB RID: 6891
		private Vector2 fixedSize;

		// Token: 0x04001AEC RID: 6892
		private bool hasFixedSize;

		// Token: 0x04001AED RID: 6893
		private bool dirty;

		// Token: 0x04001AEE RID: 6894
		private List<bool> columnAtMaxWidth = new List<bool>();

		// Token: 0x04001AEF RID: 6895
		private List<bool> columnAtOptimalWidth = new List<bool>();

		// Token: 0x04001AF0 RID: 6896
		private Vector2 scrollPosition;

		// Token: 0x04001AF1 RID: 6897
		private PawnColumnDef sortByColumn;

		// Token: 0x04001AF2 RID: 6898
		private bool sortDescending;

		// Token: 0x04001AF3 RID: 6899
		private Vector2 cachedSize;

		// Token: 0x04001AF4 RID: 6900
		private List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x04001AF5 RID: 6901
		private List<float> cachedColumnWidths = new List<float>();

		// Token: 0x04001AF6 RID: 6902
		private List<float> cachedRowHeights = new List<float>();

		// Token: 0x04001AF7 RID: 6903
		private float cachedHeaderHeight;

		// Token: 0x04001AF8 RID: 6904
		private float cachedHeightNoScrollbar;
	}
}
