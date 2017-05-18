using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_RandomMain.<MakeIntervalIncidents>c__IteratorAC <MakeIntervalIncidents>c__IteratorAC = new StorytellerComp_RandomMain.<MakeIntervalIncidents>c__IteratorAC();
			<MakeIntervalIncidents>c__IteratorAC.target = target;
			<MakeIntervalIncidents>c__IteratorAC.<$>target = target;
			<MakeIntervalIncidents>c__IteratorAC.<>f__this = this;
			StorytellerComp_RandomMain.<MakeIntervalIncidents>c__IteratorAC expr_1C = <MakeIntervalIncidents>c__IteratorAC;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		private IncidentCategory DecideCategory(IIncidentTarget target, List<IncidentCategory> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategory.ThreatBig))
			{
				int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
				if ((float)num > 60000f * this.Props.maxThreatBigIntervalDays)
				{
					return IncidentCategory.ThreatBig;
				}
			}
			return (from cw in this.Props.categoryWeights
			where !skipCategories.Contains(cw.category)
			select cw).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
		}

		public override IncidentParms GenerateParms(IncidentCategory incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, incCat, target);
			incidentParms.points *= Rand.Range(0.5f, 1.5f);
			return incidentParms;
		}
	}
}
