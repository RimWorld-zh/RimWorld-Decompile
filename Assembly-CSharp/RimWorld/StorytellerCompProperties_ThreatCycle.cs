using System;

namespace RimWorld
{
	// Token: 0x02000378 RID: 888
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

		// Token: 0x06000F50 RID: 3920 RVA: 0x00081ADC File Offset: 0x0007FEDC
		public StorytellerCompProperties_ThreatCycle()
		{
			this.compClass = typeof(StorytellerComp_ThreatCycle);
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00081B00 File Offset: 0x0007FF00
		public float ThreatCycleTotalDays
		{
			get
			{
				return this.threatOffDays + this.threatOnDays;
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00081B22 File Offset: 0x0007FF22
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
