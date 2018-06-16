using System;

namespace Verse
{
	// Token: 0x02000EBB RID: 3771
	public struct LabelValue
	{
		// Token: 0x06005918 RID: 22808 RVA: 0x002DA901 File Offset: 0x002D8D01
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06005919 RID: 22809 RVA: 0x002DA91C File Offset: 0x002D8D1C
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x0600591A RID: 22810 RVA: 0x002DA938 File Offset: 0x002D8D38
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x002DA954 File Offset: 0x002D8D54
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x04003B68 RID: 15208
		private string label;

		// Token: 0x04003B69 RID: 15209
		private string value;
	}
}
