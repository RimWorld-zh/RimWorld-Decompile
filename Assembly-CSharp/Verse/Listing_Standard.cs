using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E82 RID: 3714
	public class Listing_Standard : Listing
	{
		// Token: 0x040039F4 RID: 14836
		private GameFont font;

		// Token: 0x040039F5 RID: 14837
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x040039F6 RID: 14838
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x040039F7 RID: 14839
		private const float DefSelectionLineHeight = 21f;

		// Token: 0x06005781 RID: 22401 RVA: 0x002CF573 File Offset: 0x002CD973
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x002CF583 File Offset: 0x002CD983
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x002CF593 File Offset: 0x002CD993
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x002CF5A8 File Offset: 0x002CD9A8
		public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
		{
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			rect.height = 100000f;
			rect.width -= 20f;
			this.Begin(rect.AtZero());
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x002CF5E4 File Offset: 0x002CD9E4
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

		// Token: 0x06005786 RID: 22406 RVA: 0x002CF660 File Offset: 0x002CDA60
		public void EndScrollView(ref Rect viewRect)
		{
			viewRect = new Rect(0f, 0f, this.listingRect.width, this.curY);
			Widgets.EndScrollView();
			this.End();
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x002CF690 File Offset: 0x002CDA90
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

		// Token: 0x06005788 RID: 22408 RVA: 0x002CF730 File Offset: 0x002CDB30
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

		// Token: 0x06005789 RID: 22409 RVA: 0x002CF7C0 File Offset: 0x002CDBC0
		public bool RadioButton(string label, bool active, float tabIn = 0f)
		{
			float lineHeight = Text.LineHeight;
			base.NewColumnIfNeeded(lineHeight);
			bool result = Widgets.RadioButtonLabeled(new Rect(this.curX + tabIn, this.curY, base.ColumnWidth - tabIn, lineHeight), label, active);
			this.curY += lineHeight + this.verticalSpacing;
			return result;
		}

		// Token: 0x0600578A RID: 22410 RVA: 0x002CF81C File Offset: 0x002CDC1C
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

		// Token: 0x0600578B RID: 22411 RVA: 0x002CF87C File Offset: 0x002CDC7C
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			bool result = Widgets.CheckboxLabeledSelectable(rect, label, ref selected, ref checkOn);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x002CF8B8 File Offset: 0x002CDCB8
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

		// Token: 0x0600578D RID: 22413 RVA: 0x002CF900 File Offset: 0x002CDD00
		public bool ButtonTextLabeled(string label, string buttonLabel)
		{
			Rect rect = base.GetRect(30f);
			Widgets.Label(rect.LeftHalf(), label);
			bool result = Widgets.ButtonText(rect.RightHalf(), buttonLabel, true, false, true);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600578E RID: 22414 RVA: 0x002CF94C File Offset: 0x002CDD4C
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			bool result = Widgets.ButtonImage(new Rect(this.curX, this.curY, width, height), tex);
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x002CF991 File Offset: 0x002CDD91
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x002CF9CC File Offset: 0x002CDDCC
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

		// Token: 0x06005791 RID: 22417 RVA: 0x002CFA1C File Offset: 0x002CDE1C
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result = Widgets.TextEntryLabeled(rect, label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x002CFA58 File Offset: 0x002CDE58
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumeric<T>(rect, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x002CFA8C File Offset: 0x002CDE8C
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumericLabeled<T>(rect, label, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x002CFAC0 File Offset: 0x002CDEC0
		public void IntRange(ref IntRange range, int min, int max)
		{
			Rect rect = base.GetRect(28f);
			Widgets.IntRange(rect, (int)base.CurHeight, ref range, min, max, null, 0);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x002CFAF8 File Offset: 0x002CDEF8
		public float Slider(float val, float min, float max)
		{
			Rect rect = base.GetRect(22f);
			float result = Widgets.HorizontalSlider(rect, val, min, max, false, null, null, null, -1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x002CFB3C File Offset: 0x002CDF3C
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

		// Token: 0x06005797 RID: 22423 RVA: 0x002CFC10 File Offset: 0x002CE010
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

		// Token: 0x06005798 RID: 22424 RVA: 0x002CFC58 File Offset: 0x002CE058
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1)
		{
			Rect rect = base.GetRect(24f);
			Widgets.IntEntry(rect, ref val, ref editBuffer, multiplier);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x002CFC88 File Offset: 0x002CE088
		public Listing_Standard BeginSection(float height)
		{
			Rect rect = base.GetRect(height + 8f);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect.ContractedBy(4f));
			return listing_Standard;
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x002CFCC9 File Offset: 0x002CE0C9
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x0600579B RID: 22427 RVA: 0x002CFCD4 File Offset: 0x002CE0D4
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

		// Token: 0x0600579C RID: 22428 RVA: 0x002CFD6C File Offset: 0x002CE16C
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

		// Token: 0x0600579D RID: 22429 RVA: 0x002CFE34 File Offset: 0x002CE234
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

		// Token: 0x0600579E RID: 22430 RVA: 0x002CFF18 File Offset: 0x002CE318
		public void LabelCheckboxDebug(string label, ref bool checkOn)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Widgets.CheckboxLabeled(new Rect(this.curX, this.curY, base.ColumnWidth, 22f), label, ref checkOn, false, null, null, false);
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x002CFF70 File Offset: 0x002CE370
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
