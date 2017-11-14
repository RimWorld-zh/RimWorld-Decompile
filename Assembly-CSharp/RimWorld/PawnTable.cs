using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnTable
	{
		private Func<IEnumerable<Pawn>> pawnsGetter;

		private List<PawnColumnDef> columns;

		private int minTableWidth;

		private int maxTableWidth;

		private int minTableHeight;

		private int maxTableHeight;

		private Vector2 fixedSize;

		private bool hasFixedSize;

		private bool dirty;

		private List<bool> columnAtMaxWidth = new List<bool>();

		private List<bool> columnAtOptimalWidth = new List<bool>();

		private Vector2 scrollPosition;

		private PawnColumnDef sortByColumn;

		private bool sortDescending;

		private Vector2 cachedSize;

		private List<Pawn> cachedPawns = new List<Pawn>();

		private List<float> cachedColumnWidths = new List<float>();

		private List<float> cachedRowHeights = new List<float>();

		private float cachedHeaderHeight;

		private float cachedHeightNoScrollbar;

		public List<PawnColumnDef> ColumnsListForReading
		{
			get
			{
				return this.columns;
			}
		}

		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

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

		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		public PawnTable(PawnTableDef table, Func<IEnumerable<Pawn>> pawnsGetter, int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
			: this(table.columns, pawnsGetter, minTableWidth, maxTableWidth, minTableHeight, maxTableHeight)
		{
		}

		public PawnTable(IEnumerable<PawnColumnDef> columns, Func<IEnumerable<Pawn>> pawnsGetter, int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.columns = columns.ToList();
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(minTableWidth, maxTableWidth, minTableHeight, maxTableHeight);
			this.SetDirty();
		}

		public PawnTable(PawnTableDef table, Func<IEnumerable<Pawn>> pawnsGetter, Vector2 size)
			: this(table.columns, pawnsGetter, size)
		{
		}

		public PawnTable(IEnumerable<PawnColumnDef> columns, Func<IEnumerable<Pawn>> pawnsGetter, Vector2 size)
		{
			this.columns = columns.ToList();
			this.pawnsGetter = pawnsGetter;
			this.SetFixedSize(this.fixedSize);
			this.SetDirty();
		}

		public void PawnTableOnGUI(Vector2 position)
		{
			if (Event.current.type != EventType.Layout)
			{
				this.RecacheIfDirty();
				float num = (float)(this.cachedSize.x - 16.0);
				int num2 = 0;
				for (int i = 0; i < this.columns.Count; i++)
				{
					int num3 = (i != this.columns.Count - 1) ? ((int)this.cachedColumnWidths[i]) : ((int)(num - (float)num2));
					Rect rect = new Rect((float)((int)position.x + num2), (float)(int)position.y, (float)num3, (float)(int)this.cachedHeaderHeight);
					this.columns[i].Worker.DoHeader(rect, this);
					num2 += num3;
				}
				Rect outRect = new Rect((float)(int)position.x, (float)((int)position.y + (int)this.cachedHeaderHeight), (float)(int)this.cachedSize.x, (float)((int)this.cachedSize.y - (int)this.cachedHeaderHeight));
				Rect viewRect = new Rect(0f, 0f, (float)(outRect.width - 16.0), (float)((int)this.cachedHeightNoScrollbar - (int)this.cachedHeaderHeight));
				Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
				int num4 = 0;
				for (int j = 0; j < this.cachedPawns.Count; j++)
				{
					num2 = 0;
					if (!((float)num4 - this.scrollPosition.y + (float)(int)this.cachedRowHeights[j] < 0.0) && !((float)num4 - this.scrollPosition.y > outRect.height))
					{
						GUI.color = new Color(1f, 1f, 1f, 0.2f);
						Widgets.DrawLineHorizontal(0f, (float)num4, viewRect.width);
						GUI.color = Color.white;
						Rect rect2 = new Rect(0f, (float)num4, viewRect.width, (float)(int)this.cachedRowHeights[j]);
						if (Mouse.IsOver(rect2))
						{
							GUI.DrawTexture(rect2, TexUI.HighlightTex);
						}
						for (int k = 0; k < this.columns.Count; k++)
						{
							int num5 = (k != this.columns.Count - 1) ? ((int)this.cachedColumnWidths[k]) : ((int)(num - (float)num2));
							Rect rect3 = new Rect((float)num2, (float)num4, (float)num5, (float)(int)this.cachedRowHeights[j]);
							this.columns[k].Worker.DoCell(rect3, this.cachedPawns[j], this);
							num2 += num5;
						}
						if (this.cachedPawns[j].Downed)
						{
							GUI.color = new Color(1f, 0f, 0f, 0.5f);
							Vector2 center = rect2.center;
							Widgets.DrawLineHorizontal(0f, center.y, viewRect.width);
							GUI.color = Color.white;
						}
					}
					num4 += (int)this.cachedRowHeights[j];
				}
				Widgets.EndScrollView();
			}
		}

		public void SetDirty()
		{
			this.dirty = true;
		}

		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

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

		private void RecachePawns()
		{
			this.cachedPawns.Clear();
			this.cachedPawns.AddRange(this.pawnsGetter());
			if (this.sortByColumn != null)
			{
				if (this.sortDescending)
				{
					this.cachedPawns.Sort(delegate(Pawn a, Pawn b)
					{
						int num2 = this.sortByColumn.Worker.Compare(b, a);
						if (num2 == 0)
						{
							return b.Label.CompareTo(a.Label);
						}
						return num2;
					});
				}
				else
				{
					this.cachedPawns.Sort(delegate(Pawn a, Pawn b)
					{
						int num = this.sortByColumn.Worker.Compare(a, b);
						if (num == 0)
						{
							return a.Label.CompareTo(b.Label);
						}
						return num;
					});
				}
			}
		}

		private void RecacheColumnWidths()
		{
			float num = (float)(this.cachedSize.x - 16.0);
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
					bool flag = default(bool);
					this.RecacheColumnWidths_DistributeUntilOptimal(num, ref num2, out flag);
					if (!flag)
					{
						this.RecacheColumnWidths_DistributeAboveOptimal(num, ref num2);
					}
				}
			}
		}

		private void RecacheColumnWidths_StartWithMinWidths(out float minWidthsSum)
		{
			minWidthsSum = 0f;
			this.cachedColumnWidths.Clear();
			for (int i = 0; i < this.columns.Count; i++)
			{
				float minWidth = this.GetMinWidth(this.columns[i]);
				this.cachedColumnWidths.Add(minWidth);
				minWidthsSum += minWidth;
			}
		}

		private void RecacheColumnWidths_DistributeUntilOptimal(float totalAvailableSpaceForColumns, ref float usedWidth, out bool noMoreFreeSpace)
		{
			this.columnAtOptimalWidth.Clear();
			for (int i = 0; i < this.columns.Count; i++)
			{
				this.columnAtOptimalWidth.Add(this.cachedColumnWidths[i] >= this.GetOptimalWidth(this.columns[i]));
			}
			int num = 0;
			while (true)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.");
				}
				else
				{
					float num2 = -3.40282347E+38f;
					for (int j = 0; j < this.columns.Count; j++)
					{
						if (!this.columnAtOptimalWidth[j])
						{
							num2 = Mathf.Max(num2, (float)this.columns[j].widthPriority);
						}
					}
					float num3 = 0f;
					for (int k = 0; k < this.cachedColumnWidths.Count; k++)
					{
						if (!this.columnAtOptimalWidth[k] && (float)this.columns[k].widthPriority == num2)
						{
							num3 += this.GetOptimalWidth(this.columns[k]);
						}
					}
					float num4 = totalAvailableSpaceForColumns - usedWidth;
					bool flag = false;
					bool flag2 = false;
					for (int l = 0; l < this.cachedColumnWidths.Count; l++)
					{
						if (!this.columnAtOptimalWidth[l])
						{
							if ((float)this.columns[l].widthPriority != num2)
							{
								flag = true;
							}
							else
							{
								float num5 = num4 * this.GetOptimalWidth(this.columns[l]) / num3;
								float num6 = this.GetOptimalWidth(this.columns[l]) - this.cachedColumnWidths[l];
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
								if (num5 > 0.0)
								{
									List<float> list;
									int index;
									(list = this.cachedColumnWidths)[index = l] = list[index] + num5;
									usedWidth += num5;
								}
							}
						}
					}
					if (usedWidth >= totalAvailableSpaceForColumns - 0.10000000149011612)
					{
						noMoreFreeSpace = true;
					}
					else if (flag && flag2)
					{
						continue;
					}
				}
				break;
			}
			noMoreFreeSpace = false;
		}

		private void RecacheColumnWidths_DistributeAboveOptimal(float totalAvailableSpaceForColumns, ref float usedWidth)
		{
			this.columnAtMaxWidth.Clear();
			for (int i = 0; i < this.columns.Count; i++)
			{
				this.columnAtMaxWidth.Add(this.cachedColumnWidths[i] >= this.GetMaxWidth(this.columns[i]));
			}
			int num = 0;
			while (true)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.");
				}
				else
				{
					float num2 = 0f;
					for (int j = 0; j < this.columns.Count; j++)
					{
						if (!this.columnAtMaxWidth[j])
						{
							num2 += Mathf.Max(this.GetOptimalWidth(this.columns[j]), 1f);
						}
					}
					float num3 = totalAvailableSpaceForColumns - usedWidth;
					bool flag = false;
					for (int k = 0; k < this.columns.Count; k++)
					{
						if (!this.columnAtMaxWidth[k])
						{
							float num4 = num3 * Mathf.Max(this.GetOptimalWidth(this.columns[k]), 1f) / num2;
							float num5 = this.GetMaxWidth(this.columns[k]) - this.cachedColumnWidths[k];
							if (num4 >= num5)
							{
								num4 = num5;
								this.columnAtMaxWidth[k] = true;
							}
							else
							{
								flag = true;
							}
							if (num4 > 0.0)
							{
								List<float> list;
								int index;
								(list = this.cachedColumnWidths)[index = k] = list[index] + num4;
								usedWidth += num4;
							}
						}
					}
					if (!(usedWidth >= totalAvailableSpaceForColumns - 0.10000000149011612))
					{
						if (!flag)
							break;
						continue;
					}
				}
				return;
			}
			this.DistributeRemainingWidthProportionallyAboveMax(totalAvailableSpaceForColumns - usedWidth);
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
				for (int i = 0; i < this.columns.Count; i++)
				{
					if (!this.columns[i].ignoreWhenCalculatingOptimalTableSize)
					{
						num += this.GetOptimalWidth(this.columns[i]);
					}
				}
				float a = Mathf.Clamp((float)(num + 16.0), (float)this.minTableWidth, (float)this.maxTableWidth);
				float a2 = Mathf.Clamp(this.cachedHeightNoScrollbar, (float)this.minTableHeight, (float)this.maxTableHeight);
				a = Mathf.Min(a, (float)UI.screenWidth);
				a2 = Mathf.Min(a2, (float)UI.screenHeight);
				this.cachedSize = new Vector2(a, a2);
			}
		}

		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list;
				int index;
				(list = this.cachedColumnWidths)[index = i] = list[index] - toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		private void DistributeRemainingWidthProportionallyAboveMax(float toDistribute)
		{
			float num = 0f;
			for (int i = 0; i < this.columns.Count; i++)
			{
				num += Mathf.Max(this.GetOptimalWidth(this.columns[i]), 1f);
			}
			for (int j = 0; j < this.columns.Count; j++)
			{
				List<float> list;
				int index;
				(list = this.cachedColumnWidths)[index = j] = list[index] + toDistribute * Mathf.Max(this.GetOptimalWidth(this.columns[j]), 1f) / num;
			}
		}

		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			for (int i = 0; i < this.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.columns[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		private float CalculateHeaderHeight()
		{
			float num = 0f;
			for (int i = 0; i < this.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.columns[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
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
