using System;

namespace Verse
{
	// Token: 0x02000CD6 RID: 3286
	public abstract class PatchOperationAttribute : PatchOperationPathed
	{
		// Token: 0x04003113 RID: 12563
		protected string attribute;

		// Token: 0x0600488C RID: 18572 RVA: 0x00261B14 File Offset: 0x0025FF14
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.attribute);
		}
	}
}
