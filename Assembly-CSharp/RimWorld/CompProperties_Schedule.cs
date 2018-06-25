using System;
using Verse;

namespace RimWorld
{
	public class CompProperties_Schedule : CompProperties
	{
		public float startTime = 0f;

		public float endTime = 1f;

		[MustTranslate]
		public string offMessage = null;

		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}
	}
}
