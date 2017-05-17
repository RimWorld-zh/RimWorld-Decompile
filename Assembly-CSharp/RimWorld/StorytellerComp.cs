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

		protected virtual float IncidentChancePopulationFactor(IncidentDef def)
		{
			float a = 1f;
			if (def.populationEffect >= IncidentPopulationEffect.Increase)
			{
				a = Find.Storyteller.intenderPopulation.PopulationIntent;
			}
			else if (def.populationEffect <= IncidentPopulationEffect.Decrease)
			{
				a = -Find.Storyteller.intenderPopulation.PopulationIntent;
			}
			return Mathf.Max(a, 0.05f);
		}

		protected float IncidentChanceAdjustedForPopulation(IncidentDef def)
		{
			return Mathf.Max(0f, def.Worker.AdjustedChance * this.IncidentChancePopulationFactor(def));
		}

		public virtual void DebugTablesIncidentChances(IncidentCategory cat)
		{
			IEnumerable<IncidentDef> arg_14D_0 = from d in DefDatabase<IncidentDef>.AllDefs
			where d.category == cat
			orderby this.IncidentChanceAdjustedForPopulation(d) descending
			select d;
			TableDataGetter<IncidentDef>[] expr_41 = new TableDataGetter<IncidentDef>[8];
			expr_41[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			expr_41[1] = new TableDataGetter<IncidentDef>("baseChance", (IncidentDef d) => d.baseChance.ToString());
			expr_41[2] = new TableDataGetter<IncidentDef>("AdjustedChance", (IncidentDef d) => d.Worker.AdjustedChance.ToString());
			expr_41[3] = new TableDataGetter<IncidentDef>("PopulationFactor", (IncidentDef d) => this.IncidentChancePopulationFactor(d).ToString());
			expr_41[4] = new TableDataGetter<IncidentDef>("final chance", (IncidentDef d) => this.IncidentChanceAdjustedForPopulation(d).ToString());
			expr_41[5] = new TableDataGetter<IncidentDef>("vismap-usable", (IncidentDef d) => (Find.VisibleMap != null) ? ((!this.UsableIncidentsInCategory(cat, Find.VisibleMap).Contains(d)) ? string.Empty : "V") : "-");
			expr_41[6] = new TableDataGetter<IncidentDef>("world-usable", (IncidentDef d) => (!this.UsableIncidentsInCategory(cat, Find.World).Contains(d)) ? string.Empty : "W");
			expr_41[7] = new TableDataGetter<IncidentDef>("pop-intent", (IncidentDef d) => Find.Storyteller.intenderPopulation.PopulationIntent.ToString("F3"));
			DebugTables.MakeTablesDialog<IncidentDef>(arg_14D_0, expr_41);
		}
	}
}
