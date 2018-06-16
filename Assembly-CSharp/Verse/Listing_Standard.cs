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
		// Token: 0x0600575F RID: 22367 RVA: 0x002CD64B File Offset: 0x002CBA4B
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x002CD65B File Offset: 0x002CBA5B
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x002CD66B File Offset: 0x002CBA6B
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x002CD680 File Offset: 0x002CBA80
		public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
		{
			Widgets.BeginScrollView(rect, ref scrollPosition, viewRect, true);
			rect.height = 100000f;
			rect.width -= 20f;
			this.Begin(rect.AtZero());
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x002CD6BC File Offset: 0x002CBABC
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

		// Token: 0x06005764 RID: 22372 RVA: 0x002CD738 File Offset: 0x002CBB38
		public void EndScrollView(ref Rect viewRect)
		{
			viewRect = new Rect(0f, 0f, this.listingRect.width, this.curY);
			Widgets.EndScrollView();
			this.End();
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x002CD768 File Offset: 0x002CBB68
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

		// Token: 0x06005766 RID: 22374 RVA: 0x002CD808 File Offset: 0x002CBC08
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

		// Token: 0x06005767 RID: 22375 RVA: 0x002CD898 File Offset: 0x002CBC98
		public bool RadioButton(string label, bool active, float tabIn = 0f)
		{
			float lineHeight = Text.LineHeight;
			base.NewColumnIfNeeded(lineHeight);
			bool result = Widgets.RadioButtonLabeled(new Rect(this.curX + tabIn, this.curY, base.ColumnWidth - tabIn, lineHeight), label, active);
			this.curY += lineHeight + this.verticalSpacing;
			return result;
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x002CD8F4 File Offset: 0x002CBCF4
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

		// Token: 0x06005769 RID: 22377 RVA: 0x002CD954 File Offset: 0x002CBD54
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight);
			bool result = Widgets.CheckboxLabeledSelectable(rect, label, ref selected, ref checkOn);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x002CD990 File Offset: 0x002CBD90
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

		// Token: 0x0600576B RID: 22379 RVA: 0x002CD9D8 File Offset: 0x002CBDD8
		public bool ButtonTextLabeled(string label, string buttonLabel)
		{
			Rect rect = base.GetRect(30f);
			Widgets.Label(rect.LeftHalf(), label);
			bool result = Widgets.ButtonText(rect.RightHalf(), buttonLabel, true, false, true);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x002CDA24 File Offset: 0x002CBE24
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			bool result = Widgets.ButtonImage(new Rect(this.curX, this.curY, width, height), tex);
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x002CDA69 File Offset: 0x002CBE69
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x002CDAA4 File Offset: 0x002CBEA4
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

		// Token: 0x0600576F RID: 22383 RVA: 0x002CDAF4 File Offset: 0x002CBEF4
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount);
			string result = Widgets.TextEntryLabeled(rect, label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x002CDB30 File Offset: 0x002CBF30
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumeric<T>(rect, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x002CDB64 File Offset: 0x002CBF64
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight);
			Widgets.TextFieldNumericLabeled<T>(rect, label, ref val, ref buffer, min, max);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x002CDB98 File Offset: 0x002CBF98
		public void IntRange(ref IntRange range, int min, int max)
		{
			Rect rect = base.GetRect(28f);
			Widgets.IntRange(rect, (int)base.CurHeight, ref range, min, max, null, 0);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x002CDBD0 File Offset: 0x002CBFD0
		public float Slider(float val, float min, float max)
		{
			Rect rect = base.GetRect(22f);
			float result = Widgets.HorizontalSlider(rect, val, min, max, false, null, null, null, -1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x002CDC14 File Offset: 0x002CC014
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

		// Token: 0x06005775 RID: 22389 RVA: 0x002CDCE8 File Offset: 0x002CC0E8
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

		// Token: 0x06005776 RID: 22390 RVA: 0x002CDD30 File Offset: 0x002CC130
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1)
		{
			Rect rect = base.GetRect(24f);
			Widgets.IntEntry(rect, ref val, ref editBuffer, multiplier);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x002CDD60 File Offset: 0x002CC160
		public Listing_Standard BeginSection(float height)
		{
			Rect rect = base.GetRect(height + 8f);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect.ContractedBy(4f));
			return listing_Standard;
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x002CDDA1 File Offset: 0x002CC1A1
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x002CDDAC File Offset: 0x002CC1AC
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

		// Token: 0x0600577A RID: 22394 RVA: 0x002CDE44 File Offset: 0x002CC244
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

		// Token: 0x0600577B RID: 22395 RVA: 0x002CDF0C File Offset: 0x002CC30C
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

		// Token: 0x0600577C RID: 22396 RVA: 0x002CDFF0 File Offset: 0x002CC3F0
		public void LabelCheckboxDebug(string label, ref bool checkOn)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Widgets.CheckboxLabeled(new Rect(this.curX, this.curY, base.ColumnWidth, 22f), label, ref checkOn, false, null, null, false);
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x002CE048 File Offset: 0x002CC448
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

		// Token: 0x040039DE RID: 14814
		private GameFont font;

		// Token: 0x040039DF RID: 14815
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x040039E0 RID: 14816
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x040039E1 RID: 14817
		private const float DefSelectionLineHeight = 21f;
	}
}
