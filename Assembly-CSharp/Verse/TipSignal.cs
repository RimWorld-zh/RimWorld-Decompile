using System;

namespace Verse
{
	// Token: 0x02000E91 RID: 3729
	public struct TipSignal
	{
		// Token: 0x060057E7 RID: 22503 RVA: 0x002D0C69 File Offset: 0x002CF069
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x002D0C88 File Offset: 0x002CF088
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
		}

		// Token: 0x060057E9 RID: 22505 RVA: 0x002D0CA7 File Offset: 0x002CF0A7
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

		// Token: 0x060057EA RID: 22506 RVA: 0x002D0CD8 File Offset: 0x002CF0D8
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
		}

		// Token: 0x060057EB RID: 22507 RVA: 0x002D0CFB File Offset: 0x002CF0FB
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
		}

		// Token: 0x060057EC RID: 22508 RVA: 0x002D0D2C File Offset: 0x002CF12C
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x002D0D48 File Offset: 0x002CF148
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

		// Token: 0x04003A28 RID: 14888
		public string text;

		// Token: 0x04003A29 RID: 14889
		public Func<string> textGetter;

		// Token: 0x04003A2A RID: 14890
		public int uniqueId;

		// Token: 0x04003A2B RID: 14891
		public TooltipPriority priority;
	}
}
