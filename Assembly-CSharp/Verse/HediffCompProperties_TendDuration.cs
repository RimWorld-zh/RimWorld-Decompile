using System;

namespace Verse
{
	// Token: 0x02000D1C RID: 3356
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x060049D7 RID: 18903 RVA: 0x002695A8 File Offset: 0x002679A8
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x0400320E RID: 12814
		public float baseTendDurationHours = -1f;

		// Token: 0x0400320F RID: 12815
		public float tendOverlapHours = 4f;

		// Token: 0x04003210 RID: 12816
		public bool tendAllAtOnce = false;

		// Token: 0x04003211 RID: 12817
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x04003212 RID: 12818
		public float severityPerDayTended = 0f;

		// Token: 0x04003213 RID: 12819
		public bool showTendQuality = true;

		// Token: 0x04003214 RID: 12820
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell = null;

		// Token: 0x04003215 RID: 12821
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner = null;

		// Token: 0x04003216 RID: 12822
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell = null;
	}
}
