using System;

namespace RimWorld
{
	public class StorytellerCompProperties_ThreatCycle : StorytellerCompProperties
	{
		public float mtbDaysThreatSmall;

		public float mtbDaysThreatBig;

		public float threatOffDays;

		public float threatOnDays;

		public float minDaysBetweenThreatBigs;

		public float minDaysBeforeNonRaidThreatBig = 20f;

		public IncidentCategoryDef threatSmallCategory;

		public IncidentCategoryDef threatBigCategory;

		public StorytellerCompProperties_ThreatCycle()
		{
			this.compClass = typeof(StorytellerComp_ThreatCycle);
		}

		public float ThreatCycleTotalDays
		{
			get
			{
				return this.threatOffDays + this.threatOnDays;
			}
		}

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
