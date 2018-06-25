using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EBA RID: 3770
	public class Dialog_Slider : Window
	{
		// Token: 0x04003B70 RID: 15216
		public Func<int, string> textGetter;

		// Token: 0x04003B71 RID: 15217
		public int from;

		// Token: 0x04003B72 RID: 15218
		public int to;

		// Token: 0x04003B73 RID: 15219
		private Action<int> confirmAction;

		// Token: 0x04003B74 RID: 15220
		private int curValue;

		// Token: 0x04003B75 RID: 15221
		private const float BotAreaHeight = 30f;

		// Token: 0x04003B76 RID: 15222
		private const float TopPadding = 15f;

		// Token: 0x06005937 RID: 22839 RVA: 0x002DC49C File Offset: 0x002DA89C
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

		// Token: 0x06005938 RID: 22840 RVA: 0x002DC4FC File Offset: 0x002DA8FC
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue)
		{
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06005939 RID: 22841 RVA: 0x002DC530 File Offset: 0x002DA930
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f);
			}
		}

		// Token: 0x0600593A RID: 22842 RVA: 0x002DC554 File Offset: 0x002DA954
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
