using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class Listing_Standard : Listing
	{
		private const float DefSelectionLineHeight = 21f;

		private GameFont font;

		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		public override void End()
		{
			base.End();
			if (this.labelScrollbarPositions != null)
			{
				for (int num = this.labelScrollbarPositions.Count - 1; num >= 0; num--)
				{
					if (!this.labelScrollbarPositionsSetThisFrame.Contains(this.labelScrollbarPositions[num].First))
					{
						this.labelScrollbarPositions.RemoveAt(num);
					}
				}
				this.labelScrollbarPositionsSetThisFrame.Clear();
			}
		}

		public void Label(string label, float maxHeight = -1f)
		{
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool flag = false;
			if (maxHeight >= 0.0 && num > maxHeight)
			{
				num = maxHeight;
				flag = true;
			}
			Rect rect = base.GetRect(num);
			if (flag)
			{
				Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(base.curX, base.curY);
				Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition);
				this.SetLabelScrollbarPosition(base.curX, base.curY, labelScrollbarPosition);
			}
			else
			{
				Widgets.Label(rect, label);
			}
			base.Gap(base.verticalSpacing);
		}

		public void LabelDouble(string leftLabel, string rightLabel)
		{
			float num = (float)(base.ColumnWidth / 2.0);
			float width = base.ColumnWidth - num;
			float a = Text.CalcHeight(leftLabel, num);
			float b = Text.CalcHeight(rightLabel, width);
			float height = Mathf.Max(a, b);
			Rect rect = base.GetRect(height);
			Widgets.Label(rect.LeftHalf(), leftLabel);
			Widgets.Label(rect.RightHalf(), rightLabel);
			base.Gap(base.verticalSpacing);
		}

		public bool RadioButton(string label, bool active, float tabIn = 0)
		{
			float lineHeight = Text.LineHeight;
			base.NewColumnIfNeeded(lineHeight);
			bool result = Widgets.RadioButtonLabeled(new Rect(base.curX + tabIn, base.curY, base.ColumnWidth - tabIn, lineHeight), label, active);
			base.curY += lineHeight + base.verticalSpacing;
			return result;
		}

		public void CheckboxLabeled(string label, ref bool checkOn, string tooltip = null)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Widgets.CheckboxLabeled(rect, label, ref checkOn, false);
			base.Gap(base.verticalSpacing);
		}

		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			bool result = Widgets.CheckboxLabeledSelectable(rect, label, ref selected, ref checkOn);
			base.Gap(base.verticalSpacing);
			return result;
		}

		public bool ButtonText(string label, string highlightTag = null)
		{
			Rect rect = base.GetRect(30f);
			bool result = Widgets.ButtonText(rect, label, true, false, true);
			if (highlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, highlightTag);
			}
			base.Gap(base.verticalSpacing);
			return result;
		}

		public bool ButtonTextLabeled(string label, string buttonLabel)
		{
			Rect rect = base.GetRect(30f);
			Widgets.Label(rect.LeftHalf(), label);
			bool result = Widgets.ButtonText(rect.RightHalf(), buttonLabel, true, false, true);
			base.Gap(base.verticalSpacing);
			return result;
		}

		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			bool result = Widgets.ButtonImage(new Rect(base.curX, base.curY, width, height), tex);
			base.Gap(height + base.verticalSpacing);
			return result;
		}

		public string TextEntry(string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result = (lineCount != 1) ? Widgets.TextArea(rect, text, false) : Widgets.TextField(rect, text);
			base.Gap(base.verticalSpacing);
			return result;
		}

		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result = Widgets.TextEntryLabeled(rect, label, text);
			base.Gap(base.verticalSpacing);
			return result;
		}

		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0, float max = 1000000000) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumeric<T>(rect, ref val, ref buffer, min, max);
			base.Gap(base.verticalSpacing);
		}

		public void TextFieldNumericLabeled<T>(string label, ref int val, ref string buffer, float min = 0, float max = 1000000000)
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumericLabeled<int>(rect, label, ref val, ref buffer, min, max);
			base.Gap(base.verticalSpacing);
		}

		public void IntRange(ref IntRange range, int min, int max)
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.IntRange(rect, (int)base.CurHeight, ref range, min, max, (string)null, 0);
			base.Gap(base.verticalSpacing);
		}

		public float Slider(float val, float min, float max)
		{
			Rect rect = base.GetRect(30f);
			float result = Widgets.HorizontalSlider(rect, val, min, max, false, (string)null, (string)null, (string)null, -1f);
			base.Gap(base.verticalSpacing);
			return result;
		}

		public void IntAdjuster(ref int val, int countChange, int min = 0)
		{
			Rect rect = base.GetRect(24f);
			rect.width = 42f;
			if (Widgets.ButtonText(rect, "-" + countChange, true, false, true))
			{
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
				val -= countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			rect.x += (float)(rect.width + 2.0);
			if (Widgets.ButtonText(rect, "+" + countChange, true, false, true))
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			base.Gap(base.verticalSpacing);
		}

		public void IntSetter(ref int val, int target, string label, float width = 42)
		{
			Rect rect = base.GetRect(24f);
			if (Widgets.ButtonText(rect, label, true, false, true))
			{
				SoundDefOf.TickLow.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(base.verticalSpacing);
		}

		private Vector2 GetLabelScrollbarPosition(float x, float y)
		{
			if (this.labelScrollbarPositions == null)
			{
				return Vector2.zero;
			}
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					return this.labelScrollbarPositions[i].Second;
				}
			}
			return Vector2.zero;
		}

		private void SetLabelScrollbarPosition(float x, float y, Vector2 scrollbarPosition)
		{
			if (this.labelScrollbarPositions == null)
			{
				this.labelScrollbarPositions = new List<Pair<Vector2, Vector2>>();
				this.labelScrollbarPositionsSetThisFrame = new List<Vector2>();
			}
			this.labelScrollbarPositionsSetThisFrame.Add(new Vector2(x, y));
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					this.labelScrollbarPositions[i] = new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition);
					return;
				}
			}
			this.labelScrollbarPositions.Add(new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition));
		}

		public bool SelectableDef(string name, bool selected, Action deleteCallback)
		{
			Text.Font = GameFont.Tiny;
			float width = (float)(base.listingRect.width - 21.0);
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(base.curX, base.curY, width, 21f);
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 1);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, name);
			Text.WordWrap = true;
			if ((object)deleteCallback != null)
			{
				Rect butRect = new Rect(rect.xMax, rect.y, 21f, 21f);
				if (Widgets.ButtonImage(butRect, TexButton.DeleteX))
				{
					deleteCallback();
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			base.curY += 21f;
			return Widgets.ButtonInvisible(rect, false);
		}

		public void LabelCheckboxDebug(string label, ref bool checkOn)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Widgets.CheckboxLabeled(new Rect(base.curX, base.curY, base.ColumnWidth, 22f), label, ref checkOn, false);
			base.Gap((float)(22.0 + base.verticalSpacing));
		}

		public bool ButtonDebug(string label)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			bool wordWrap = Text.WordWrap;
			Text.WordWrap = false;
			bool result = Widgets.ButtonText(new Rect(base.curX, base.curY, base.ColumnWidth, 22f), label, true, false, true);
			Text.WordWrap = wordWrap;
			base.Gap((float)(22.0 + base.verticalSpacing));
			return result;
		}
	}
}
