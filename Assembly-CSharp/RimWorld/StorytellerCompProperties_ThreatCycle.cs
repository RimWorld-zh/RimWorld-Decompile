using System;

namespace RimWorld
{
	// Token: 0x02000376 RID: 886
	public class StorytellerCompProperties_ThreatCycle : StorytellerCompProperties
	{
		// Token: 0x0400095C RID: 2396
		public float mtbDaysThreatSmall;

		// Token: 0x0400095D RID: 2397
		public float mtbDaysThreatBig;

		// Token: 0x0400095E RID: 2398
		public float threatOffDays;

		// Token: 0x0400095F RID: 2399
		public float threatOnDays;

		// Token: 0x04000960 RID: 2400
		public float minDaysBetweenThreatBigs;

		// Token: 0x04000961 RID: 2401
		public float minDaysBeforeNonRaidThreatBig = 20f;

		// Token: 0x04000962 RID: 2402
		public IncidentCategoryDef threatSmallCategory;

		// Token: 0x04000963 RID: 2403
		public IncidentCategoryDef threatBigCategory;

		// Token: 0x06000F4C RID: 3916 RVA: 0x0008198C File Offset: 0x0007FD8C
		public StorytellerCompProperties_ThreatCycle()
		{
			this.compClass = typeof(StorytellerComp_ThreatCycle);
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000F4D RID: 3917 RVA: 0x000819B0 File Offset: 0x0007FDB0
		public float ThreatCycleTotalDays
		{
			get
			{
				return this.threatOffDays + this.threatOnDays;
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x000819D2 File Offset: 0x0007FDD2
		public override void ResolveReferences(StorytellerDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.threatSmallCategory == null)
			{
				this.threatSmallCategory = IncidentCategoryDefOf.ThreatSmall;
			}
			if (this.threatBigCategory == null)
			{
				this.threatBigCategory = IncidentCategoryDefOf.ThreatBig;
			}
		}
	}
}
