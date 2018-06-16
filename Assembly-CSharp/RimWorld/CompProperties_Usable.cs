using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000257 RID: 599
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x0005FE0B File Offset: 0x0005E20B
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		// Token: 0x040004BD RID: 1213
		public JobDef useJob;

		// Token: 0x040004BE RID: 1214
		[MustTranslate]
		public string useLabel;

		// Token: 0x040004BF RID: 1215
		public int useDuration = 100;
	}
}
