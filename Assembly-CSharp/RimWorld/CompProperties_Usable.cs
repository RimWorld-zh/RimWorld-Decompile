using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000257 RID: 599
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x0005FE67 File Offset: 0x0005E267
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		// Token: 0x040004BB RID: 1211
		public JobDef useJob;

		// Token: 0x040004BC RID: 1212
		[MustTranslate]
		public string useLabel;

		// Token: 0x040004BD RID: 1213
		public int useDuration = 100;
	}
}
