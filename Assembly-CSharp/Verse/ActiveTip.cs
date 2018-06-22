using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E8E RID: 3726
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x06005801 RID: 22529 RVA: 0x002D2628 File Offset: 0x002D0A28
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x002D2647 File Offset: 0x002D0A47
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06005803 RID: 22531 RVA: 0x002D2684 File Offset: 0x002D0A84
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

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06005804 RID: 22532 RVA: 0x002D270C File Offset: 0x002D0B0C
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

		// Token: 0x06005805 RID: 22533 RVA: 0x002D2790 File Offset: 0x002D0B90
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

		// Token: 0x04003A2F RID: 14895
		public TipSignal signal;

		// Token: 0x04003A30 RID: 14896
		public double firstTriggerTime = 0.0;

		// Token: 0x04003A31 RID: 14897
		public int lastTriggerFrame;

		// Token: 0x04003A32 RID: 14898
		private const int TipMargin = 4;

		// Token: 0x04003A33 RID: 14899
		private const float MaxWidth = 260f;

		// Token: 0x04003A34 RID: 14900
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}
