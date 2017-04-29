using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class StorytellerComp_Disease : StorytellerComp
	{
		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_Disease.<MakeIntervalIncidents>c__IteratorAA <MakeIntervalIncidents>c__IteratorAA = new StorytellerComp_Disease.<MakeIntervalIncidents>c__IteratorAA();
			<MakeIntervalIncidents>c__IteratorAA.target = target;
			<MakeIntervalIncidents>c__IteratorAA.<$>target = target;
			<MakeIntervalIncidents>c__IteratorAA.<>f__this = this;
			StorytellerComp_Disease.<MakeIntervalIncidents>c__IteratorAA expr_1C = <MakeIntervalIncidents>c__IteratorAA;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
