using System;

namespace Verse
{
	// Token: 0x02000CD1 RID: 3281
	public abstract class PatchOperationPathed : PatchOperation
	{
		// Token: 0x0600486F RID: 18543 RVA: 0x00260120 File Offset: 0x0025E520
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}

		// Token: 0x040030FC RID: 12540
		protected string xpath;
	}
}
