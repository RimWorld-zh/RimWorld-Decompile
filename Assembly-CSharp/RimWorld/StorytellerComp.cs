using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class StorytellerComp
	{
		public StorytellerCompProperties props;

		public abstract IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target);

		public virtual IncidentParms GenerateParms(IncidentCategory incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, incCat, target);
		}

		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategory cat, IIncidentTarget target)
		{
			return from x in DefDatabase<IncidentDef>.AllDefsListForReading
			where x.category == cat && x.Worker.CanFireNow(target)
			select x;
		}

		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			if (def.chanceFactorByPopulationCurve == null)
			{
				return 1f;
			}
			int num = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Colonists.Count();
			return def.chanceFactorByPopulationCurve.Evaluate((float)num);
		}

		protected float IncidentChanceFactor_PopulationIntent(IncidentDef def)
		{
			switch (def.populationEffect)
			{
			case IncidentPopulationEffect.None:
			{
				return 1f;
			}
			case IncidentPopulationEffect.Increase:
			{
				return Mathf.Max(Find.Storyteller.intenderPopulation.PopulationIntent, this.props.minIncChancePopulationIntentFactor);
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
		}

		protected float IncidentChanceFinal(IncidentDef def)
		{
			float adjustedChance = def.Worker.AdjustedChance;
			adjustedChance *= this.IncidentChanceFactor_CurrentPopulation(def);
			adjustedChance *= this.IncidentChanceFactor_PopulationIntent(def);
			return Mathf.Max(0f, adjustedChance);
		}

		public virtual void DebugTablesIncidentChances(IncidentCategory cat)
		{
			DebugTables.MakeTablesDialog(from d in DefDatabase<IncidentDef>.AllDefs
			where d.category == cat
			orderby this.IncidentChanceFinal(d) descending
			select d, new TableDataGetter<IncidentDef>("defName", (Func<IncidentDef, string>)((IncidentDef d) => d.defName)), new TableDataGetter<IncidentDef>("baseChance", (Func<IncidentDef, string>)((IncidentDef d) => d.baseChance.ToString())), new TableDataGetter<IncidentDef>("AdjustedChance", (Func<IncidentDef, string>)((IncidentDef d) => d.Worker.AdjustedChance.ToString())), new TableDataGetter<IncidentDef>("Factor-PopCurrent", (Func<IncidentDef, string>)((IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString())), new TableDataGetter<IncidentDef>("Factor-PopIntent", (Func<IncidentDef, string>)((IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString())), new TableDataGetter<IncidentDef>("final chance", (Func<IncidentDef, string>)((IncidentDef d) => this.IncidentChanceFinal(d).ToString())), new TableDataGetter<IncidentDef>("vismap-usable", (Func<IncidentDef, string>)((IncidentDef d) => (Find.VisibleMap != null) ? ((!this.UsableIncidentsInCategory(cat, Find.VisibleMap).Contains(d)) ? string.Empty : "V") : "-")), new TableDataGetter<IncidentDef>("world-usable", (Func<IncidentDef, string>)((IncidentDef d) => (!this.UsableIncidentsInCategory(cat, Find.World).Contains(d)) ? string.Empty : "W")), new TableDataGetter<IncidentDef>("pop-current", (Func<IncidentDef, string>)((IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Colonists.Count().ToString())), new TableDataGetter<IncidentDef>("pop-intent", (Func<IncidentDef, string>)((IncidentDef d) => Find.Storyteller.intenderPopulation.PopulationIntent.ToString("F3"))));
		}
	}
}
