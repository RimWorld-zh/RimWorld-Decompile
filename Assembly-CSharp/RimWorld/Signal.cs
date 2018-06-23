using System;

namespace RimWorld
{
	// Token: 0x02000305 RID: 773
	public struct Signal
	{
		// Token: 0x0400085A RID: 2138
		public string tag;

		// Token: 0x0400085B RID: 2139
		public object[] args;

		// Token: 0x06000CDE RID: 3294 RVA: 0x00070E58 File Offset: 0x0006F258
		public Signal(string tag)
		{
			this.tag = tag;
			this.args = null;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00070E69 File Offset: 0x0006F269
		public Signal(string tag, object[] args)
		{
			this.tag = tag;
			this.args = args;
		}
	}
}
