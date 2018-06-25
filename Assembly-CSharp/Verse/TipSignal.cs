using System;

namespace Verse
{
	// Token: 0x02000E93 RID: 3731
	public struct TipSignal
	{
		// Token: 0x04003A40 RID: 14912
		public string text;

		// Token: 0x04003A41 RID: 14913
		public Func<string> textGetter;

		// Token: 0x04003A42 RID: 14914
		public int uniqueId;

		// Token: 0x04003A43 RID: 14915
		public TooltipPriority priority;

		// Token: 0x0600580B RID: 22539 RVA: 0x002D2B91 File Offset: 0x002D0F91
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x002D2BB0 File Offset: 0x002D0FB0
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x002D2BCF File Offset: 0x002D0FCF
		public TipSignal(string text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x002D2C00 File Offset: 0x002D1000
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x002D2C23 File Offset: 0x002D1023
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x002D2C54 File Offset: 0x002D1054
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x002D2C70 File Offset: 0x002D1070
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Tip(",
				this.text,
				", ",
				this.uniqueId,
				")"
			});
		}
	}
}
