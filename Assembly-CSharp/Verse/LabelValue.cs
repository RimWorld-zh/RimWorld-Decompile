using System;

namespace Verse
{
	// Token: 0x02000EBB RID: 3771
	public struct LabelValue
	{
		// Token: 0x04003B77 RID: 15223
		private string label;

		// Token: 0x04003B78 RID: 15224
		private string value;

		// Token: 0x0600593B RID: 22843 RVA: 0x002DC6B1 File Offset: 0x002DAAB1
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x0600593C RID: 22844 RVA: 0x002DC6CC File Offset: 0x002DAACC
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600593D RID: 22845 RVA: 0x002DC6E8 File Offset: 0x002DAAE8
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x002DC704 File Offset: 0x002DAB04
		public override string ToString()
		{
			return this.label;
		}
	}
}
