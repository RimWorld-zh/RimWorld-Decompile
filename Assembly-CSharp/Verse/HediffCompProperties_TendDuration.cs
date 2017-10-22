namespace Verse
{
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		public int tendDuration = -1;

		public bool tendAllAtOnce = false;

		public int disappearsAtTendedCount = -1;

		public float severityPerDayTended = 0f;

		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell = (string)null;

		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner = (string)null;

		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell = (string)null;

		public HediffCompProperties_TendDuration()
		{
			base.compClass = typeof(HediffComp_TendDuration);
		}
	}
}
