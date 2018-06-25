using System;

namespace RimWorld
{
	// Token: 0x02000307 RID: 775
	public struct Signal
	{
		// Token: 0x0400085D RID: 2141
		public string tag;

		// Token: 0x0400085E RID: 2142
		public object[] args;

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00070FB0 File Offset: 0x0006F3B0
		public Signal(string tag)
		{
			this.tag = tag;
			this.args = null;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00070FC1 File Offset: 0x0006F3C1
		public Signal(string tag, object[] args)
		{
			this.tag = tag;
			this.args = args;
		}
	}
}
