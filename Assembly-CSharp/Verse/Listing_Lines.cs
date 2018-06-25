using System;

namespace Verse
{
	// Token: 0x02000E7F RID: 3711
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x040039EF RID: 14831
		public float lineHeight = 20f;

		// Token: 0x0600577E RID: 22398 RVA: 0x001B30D6 File Offset: 0x001B14D6
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}
	}
}
