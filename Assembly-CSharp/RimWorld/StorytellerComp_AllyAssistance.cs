using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class StorytellerComp_AllyAssistance : StorytellerComp
	{
		private StorytellerCompProperties_AllyAssistance Props
		{
			get
			{
				return (StorytellerCompProperties_AllyAssistance)this.props;
			}
		}

		private float IncidentMTBDays
		{
			get
			{
				return this.Props.baseMtb * StorytellerUtility.AllyIncidentMTBMultiplier();
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_AllyAssistance.<MakeIntervalIncidents>c__IteratorA4 <MakeIntervalIncidents>c__IteratorA = new StorytellerComp_AllyAssistance.<MakeIntervalIncidents>c__IteratorA4();
			<MakeIntervalIncidents>c__IteratorA.target = target;
			<MakeIntervalIncidents>c__IteratorA.<$>target = target;
			<MakeIntervalIncidents>c__IteratorA.<>f__this = this;
			StorytellerComp_AllyAssistance.<MakeIntervalIncidents>c__IteratorA4 expr_1C = <MakeIntervalIncidents>c__IteratorA;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
