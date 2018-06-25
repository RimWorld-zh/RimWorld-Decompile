using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E91 RID: 3729
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x04003A37 RID: 14903
		public TipSignal signal;

		// Token: 0x04003A38 RID: 14904
		public double firstTriggerTime = 0.0;

		// Token: 0x04003A39 RID: 14905
		public int lastTriggerFrame;

		// Token: 0x04003A3A RID: 14906
		private const int TipMargin = 4;

		// Token: 0x04003A3B RID: 14907
		private const float MaxWidth = 260f;

		// Token: 0x04003A3C RID: 14908
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);

		// Token: 0x06005805 RID: 22533 RVA: 0x002D2940 File Offset: 0x002D0D40
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x002D295F File Offset: 0x002D0D5F
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06005807 RID: 22535 RVA: 0x002D299C File Offset: 0x002D0D9C
		private string FinalText
		{
			get
			{
				string text;
				if (this.signal.textGetter != null)
				{
					try
					{
						text = this.signal.textGetter();
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString(), false);
						text = "Error getting tip text.";
					}
				}
				else
				{
					text = this.signal.text;
				}
				return text.TrimEnd(new char[0]);
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06005808 RID: 22536 RVA: 0x002D2A24 File Offset: 0x002D0E24
		public Rect TipRect
		{
			get
			{
				string finalText = this.FinalText;
				Vector2 vector = Text.CalcSize(finalText);
				if (vector.x > 260f)
				{
					vector.x = 260f;
					vector.y = Text.CalcHeight(finalText, vector.x);
				}
				Rect rect = new Rect(0f, 0f, vector.x, vector.y);
				rect = rect.ContractedBy(-4f);
				return rect;
			}
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x002D2AA8 File Offset: 0x002D0EA8
		public float DrawTooltip(Vector2 pos)
		{
			Text.Font = GameFont.Small;
			string finalText = this.FinalText;
			Rect bgRect = this.TipRect;
			bgRect.position = pos;
			Find.WindowStack.ImmediateWindow(153 * this.signal.uniqueId + 62346, bgRect, WindowLayer.Super, delegate
			{
				Rect rect = bgRect.AtZero();
				Widgets.DrawAtlas(rect, ActiveTip.TooltipBGAtlas);
				Text.Font = GameFont.Small;
				Widgets.Label(rect.ContractedBy(4f), finalText);
			}, false, false, 1f);
			return bgRect.height;
		}
	}
}
