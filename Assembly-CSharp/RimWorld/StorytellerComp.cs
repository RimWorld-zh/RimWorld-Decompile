using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000360 RID: 864
	public abstract class StorytellerComp
	{
		// Token: 0x0400093F RID: 2367
		public StorytellerCompProperties props;

		// Token: 0x06000F06 RID: 3846
		public abstract IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target);

		// Token: 0x06000F07 RID: 3847 RVA: 0x0007F004 File Offset: 0x0007D404
		public virtual IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(incCat, target);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0007F020 File Offset: 0x0007D420
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IIncidentTarget target)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => this.GenerateParms(cat, target));
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0007F068 File Offset: 0x0007D468
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IncidentParms parms)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => parms);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0007F0A0 File Offset: 0x0007D4A0
		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, Func<IncidentDef, IncidentParms> parmsGetter)
		{
			return from x in DefDatabase<IncidentDef>.AllDefsListForReading
			where x.category == cat && x.Worker.CanFireNow(parmsGetter(x))
			select x;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0007F0E0 File Offset: 0x0007D4E0
		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			float result;
			if (def.chanceFactorByPopulationCurve == null)
			{
				result = 1f;
			}
			else
			{
				int num = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
				result = def.chanceFactorByPopulationCurve.Evaluate((float)num);
			}
			return result;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0007F124 File Offset: 0x0007D524
		protected float IncidentChanceFactor_PopulationIntent(IncidentDef def)
		{
			IncidentPopulationEffect populationEffect = def.populationEffect;
			float result;
			if (populationEffect != IncidentPopulationEffect.None)
			{
				if (populationEffect != IncidentPopulationEffect.Increase)
				{
					throw new NotImplementedException();
				}
				result = Mathf.Max(Find.Storyteller.intenderPopulation.PopulationIntent, this.props.minIncChancePopulationIntentFactor);
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0007F184 File Offset: 0x0007D584
		protected float IncidentChanceFinal(IncidentDef def)
		{
			float num = def.Worker.AdjustedChance;
			num *= this.IncidentChanceFactor_CurrentPopulation(def);
			num *= this.IncidentChanceFactor_PopulationIntent(def);
			return Mathf.Max(0f, num);
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0007F1C4 File Offset: 0x0007D5C4
		public override string ToString()
		{
			string text = base.GetType().Name;
			string text2 = typeof(StorytellerComp).Name + "_";
			if (text.StartsWith(text2))
			{
				text = text.Substring(text2.Length);
			}
			if (!this.props.allowedTargetTypes.NullOrEmpty<IncidentTargetTypeDef>())
			{
				text = text + " (" + (from x in this.props.allowedTargetTypes
				select x.ToString()).ToCommaList(false) + ")";
			}
			return text;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0007F274 File Offset: 0x0007D674
		public virtual void DebugTablesIncidentChances(IncidentCategoryDef cat)
		{
			IEnumerable<IncidentDef> dataSources = from d in DefDatabase<IncidentDef>.AllDefs
			where d.category == cat
			orderby this.IncidentChanceFinal(d) descending
			select d;
			TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[10];
			array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			array[1] = new TableDataGetter<IncidentDef>("baseChance", (IncidentDef d) => d.baseChance.ToString());
			array[2] = new TableDataGetter<IncidentDef>("AdjustedChance", (IncidentDef d) => d.Worker.AdjustedChance.ToString());
			array[3] = new TableDataGetter<IncidentDef>("Factor-PopCurrent", (IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString());
			array[4] = new TableDataGetter<IncidentDef>("Factor-PopIntent", (IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString());
			array[5] = new TableDataGetter<IncidentDef>("final chance", (IncidentDef d) => this.IncidentChanceFinal(d).ToString());
			array[6] = new TableDataGetter<IncidentDef>("vismap-usable", (IncidentDef d) => (Find.CurrentMap != null) ? ((!this.UsableIncidentsInCategory(cat, Find.CurrentMap).Contains(d)) ? "" : "V") : "-");
			array[7] = new TableDataGetter<IncidentDef>("world-usable", (IncidentDef d) => (!this.UsableIncidentsInCategory(cat, Find.World).Contains(d)) ? "" : "W");
			array[8] = new TableDataGetter<IncidentDef>("pop-current", (IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>().ToString());
			array[9] = new TableDataGetter<IncidentDef>("pop-intent", (IncidentDef d) => Find.Storyteller.intenderPopulation.PopulationIntent.ToString("F3"));
			DebugTables.MakeTablesDialog<IncidentDef>(dataSources, array);
		}
	}
}
