using System;

namespace Verse
{
	// Token: 0x02000CD0 RID: 3280
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x0600486D RID: 18541 RVA: 0x002600F8 File Offset: 0x0025E4F8
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x040030FA RID: 12538
		protected string xpath;
	}
}
