using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000261 RID: 609
	public class CompProperties_Schedule : CompProperties
	{
		// Token: 0x040004C4 RID: 1220
		public float startTime = 0f;

		// Token: 0x040004C5 RID: 1221
		public float endTime = 1f;

		// Token: 0x040004C6 RID: 1222
		[MustTranslate]
		public string offMessage = null;

		// Token: 0x06000A9F RID: 2719 RVA: 0x0006008B File Offset: 0x0005E48B
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}
	}
}
