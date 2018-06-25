using System;

namespace Verse
{
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		public float baseTendDurationHours = -1f;

		public float tendOverlapHours = 4f;

		public bool tendAllAtOnce = false;

		public int disappearsAtTotalTendQuality = -1;

		public float severityPerDayTended = 0f;

		public bool showTendQuality = true;

		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell = null;

		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner = null;

		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell = null;

		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}
	}
}
