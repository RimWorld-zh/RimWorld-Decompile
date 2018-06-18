using System;

namespace Verse
{
	// Token: 0x02000E7D RID: 3709
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x0600575A RID: 22362 RVA: 0x001B2B46 File Offset: 0x001B0F46
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x040039D7 RID: 14807
		public float lineHeight = 20f;
	}
}
