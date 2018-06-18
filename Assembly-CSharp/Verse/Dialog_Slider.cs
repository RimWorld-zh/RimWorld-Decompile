using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EB9 RID: 3769
	public class Dialog_Slider : Window
	{
		// Token: 0x06005912 RID: 22802 RVA: 0x002DA724 File Offset: 0x002D8B24
		public Dialog_Slider(Func<int, string> textGetter, int from, int to, Action<int> confirmAction, int startingValue = -2147483648)
		{
			this.textGetter = textGetter;
			this.from = from;
			this.to = to;
			this.confirmAction = confirmAction;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			if (startingValue == -2147483648)
			{
				this.curValue = from;
			}
			else
			{
				this.curValue = startingValue;
			}
		}

		// Token: 0x06005913 RID: 22803 RVA: 0x002DA784 File Offset: 0x002D8B84
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue)
		{
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005914 RID: 22804 RVA: 0x002DA7B8 File Offset: 0x002D8BB8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f);
			}
		}

		// Token: 0x06005915 RID: 22805 RVA: 0x002DA7DC File Offset: 0x002D8BDC
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect.x, inRect.y + 15f, inRect.width, 30f);
			this.curValue = (int)Widgets.HorizontalSlider(rect, (float)this.curValue, (float)this.from, (float)this.to, true, this.textGetter(this.curValue), null, null, 1f);
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(inRect.x, inRect.yMax - 30f, inRect.width / 2f, 30f);
			if (Widgets.ButtonText(rect2, "CancelButton".Translate(), true, false, true))
			{
				this.Close(true);
			}
			Rect rect3 = new Rect(inRect.x + inRect.width / 2f, inRect.yMax - 30f, inRect.width / 2f, 30f);
			if (Widgets.ButtonText(rect3, "OK".Translate(), true, false, true))
			{
				this.Close(true);
				this.confirmAction(this.curValue);
			}
		}

		// Token: 0x04003B60 RID: 15200
		public Func<int, string> textGetter;

		// Token: 0x04003B61 RID: 15201
		public int from;

		// Token: 0x04003B62 RID: 15202
		public int to;

		// Token: 0x04003B63 RID: 15203
		private Action<int> confirmAction;

		// Token: 0x04003B64 RID: 15204
		private int curValue;

		// Token: 0x04003B65 RID: 15205
		private const float BotAreaHeight = 30f;

		// Token: 0x04003B66 RID: 15206
		private const float TopPadding = 15f;
	}
}
