using System;

namespace RimWorld
{
	// Token: 0x02000378 RID: 888
	public class StorytellerCompProperties_ThreatCycle : StorytellerCompProperties
	{
		// Token: 0x0400095F RID: 2399
		public float mtbDaysThreatSmall;

		// Token: 0x04000960 RID: 2400
		public float mtbDaysThreatBig;

		// Token: 0x04000961 RID: 2401
		public float threatOffDays;

		// Token: 0x04000962 RID: 2402
		public float threatOnDays;

		// Token: 0x04000963 RID: 2403
		public float minDaysBetweenThreatBigs;

		// Token: 0x04000964 RID: 2404
		public float minDaysBeforeNonRaidThreatBig = 20f;

		// Token: 0x04000965 RID: 2405
		public IncidentCategoryDef threatSmallCategory;

		// Token: 0x04000966 RID: 2406
		public IncidentCategoryDef threatBigCategory;

		// Token: 0x06000F4F RID: 3919 RVA: 0x00081AEC File Offset: 0x0007FEEC
		public StorytellerCompProperties_ThreatCycle()
		{
			this.compClass = typeof(StorytellerComp_ThreatCycle);
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x00081B10 File Offset: 0x0007FF10
		public float ThreatCycleTotalDays
		{
			get
			{
				return this.threatOffDays + this.threatOnDays;
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00081B32 File Offset: 0x0007FF32
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
