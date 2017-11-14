namespace Verse
{
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		public float severityPerDay;

		public HediffCompProperties_SeverityPerDay()
		{
			base.compClass = typeof(HediffComp_SeverityPerDay);
		}
	}
}
