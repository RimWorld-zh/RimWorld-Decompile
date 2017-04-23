using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_CategoryMTB.<MakeIntervalIncidents>c__IteratorA7 <MakeIntervalIncidents>c__IteratorA = new StorytellerComp_CategoryMTB.<MakeIntervalIncidents>c__IteratorA7();
			<MakeIntervalIncidents>c__IteratorA.target = target;
			<MakeIntervalIncidents>c__IteratorA.<$>target = target;
			<MakeIntervalIncidents>c__IteratorA.<>f__this = this;
			StorytellerComp_CategoryMTB.<MakeIntervalIncidents>c__IteratorA7 expr_1C = <MakeIntervalIncidents>c__IteratorA;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
