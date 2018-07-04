using System;
using Verse;

namespace RimWorld
{
	public class StorytellerCompProperties_ThreatCycle : StorytellerCompProperties
	{
		public ThreatCycleMode mode = ThreatCycleMode.Normal;

		private float mtbDaysThreatBig = 0f;

		public float mtbDaysThreatSmall;

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

		public float MTBDaysThreatBigCurrentDifficulty
		{
			get
			{
				ThreatCycleMode threatCycleMode = this.mode;
				float result;
				if (threatCycleMode != ThreatCycleMode.Normal)
				{
					if (threatCycleMode != ThreatCycleMode.RaidBeacon)
					{
						throw new Exception();
					}
					result = this.mtbDaysThreatBig * Find.Storyteller.difficulty.raidBeaconThreatMtbFactor;
				}
				else
				{
					result = this.mtbDaysThreatBig;
				}
				return result;
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
