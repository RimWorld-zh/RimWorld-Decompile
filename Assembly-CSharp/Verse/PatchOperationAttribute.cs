using System;

namespace Verse
{
	// Token: 0x02000CDA RID: 3290
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x0600487D RID: 18557 RVA: 0x00260724 File Offset: 0x0025EB24
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x0400310A RID: 12554
		protected string attribute;
	}
}
