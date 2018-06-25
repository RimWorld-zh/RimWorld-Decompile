using System;

namespace Verse
{
	// Token: 0x02000CD9 RID: 3289
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x0400311A RID: 12570
		protected string attribute;

		// Token: 0x0600488F RID: 18575 RVA: 0x00261ED0 File Offset: 0x002602D0
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}
	}
}
