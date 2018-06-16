using System;

namespace Verse
{
	// Token: 0x02000E7E RID: 3710
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x0600575C RID: 22364 RVA: 0x001B2A7E File Offset: 0x001B0E7E
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x040039D9 RID: 14809
		public float lineHeight = 20f;
	}
}
