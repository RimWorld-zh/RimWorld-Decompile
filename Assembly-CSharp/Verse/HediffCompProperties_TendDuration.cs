using System;

namespace Verse
{
	// Token: 0x02000D19 RID: 3353
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x060049E8 RID: 18920 RVA: 0x0026A9DC File Offset: 0x00268DDC
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x04003219 RID: 12825
		public float baseTendDurationHours = -1f;

		// Token: 0x0400321A RID: 12826
		public float tendOverlapHours = 4f;

		// Token: 0x0400321B RID: 12827
		public bool tendAllAtOnce = false;

		// Token: 0x0400321C RID: 12828
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x0400321D RID: 12829
		public float severityPerDayTended = 0f;

		// Token: 0x0400321E RID: 12830
		public bool showTendQuality = true;

		// Token: 0x0400321F RID: 12831
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell = null;

		// Token: 0x04003220 RID: 12832
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner = null;

		// Token: 0x04003221 RID: 12833
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell = null;
	}
}
