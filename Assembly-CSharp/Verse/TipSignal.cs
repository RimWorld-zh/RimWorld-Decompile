using System;

namespace Verse
{
	// Token: 0x02000E92 RID: 3730
	public struct TipSignal
	{
		// Token: 0x04003A38 RID: 14904
		public string text;

		// Token: 0x04003A39 RID: 14905
		public Func<string> textGetter;

		// Token: 0x04003A3A RID: 14906
		public int uniqueId;

		// Token: 0x04003A3B RID: 14907
		public TooltipPriority priority;

		// Token: 0x0600580B RID: 22539 RVA: 0x002D29A5 File Offset: 0x002D0DA5
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x002D29C4 File Offset: 0x002D0DC4
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x002D29E3 File Offset: 0x002D0DE3
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

		// Token: 0x0600580E RID: 22542 RVA: 0x002D2A14 File Offset: 0x002D0E14
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x002D2A37 File Offset: 0x002D0E37
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x002D2A68 File Offset: 0x002D0E68
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x002D2A84 File Offset: 0x002D0E84
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
