using Verse;

namespace RimWorld
{
	public class CompProperties_Schedule : CompProperties
	{
		public float startTime = 0f;

		public float endTime = 1f;

		public string offMessage = (string)null;

		public CompProperties_Schedule()
		{
			base.compClass = typeof(CompSchedule);
		}
	}
}
