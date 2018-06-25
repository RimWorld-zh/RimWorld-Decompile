using System;

namespace Verse
{
	// Token: 0x02000CD8 RID: 3288
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x04003113 RID: 12563
		protected string attribute;

		// Token: 0x0600488F RID: 18575 RVA: 0x00261BF0 File Offset: 0x0025FFF0
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}
	}
}
