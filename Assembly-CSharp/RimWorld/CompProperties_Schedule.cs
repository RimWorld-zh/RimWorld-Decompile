using Verse;

namespace RimWorld
{
	public class CompProperties_Schedule : CompProperties
	{
		public float startTime;

		public float endTime = 1f;

		public string offMessage;

		public CompProperties_Schedule()
		{
			base.compClass = typeof(CompSchedule);
		}
	}
}
