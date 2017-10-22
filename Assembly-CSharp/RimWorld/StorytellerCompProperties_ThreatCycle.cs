namespace RimWorld
{
	public class StorytellerCompProperties_ThreatCycle : StorytellerCompProperties
	{
		public float mtbDaysThreatSmall;

		public float mtbDaysThreatBig;

		public float threatOffDays;

		public float threatOnDays;

		public float minDaysBetweenThreatBigs;

		public float ThreatCycleTotalDays
		{
			get
			{
				return this.threatOffDays + this.threatOnDays;
			}
		}

		public StorytellerCompProperties_ThreatCycle()
		{
			base.compClass = typeof(StorytellerComp_ThreatCycle);
		}
	}
}
