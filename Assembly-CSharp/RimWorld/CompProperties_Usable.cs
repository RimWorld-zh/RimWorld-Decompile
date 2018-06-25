using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000259 RID: 601
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x040004BB RID: 1211
		public JobDef useJob;

		// Token: 0x040004BC RID: 1212
		[MustTranslate]
		public string useLabel;

		// Token: 0x040004BD RID: 1213
		public int useDuration = 100;

		// Token: 0x06000A98 RID: 2712 RVA: 0x0005FFB7 File Offset: 0x0005E3B7
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}
	}
}
