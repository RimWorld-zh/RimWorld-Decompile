using System;

namespace RimWorld
{
	// Token: 0x02000307 RID: 775
	public struct Signal
	{
		// Token: 0x0400085A RID: 2138
		public string tag;

		// Token: 0x0400085B RID: 2139
		public object[] args;

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00070FA8 File Offset: 0x0006F3A8
		public Signal(string tag)
		{
			this.tag = tag;
			this.args = null;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00070FB9 File Offset: 0x0006F3B9
		public Signal(string tag, object[] args)
		{
			this.tag = tag;
			this.args = args;
		}
	}
}
