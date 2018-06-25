using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E81 RID: 3713
	public class Listing_Standard : Listing
	{
		// Token: 0x040039EC RID: 14828
		private GameFont font;

		// Token: 0x040039ED RID: 14829
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x040039EE RID: 14830
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x040039EF RID: 14831
		private const float DefSelectionLineHeight = 21f;

		// Token: 0x06005781 RID: 22401 RVA: 0x002CF387 File Offset: 0x002CD787
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x002CF397 File Offset: 0x002CD797
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x002CF3A7 File Offset: 0x002CD7A7
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x002CF3BC File Offset: 0x002CD7BC
		public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
		{
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			rect.height = 100000f;
			rect.width -= 20f;
			this.Begin(rect.AtZero());
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x002CF3F8 File Offset: 0x002CD7F8
		public override void End()
		{
			base.End();
			if (this.labelScrollbarPositions != null)
			{
				for (int i = this.labelScrollbarPositions.Count - 1; i >= 0; i--)
				{
					if (!this.labelScrollbarPositionsSetThisFrame.Contains(this.labelScrollbarPositions[i].First))
					{
						this.labelScrollbarPositions.RemoveAt(i);
					}
				}
				this.labelScrollbarPositionsSetThisFrame.Clear();
			}
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x002CF474 File Offset: 0x002CD874
		public void EndScrollView(ref Rect viewRect)
		{
			viewRect = new Rect(0f, 0f, this.listingRect.width, this.curY);
			Widgets.EndScrollView();
			this.End();
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x002CF4A4 File Offset: 0x002CD8A4
		public void Label(string label, float maxHeight = -1f, string tooltip = null)
		{
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool flag = false;
			if (maxHeight >= 0f && num > maxHeight)
			{
				num = maxHeight;
				flag = true;
			}
			Rect rect = base.GetRect(num);
			if (flag)
			{
				Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(this.curX, this.curY);
				Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition, false);
				this.SetLabelScrollbarPosition(this.curX, this.curY, labelScrollbarPosition);
			}
			else
			{
				Widgets.Label(rect, label);
			}
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x002CF544 File Offset: 0x002CD944
		public void LabelDouble(string leftLabel, string rightLabel, string tip = null)
		{
			float num = base.ColumnWidth / 2f;
			float width = base.ColumnWidth - num;
			float a = Text.CalcHeight(leftLabel, num);
			float b = Text.CalcHeight(rightLabel, width);
			float height = Mathf.Max(a, b);
			Rect rect = base.GetRect(height);
			if (!tip.NullOrEmpty())
			{
				Widgets.DrawHighlightIfMouseover(rect);
				TooltipHandler.TipRegion(rect, tip);
			}
			Widgets.Label(rect.LeftHalf(), leftLabel);
			Widgets.Label(rect.RightHalf(), rightLabel);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x002CF5D4 File Offset: 0x002CD9D4
		public bool RadioButton(string label, bool active, float tabIn = 0f)
		{
			float lineHeight = Text.LineHeight;
			base.NewColumnIfNeeded(lineHeight);
			bool result = Widgets.RadioButtonLabeled(new Rect(this.curX + tabIn, this.curY, base.ColumnWidth - tabIn, lineHeight), label, active);
			this.curY += lineHeight + this.verticalSpacing;
			return result;
		}

		// Token: 0x0600578A RID: 22410 RVA: 0x002CF630 File Offset: 0x002CDA30
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
			Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600578B RID: 22411 RVA: 0x002CF690 File Offset: 0x002CDA90
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			bool result = Widgets.CheckboxLabeledSelectable(rect, label, ref selected, ref checkOn);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x002CF6CC File Offset: 0x002CDACC
		public bool ButtonText(string label, string highlightTag = null)
		{
			Rect rect = base.GetRect(30f);
			bool result = Widgets.ButtonText(rect, label, true, false, true);
			if (highlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, highlightTag);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600578D RID: 22413 RVA: 0x002CF714 File Offset: 0x002CDB14
		public bool ButtonTextLabeled(string label, string buttonLabel)
		{
			Rect rect = base.GetRect(30f);
			Widgets.Label(rect.LeftHalf(), label);
			bool result = Widgets.ButtonText(rect.RightHalf(), buttonLabel, true, false, true);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600578E RID: 22414 RVA: 0x002CF760 File Offset: 0x002CDB60
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			bool result = Widgets.ButtonImage(new Rect(this.curX, this.curY, width, height), tex);
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x002CF7A5 File Offset: 0x002CDBA5
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x002CF7E0 File Offset: 0x002CDBE0
		public string TextEntry(string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result;
			if (lineCount == 1)
			{
				result = Widgets.TextField(rect, text);
			}
			else
			{
				result = Widgets.TextArea(rect, text, false);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06005791 RID: 22417 RVA: 0x002CF830 File Offset: 0x002CDC30
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result = Widgets.TextEntryLabeled(rect, label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x002CF86C File Offset: 0x002CDC6C
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumeric<T>(rect, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x002CF8A0 File Offset: 0x002CDCA0
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumericLabeled<T>(rect, label, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x002CF8D4 File Offset: 0x002CDCD4
		public void IntRange(ref IntRange range, int min, int max)
		{
			Rect rect = base.GetRect(28f);
			Widgets.IntRange(rect, (int)base.CurHeight, ref range, min, max, null, 0);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x002CF90C File Offset: 0x002CDD0C
		public float Slider(float val, float min, float max)
		{
			Rect rect = base.GetRect(22f);
			float result = Widgets.HorizontalSlider(rect, val, min, max, false, null, null, null, -1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x002CF950 File Offset: 0x002CDD50
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
			rect.x += rect.width + 2f;
			if (Widgets.ButtonText(rect, "+" + countChange, true, false, true))
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x002CFA24 File Offset: 0x002CDE24
		public void IntSetter(ref int val, int target, string label, float width = 42f)
		{
			Rect rect = base.GetRect(24f);
			if (Widgets.ButtonText(rect, label, true, false, true))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005798 RID: 22424 RVA: 0x002CFA6C File Offset: 0x002CDE6C
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1)
		{
			Rect rect = base.GetRect(24f);
			Widgets.IntEntry(rect, ref val, ref editBuffer, multiplier);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x002CFA9C File Offset: 0x002CDE9C
		public Listing_Standard BeginSection(float height)
		{
			Rect rect = base.GetRect(height + 8f);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect.ContractedBy(4f));
			return listing_Standard;
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x002CFADD File Offset: 0x002CDEDD
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x0600579B RID: 22427 RVA: 0x002CFAE8 File Offset: 0x002CDEE8
		private Vector2 GetLabelScrollbarPosition(float x, float y)
		{
			Vector2 zero;
			if (this.labelScrollbarPositions == null)
			{
				zero = Vector2.zero;
			}
			else
			{
				for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
				{
					Vector2 first = this.labelScrollbarPositions[i].First;
					if (first.x == x && first.y == y)
					{
						return this.labelScrollbarPositions[i].Second;
					}
				}
				zero = Vector2.zero;
			}
			return zero;
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x002CFB80 File Offset: 0x002CDF80
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

		// Token: 0x0600579D RID: 22429 RVA: 0x002CFC48 File Offset: 0x002CE048
		public bool SelectableDef(string name, bool selected, Action deleteCallback)
		{
			Text.Font = GameFont.Tiny;
			float width = this.listingRect.width - 21f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(this.curX, this.curY, width, 21f);
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
			if (deleteCallback != null)
			{
				Rect butRect = new Rect(rect.xMax, rect.y, 21f, 21f);
				if (Widgets.ButtonImage(butRect, TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor))
				{
					deleteCallback();
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.curY += 21f;
			return Widgets.ButtonInvisible(rect, false);
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x002CFD2C File Offset: 0x002CE12C
		public void LabelCheckboxDebug(string label, ref bool checkOn)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Widgets.CheckboxLabeled(new Rect(this.curX, this.curY, base.ColumnWidth, 22f), label, ref checkOn, false, null, null, false);
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x002CFD84 File Offset: 0x002CE184
		public bool ButtonDebug(string label)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			bool wordWrap = Text.WordWrap;
			Text.WordWrap = false;
			bool result = Widgets.ButtonText(new Rect(this.curX, this.curY, base.ColumnWidth, 22f), label, true, false, true);
			Text.WordWrap = wordWrap;
			base.Gap(22f + this.verticalSpacing);
			return result;
		}
	}
}
