using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E82 RID: 3714
	public class Listing_Tree : Listing_Lines
	{
		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x0600577F RID: 22399 RVA: 0x001B2AB0 File Offset: 0x001B0EB0
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06005780 RID: 22400 RVA: 0x001B2AD4 File Offset: 0x001B0ED4
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x001B2AF6 File Offset: 0x001B0EF6
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x001B2B0C File Offset: 0x001B0F0C
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x001B2B24 File Offset: 0x001B0F24
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x001B2B44 File Offset: 0x001B0F44
		protected void LabelLeft(string label, string tipText, int indentLevel)
		{
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight)
			{
				xMin = this.XAtIndentLevel(indentLevel) + 18f
			};
			Widgets.DrawHighlightIfMouseover(rect);
			if (!tipText.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					GUI.DrawTexture(rect, TexUI.HighlightTex);
				}
				TooltipHandler.TipRegion(rect, tipText);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			rect.width = this.LabelWidth;
			rect.yMax += 5f;
			rect.yMin -= 5f;
			Widgets.Label(rect, label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x001B2C00 File Offset: 0x001B1000
		protected bool OpenCloseWidget(TreeNode node, int indentLevel, int openMask)
		{
			bool result;
			if (!node.Openable)
			{
				result = false;
			}
			else
			{
				float x = this.XAtIndentLevel(indentLevel);
				float y = this.curY + this.lineHeight / 2f - 9f;
				Rect butRect = new Rect(x, y, 18f, 18f);
				Texture2D tex = (!node.IsOpen(openMask)) ? TexButton.Reveal : TexButton.Collapse;
				if (Widgets.ButtonImage(butRect, tex))
				{
					node.SetOpen(openMask, !node.IsOpen(openMask));
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x001B2CA0 File Offset: 0x001B10A0
		public void InfoText(string text, int indentLevel)
		{
			Text.WordWrap = true;
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, 50f);
			rect.xMin = this.LabelWidth;
			rect.height = Text.CalcHeight(text, rect.width);
			Widgets.Label(rect, text);
			this.curY += rect.height;
			Text.WordWrap = false;
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x001B2D14 File Offset: 0x001B1114
		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, num);
			bool result = Widgets.ButtonText(rect, label, true, false, true);
			this.curY += num;
			Text.WordWrap = false;
			return result;
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x001B2D78 File Offset: 0x001B1178
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}

		// Token: 0x040039E2 RID: 14818
		public float nestIndentWidth = 11f;

		// Token: 0x040039E3 RID: 14819
		protected const float OpenCloseWidgetSize = 18f;
	}
}
