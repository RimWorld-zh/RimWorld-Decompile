using System;

namespace Verse
{
	// Token: 0x02000EBC RID: 3772
	public struct LabelValue
	{
		// Token: 0x04003B7F RID: 15231
		private string label;

		// Token: 0x04003B80 RID: 15232
		private string value;

		// Token: 0x0600593B RID: 22843 RVA: 0x002DC89D File Offset: 0x002DAC9D
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x0600593C RID: 22844 RVA: 0x002DC8B8 File Offset: 0x002DACB8
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600593D RID: 22845 RVA: 0x002DC8D4 File Offset: 0x002DACD4
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x002DC8F0 File Offset: 0x002DACF0
		public override string ToString()
		{
			return this.label;
		}
	}
}
