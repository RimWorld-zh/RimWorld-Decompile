using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)this.props;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>c__IteratorA7 <MakeIntervalIncidents>c__IteratorA = new StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>c__IteratorA7();
			<MakeIntervalIncidents>c__IteratorA.target = target;
			<MakeIntervalIncidents>c__IteratorA.<$>target = target;
			<MakeIntervalIncidents>c__IteratorA.<>f__this = this;
			StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>c__IteratorA7 expr_1C = <MakeIntervalIncidents>c__IteratorA;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
