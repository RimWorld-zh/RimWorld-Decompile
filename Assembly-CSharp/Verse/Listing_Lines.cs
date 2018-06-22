using System;

namespace Verse
{
	// Token: 0x02000E7C RID: 3708
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x0600577A RID: 22394 RVA: 0x001B2D2E File Offset: 0x001B112E
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x040039E7 RID: 14823
		public float lineHeight = 20f;
	}
}
