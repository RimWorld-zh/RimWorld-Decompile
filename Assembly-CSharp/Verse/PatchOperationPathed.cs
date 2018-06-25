using System;

namespace Verse
{
	// Token: 0x02000CD0 RID: 3280
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x0400310C RID: 12556
		protected string xpath;

		// Token: 0x06004881 RID: 18561 RVA: 0x002618CC File Offset: 0x0025FCCC
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}
	}
}
