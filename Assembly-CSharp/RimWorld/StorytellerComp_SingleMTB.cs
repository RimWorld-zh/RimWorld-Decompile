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
			StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__IteratorAD <MakeIntervalIncidents>c__IteratorAD = new StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__IteratorAD();
			<MakeIntervalIncidents>c__IteratorAD.target = target;
			<MakeIntervalIncidents>c__IteratorAD.<$>target = target;
			<MakeIntervalIncidents>c__IteratorAD.<>f__this = this;
			StorytellerComp_SingleMTB.<MakeIntervalIncidents>c__IteratorAD expr_1C = <MakeIntervalIncidents>c__IteratorAD;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
