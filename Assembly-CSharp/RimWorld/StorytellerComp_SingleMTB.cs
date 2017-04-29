using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__IteratorAE <MakeIntervalIncidents>c__IteratorAE = new StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__IteratorAE();
			<MakeIntervalIncidents>c__IteratorAE.target = target;
			<MakeIntervalIncidents>c__IteratorAE.<$>target = target;
			<MakeIntervalIncidents>c__IteratorAE.<>f__this = this;
			StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__IteratorAE expr_1C = <MakeIntervalIncidents>c__IteratorAE;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
