using System;

namespace Verse
{
	// Token: 0x02000D1D RID: 3357
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x060049D9 RID: 18905 RVA: 0x002695D0 File Offset: 0x002679D0
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x04003210 RID: 12816
		public float baseTendDurationHours = -1f;

		// Token: 0x04003211 RID: 12817
		public float tendOverlapHours = 4f;

		// Token: 0x04003212 RID: 12818
		public bool tendAllAtOnce = false;

		// Token: 0x04003213 RID: 12819
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x04003214 RID: 12820
		public float severityPerDayTended = 0f;

		// Token: 0x04003215 RID: 12821
		public bool showTendQuality = true;

		// Token: 0x04003216 RID: 12822
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell = null;

		// Token: 0x04003217 RID: 12823
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner = null;

		// Token: 0x04003218 RID: 12824
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell = null;
	}
}
