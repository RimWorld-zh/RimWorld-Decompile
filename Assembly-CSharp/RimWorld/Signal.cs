using System;

namespace RimWorld
{
	// Token: 0x02000305 RID: 773
	public struct Signal
	{
		// Token: 0x06000CDE RID: 3294 RVA: 0x00070DA4 File Offset: 0x0006F1A4
		public Signal(string tag)
		{
			this.tag = tag;
			this.args = null;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00070DB5 File Offset: 0x0006F1B5
		public Signal(string tag, object[] args)
		{
			this.tag = tag;
			this.args = args;
		}

		// Token: 0x04000858 RID: 2136
		public string tag;

		// Token: 0x04000859 RID: 2137
		public object[] args;
	}
}
