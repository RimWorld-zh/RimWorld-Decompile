using System;
using System.Collections.Generic;
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
				return (StorytellerCompProperties_RandomMain)base.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			_003CMakeIntervalIncidents_003Ec__Iterator0 _003CMakeIntervalIncidents_003Ec__Iterator = (_003CMakeIntervalIncidents_003Ec__Iterator0)/*Error near IL_0032: stateMachine*/;
			if (!Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
				yield break;
			List<IncidentCategory> triedCategories = new List<IncidentCategory>();
			IEnumerable<IncidentDef> options;
			while (true)
			{
				_003CMakeIntervalIncidents_003Ec__Iterator0 _003CMakeIntervalIncidents_003Ec__Iterator2 = (_003CMakeIntervalIncidents_003Ec__Iterator0)/*Error near IL_007f: stateMachine*/;
				if (triedCategories.Count < this.Props.categoryWeights.Count)
				{
					IncidentCategory category = this.DecideCategory(target, triedCategories);
					triedCategories.Add(category);
					IncidentParms parms = this.GenerateParms(category, target);
					options = from d in DefDatabase<IncidentDef>.AllDefs
					where d.category == category && d.Worker.CanFireNow(target) && (!d.NeedsParms || d.minThreatPoints <= parms.points)
					select d;
					if (options.Any())
						break;
					continue;
				}
				yield break;
			}
			IncidentDef incDef;
			if (!options.TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)base.IncidentChanceFinal, out incDef))
				yield break;
			yield return new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private IncidentCategory DecideCategory(IIncidentTarget target, List<IncidentCategory> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategory.ThreatBig))
			{
				int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
				if ((float)num > 60000.0 * this.Props.maxThreatBigIntervalDays)
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
