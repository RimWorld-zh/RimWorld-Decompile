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

		protected virtual float IncidentChanceAdjustedForPopulation(IncidentDef def)
		{
			float num = 1f;
			if (def.populationEffect >= IncidentPopulationEffect.Increase)
			{
				num = Find.Storyteller.intenderPopulation.PopulationIntent;
			}
			else if (def.populationEffect <= IncidentPopulationEffect.Decrease)
			{
				num = -Find.Storyteller.intenderPopulation.PopulationIntent;
			}
			num = Mathf.Max(num, 0.05f);
			return Mathf.Max(0f, def.Worker.AdjustedChance * num);
		}

		public virtual void DebugTablesIncidentChances(IncidentCategory cat)
		{
			IEnumerable<IncidentDef> arg_B6_0 = from d in DefDatabase<IncidentDef>.AllDefs
			where d.category == cat
			orderby this.IncidentChanceAdjustedForPopulation(d) descending
			select d;
			TableDataGetter<IncidentDef>[] expr_41 = new TableDataGetter<IncidentDef>[4];
			expr_41[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			expr_41[1] = new TableDataGetter<IncidentDef>("chance now", (IncidentDef d) => this.IncidentChanceAdjustedForPopulation(d).ToString());
			expr_41[2] = new TableDataGetter<IncidentDef>("usable on visible map", (IncidentDef d) => (Find.VisibleMap != null) ? ((!this.UsableIncidentsInCategory(cat, Find.VisibleMap).Contains(d)) ? string.Empty : "V") : "-");
			expr_41[3] = new TableDataGetter<IncidentDef>("usable on world", (IncidentDef d) => (!this.UsableIncidentsInCategory(cat, Find.World).Contains(d)) ? string.Empty : "W");
			DebugTables.MakeTablesDialog<IncidentDef>(arg_B6_0, expr_41);
		}
	}
}
