using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E90 RID: 3728
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x060057E3 RID: 22499 RVA: 0x002D0A18 File Offset: 0x002CEE18
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x002D0A37 File Offset: 0x002CEE37
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x060057E5 RID: 22501 RVA: 0x002D0A74 File Offset: 0x002CEE74
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

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x060057E6 RID: 22502 RVA: 0x002D0AFC File Offset: 0x002CEEFC
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

		// Token: 0x060057E7 RID: 22503 RVA: 0x002D0B80 File Offset: 0x002CEF80
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

		// Token: 0x04003A21 RID: 14881
		public TipSignal signal;

		// Token: 0x04003A22 RID: 14882
		public double firstTriggerTime = 0.0;

		// Token: 0x04003A23 RID: 14883
		public int lastTriggerFrame;

		// Token: 0x04003A24 RID: 14884
		private const int TipMargin = 4;

		// Token: 0x04003A25 RID: 14885
		private const float MaxWidth = 260f;

		// Token: 0x04003A26 RID: 14886
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}
