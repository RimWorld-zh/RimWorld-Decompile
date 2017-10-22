using UnityEngine;

namespace Verse
{
	public class Listing_Tree : Listing_Lines
	{
		public float nestIndentWidth = 11f;

		protected const float OpenCloseWidgetSize = 18f;

		protected virtual float LabelWidth
		{
			get
			{
				return (float)(base.ColumnWidth - 26.0);
			}
		}

		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		protected void LabelLeft(string label, string tipText, int indentLevel)
		{
			Rect rect = new Rect(0f, base.curY, base.ColumnWidth, base.lineHeight)
			{
				xMin = (float)(this.XAtIndentLevel(indentLevel) + 18.0)
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
				float y = (float)(base.curY + base.lineHeight / 2.0 - 9.0);
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

		public void InfoText(string text, int indentLevel)
		{
			Text.WordWrap = true;
			Rect rect = new Rect(0f, base.curY, base.ColumnWidth, 50f)
			{
				xMin = this.LabelWidth
			};
			rect.height = Text.CalcHeight(text, rect.width);
			Widgets.Label(rect, text);
			base.curY += rect.height;
			Text.WordWrap = false;
		}

		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			Rect rect = new Rect(0f, base.curY, base.ColumnWidth, num);
			bool result = Widgets.ButtonText(rect, label, true, false, true);
			base.curY += num;
			Text.WordWrap = false;
			return result;
		}

		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, base.curY, UIDirection.RightThenUp, 99999f, 4f);
			base.curY += 24f;
			return result;
		}
	}
}
