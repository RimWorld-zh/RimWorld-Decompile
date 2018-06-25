using System;

namespace Verse
{
	// Token: 0x02000E7E RID: 3710
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x040039E7 RID: 14823
		public float lineHeight = 20f;

		// Token: 0x0600577E RID: 22398 RVA: 0x001B2E6E File Offset: 0x001B126E
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}
	}
}
