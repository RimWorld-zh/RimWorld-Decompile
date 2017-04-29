using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_ClassicIntro : StorytellerComp
	{
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__IteratorA9 <MakeIntervalIncidents>c__IteratorA = new StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__IteratorA9();
			<MakeIntervalIncidents>c__IteratorA.target = target;
			<MakeIntervalIncidents>c__IteratorA.<$>target = target;
			<MakeIntervalIncidents>c__IteratorA.<>f__this = this;
			StorytellerComp_ClassicIntro.<MakeIntervalIncidents>c__IteratorA9 expr_1C = <MakeIntervalIncidents>c__IteratorA;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
