using System;

namespace Verse
{
	// Token: 0x02000CCD RID: 3277
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x04003105 RID: 12549
		protected string xpath;

		// Token: 0x0600487E RID: 18558 RVA: 0x00261510 File Offset: 0x0025F910
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}
	}
}
