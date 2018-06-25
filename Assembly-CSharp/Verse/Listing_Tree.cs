using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E83 RID: 3715
	public class Listing_Tree : Listing_Lines
	{
		// Token: 0x040039F8 RID: 14840
		public float nestIndentWidth = 11f;

		// Token: 0x040039F9 RID: 14841
		protected const float OpenCloseWidgetSize = 18f;

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x001B3108 File Offset: 0x001B1508
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x060057A2 RID: 22434 RVA: 0x001B312C File Offset: 0x001B152C
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x001B314E File Offset: 0x001B154E
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x001B3164 File Offset: 0x001B1564
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x001B317C File Offset: 0x001B157C
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x001B319C File Offset: 0x001B159C
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

		// Token: 0x060057A7 RID: 22439 RVA: 0x001B3258 File Offset: 0x001B1658
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

		// Token: 0x060057A8 RID: 22440 RVA: 0x001B32F8 File Offset: 0x001B16F8
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

		// Token: 0x060057A9 RID: 22441 RVA: 0x001B336C File Offset: 0x001B176C
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

		// Token: 0x060057AA RID: 22442 RVA: 0x001B33D0 File Offset: 0x001B17D0
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}
	}
}
