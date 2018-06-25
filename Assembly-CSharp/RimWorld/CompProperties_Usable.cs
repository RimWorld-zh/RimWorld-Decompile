using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000259 RID: 601
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x040004BD RID: 1213
		public JobDef useJob;

		// Token: 0x040004BE RID: 1214
		[MustTranslate]
		public string useLabel;

		// Token: 0x040004BF RID: 1215
		public int useDuration = 100;

		// Token: 0x06000A97 RID: 2711 RVA: 0x0005FFB3 File Offset: 0x0005E3B3
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}
	}
}
