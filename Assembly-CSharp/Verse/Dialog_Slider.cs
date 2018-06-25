using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBB RID: 3771
	public class Dialog_Slider : Window
	{
		// Token: 0x04003B78 RID: 15224
		public Func<int, string> textGetter;

		// Token: 0x04003B79 RID: 15225
		public int from;

		// Token: 0x04003B7A RID: 15226
		public int to;

		// Token: 0x04003B7B RID: 15227
		private Action<int> confirmAction;

		// Token: 0x04003B7C RID: 15228
		private int curValue;

		// Token: 0x04003B7D RID: 15229
		private const float BotAreaHeight = 30f;

		// Token: 0x04003B7E RID: 15230
		private const float TopPadding = 15f;

		// Token: 0x06005937 RID: 22839 RVA: 0x002DC688 File Offset: 0x002DAA88
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

		// Token: 0x06005938 RID: 22840 RVA: 0x002DC6E8 File Offset: 0x002DAAE8
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue)
		{
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06005939 RID: 22841 RVA: 0x002DC71C File Offset: 0x002DAB1C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f);
			}
		}

		// Token: 0x0600593A RID: 22842 RVA: 0x002DC740 File Offset: 0x002DAB40
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
	}
}
