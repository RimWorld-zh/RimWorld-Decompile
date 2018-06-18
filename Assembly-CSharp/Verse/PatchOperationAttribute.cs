using System;

namespace Verse
{
	// Token: 0x02000CD9 RID: 3289
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x0600487B RID: 18555 RVA: 0x002606FC File Offset: 0x0025EAFC
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}

		// Token: 0x04003108 RID: 12552
		protected string attribute;
	}
}
