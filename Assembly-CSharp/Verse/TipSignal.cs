using System;

namespace Verse
{
	// Token: 0x02000E90 RID: 3728
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

		// Token: 0x06005807 RID: 22535 RVA: 0x002D2879 File Offset: 0x002D0C79
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x002D2898 File Offset: 0x002D0C98
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x002D28B7 File Offset: 0x002D0CB7
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

		// Token: 0x0600580A RID: 22538 RVA: 0x002D28E8 File Offset: 0x002D0CE8
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x002D290B File Offset: 0x002D0D0B
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x002D293C File Offset: 0x002D0D3C
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x002D2958 File Offset: 0x002D0D58
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
