using System;

namespace Verse
{
	// Token: 0x02000D1C RID: 3356
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x04003220 RID: 12832
		public float baseTendDurationHours = -1f;

		// Token: 0x04003221 RID: 12833
		public float tendOverlapHours = 4f;

		// Token: 0x04003222 RID: 12834
		public bool tendAllAtOnce = false;

		// Token: 0x04003223 RID: 12835
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x04003224 RID: 12836
		public float severityPerDayTended = 0f;

		// Token: 0x04003225 RID: 12837
		public bool showTendQuality = true;

		// Token: 0x04003226 RID: 12838
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell = null;

		// Token: 0x04003227 RID: 12839
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner = null;

		// Token: 0x04003228 RID: 12840
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell = null;

		// Token: 0x060049EB RID: 18923 RVA: 0x0026AD98 File Offset: 0x00269198
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}
	}
}
