using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000887 RID: 2183
	public abstract class PawnColumnWorker_Icon : PawnColumnWorker
	{
		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060031CE RID: 12750 RVA: 0x001AEA58 File Offset: 0x001ACE58
		protected virtual int Width
		{
			get
			{
				return 26;
			}
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x001AEA70 File Offset: 0x001ACE70
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (iconFor != null)
			{
				Vector2 iconSize = this.GetIconSize(pawn);
				int num = (int)((rect.width - iconSize.x) / 2f);
				int num2 = Mathf.Max((int)((30f - iconSize.y) / 2f), 0);
				Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, iconSize.x, iconSize.y);
				GUI.color = this.GetIconColor(pawn);
				GUI.DrawTexture(rect2, iconFor);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect2))
				{
					string iconTip = this.GetIconTip(pawn);
					if (!iconTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, iconTip);
					}
				}
				if (Widgets.ButtonInvisible(rect2, false))
				{
					this.ClickedIcon(pawn);
				}
				if (Mouse.IsOver(rect2) && Input.GetMouseButton(0))
				{
					this.PaintedIcon(pawn);
				}
			}
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x001AEB78 File Offset: 0x001ACF78
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x001AEBA0 File Offset: 0x001ACFA0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x001AEBC8 File Offset: 0x001ACFC8
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(this.GetIconSize(pawn).y));
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x001AEC00 File Offset: 0x001AD000
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x001AEC2C File Offset: 0x001AD02C
		private int GetValueToCompare(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			return (!(iconFor != null)) ? int.MinValue : iconFor.GetInstanceID();
		}

		// Token: 0x060031D5 RID: 12757
		protected abstract Texture2D GetIconFor(Pawn pawn);

		// Token: 0x060031D6 RID: 12758 RVA: 0x001AEC68 File Offset: 0x001AD068
		protected virtual string GetIconTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x001AEC80 File Offset: 0x001AD080
		protected virtual Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x001AEC9A File Offset: 0x001AD09A
		protected virtual void ClickedIcon(Pawn pawn)
		{
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x001AEC9D File Offset: 0x001AD09D
		protected virtual void PaintedIcon(Pawn pawn)
		{
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x001AECA0 File Offset: 0x001AD0A0
		protected virtual Vector2 GetIconSize(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			Vector2 result;
			if (iconFor == null)
			{
				result = Vector2.zero;
			}
			else
			{
				result = new Vector2((float)iconFor.width, (float)iconFor.height);
			}
			return result;
		}
	}
}
