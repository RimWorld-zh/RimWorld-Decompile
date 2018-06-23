using System;

namespace Verse
{
	// Token: 0x02000EB9 RID: 3769
	public struct LabelValue
	{
		// Token: 0x04003B77 RID: 15223
		private string label;

		// Token: 0x04003B78 RID: 15224
		private string value;

		// Token: 0x06005937 RID: 22839 RVA: 0x002DC585 File Offset: 0x002DA985
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06005938 RID: 22840 RVA: 0x002DC5A0 File Offset: 0x002DA9A0
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06005939 RID: 22841 RVA: 0x002DC5BC File Offset: 0x002DA9BC
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600593A RID: 22842 RVA: 0x002DC5D8 File Offset: 0x002DA9D8
		public override string ToString()
		{
			return this.label;
		}
	}
}
