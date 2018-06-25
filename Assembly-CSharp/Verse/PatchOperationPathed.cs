using System;

namespace Verse
{
	// Token: 0x02000CCF RID: 3279
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x04003105 RID: 12549
		protected string xpath;

		// Token: 0x06004881 RID: 18561 RVA: 0x002615EC File Offset: 0x0025F9EC
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}
	}
}
