using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020002B5 RID: 693
	[StaticConstructorOnStartup]
	public abstract class PawnColumnWorker
	{
		// Token: 0x040006AA RID: 1706
		public PawnColumnDef def;

		// Token: 0x040006AB RID: 1707
		protected const int DefaultCellHeight = 30;

		// Token: 0x040006AC RID: 1708
		private static readonly Texture2D SortingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Sorting", true);

		// Token: 0x040006AD RID: 1709
		private static readonly Texture2D SortingDescendingIcon = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending", true);

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x000685F0 File Offset: 0x000669F0
		protected virtual Color DefaultHeaderColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0006860C File Offset: 0x00066A0C
		protected virtual GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Small;
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00068624 File Offset: 0x00066A24
		public virtual void DoHeader(Rect rect, PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				GUI.color = this.DefaultHeaderColor;
				Text.Anchor = TextAnchor.LowerCenter;
				Rect rect2 = rect;
				rect2.y += 3f;
				Widgets.Label(rect2, this.def.LabelCap.Truncate(rect.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			else if (this.def.HeaderIcon != null)
			{
				Vector2 headerIconSize = this.def.HeaderIconSize;
				int num = (int)((rect.width - headerIconSize.x) / 2f);
				Rect position = new Rect(rect.x + (float)num, rect.yMax - headerIconSize.y, headerIconSize.x, headerIconSize.y);
				GUI.DrawTexture(position, this.def.HeaderIcon);
			}
			if (table.SortingBy == this.def)
			{
				Texture2D texture2D = (!table.SortingDescending) ? PawnColumnWorker.SortingIcon : PawnColumnWorker.SortingDescendingIcon;
				Rect position2 = new Rect(rect.xMax - (float)texture2D.width - 1f, rect.yMax - (float)texture2D.height - 1f, (float)texture2D.width, (float)texture2D.height);
				GUI.DrawTexture(position2, texture2D);
			}
			if (this.def.HeaderInteractable)
			{
				Rect interactableHeaderRect = this.GetInteractableHeaderRect(rect, table);
				Widgets.DrawHighlightIfMouseover(interactableHeaderRect);
				if (interactableHeaderRect.Contains(Event.current.mousePosition))
				{
					string headerTip = this.GetHeaderTip(table);
					if (!headerTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(interactableHeaderRect, headerTip);
					}
				}
				if (Widgets.ButtonInvisible(interactableHeaderRect, false))
				{
					this.HeaderClicked(rect, table);
				}
			}
		}

		// Token: 0x06000B93 RID: 2963
		public abstract void DoCell(Rect rect, Pawn pawn, PawnTable table);

		// Token: 0x06000B94 RID: 2964 RVA: 0x0006881C File Offset: 0x00066C1C
		public virtual int GetMinWidth(PawnTable table)
		{
			int result;
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int num = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).x);
				Text.Font = GameFont.Small;
				result = num;
			}
			else if (this.def.HeaderIcon != null)
			{
				result = Mathf.CeilToInt(this.def.HeaderIconSize.x);
			}
			else
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000688B4 File Offset: 0x00066CB4
		public virtual int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000688D0 File Offset: 0x00066CD0
		public virtual int GetOptimalWidth(PawnTable table)
		{
			return this.GetMinWidth(table);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000688EC File Offset: 0x00066CEC
		public virtual int GetMinCellHeight(Pawn pawn)
		{
			return 30;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00068904 File Offset: 0x00066D04
		public virtual int GetMinHeaderHeight(PawnTable table)
		{
			int result;
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int num = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).y);
				Text.Font = GameFont.Small;
				result = num;
			}
			else if (this.def.HeaderIcon != null)
			{
				result = Mathf.CeilToInt(this.def.HeaderIconSize.y);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0006899C File Offset: 0x00066D9C
		public virtual int Compare(Pawn a, Pawn b)
		{
			return 0;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x000689B4 File Offset: 0x00066DB4
		protected virtual Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			float num = Mathf.Min(25f, headerRect.height);
			return new Rect(headerRect.x, headerRect.yMax - num, headerRect.width, num);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000689F8 File Offset: 0x00066DF8
		protected virtual void HeaderClicked(Rect headerRect, PawnTable table)
		{
			if (this.def.sortable && !Event.current.shift)
			{
				if (Event.current.button == 0)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, true);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					else if (table.SortingDescending)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					else
					{
						table.SortBy(null, false);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
				}
				else if (Event.current.button == 1)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					else if (table.SortingDescending)
					{
						table.SortBy(null, false);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					else
					{
						table.SortBy(this.def, true);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
				}
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00068B30 File Offset: 0x00066F30
		protected virtual string GetHeaderTip(PawnTable table)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.def.headerTip.NullOrEmpty())
			{
				stringBuilder.Append(this.def.headerTip);
			}
			if (this.def.sortable)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ClickToSortByThisColumn".Translate());
			}
			return stringBuilder.ToString();
		}
	}
}
