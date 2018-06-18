using System;

namespace Verse
{
	// Token: 0x02000EBA RID: 3770
	public struct LabelValue
	{
		// Token: 0x06005916 RID: 22806 RVA: 0x002DA939 File Offset: 0x002D8D39
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06005917 RID: 22807 RVA: 0x002DA954 File Offset: 0x002D8D54
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06005918 RID: 22808 RVA: 0x002DA970 File Offset: 0x002D8D70
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06005919 RID: 22809 RVA: 0x002DA98C File Offset: 0x002D8D8C
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x04003B67 RID: 15207
		private string label;

		// Token: 0x04003B68 RID: 15208
		private string value;
	}
}
