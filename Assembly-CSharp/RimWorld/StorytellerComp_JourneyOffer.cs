using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_JourneyOffer : StorytellerComp
	{
		private const int StartOnDay = 14;

		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_JourneyOffer.<MakeIntervalIncidents>c__IteratorAA <MakeIntervalIncidents>c__IteratorAA = new StorytellerComp_JourneyOffer.<MakeIntervalIncidents>c__IteratorAA();
			<MakeIntervalIncidents>c__IteratorAA.target = target;
			<MakeIntervalIncidents>c__IteratorAA.<$>target = target;
			<MakeIntervalIncidents>c__IteratorAA.<>f__this = this;
			StorytellerComp_JourneyOffer.<MakeIntervalIncidents>c__IteratorAA expr_1C = <MakeIntervalIncidents>c__IteratorAA;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
