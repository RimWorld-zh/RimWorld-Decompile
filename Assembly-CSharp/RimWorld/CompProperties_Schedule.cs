using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200025F RID: 607
	public class CompProperties_Schedule : CompProperties
	{
		// Token: 0x06000A9C RID: 2716 RVA: 0x0005FF3F File Offset: 0x0005E33F
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}

		// Token: 0x040004C2 RID: 1218
		public float startTime = 0f;

		// Token: 0x040004C3 RID: 1219
		public float endTime = 1f;

		// Token: 0x040004C4 RID: 1220
		[MustTranslate]
		public string offMessage = null;
	}
}
