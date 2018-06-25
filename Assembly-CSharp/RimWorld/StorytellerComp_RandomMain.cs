using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000371 RID: 881
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00081160 File Offset: 0x0007F560
		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00081180 File Offset: 0x0007F580
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
				IEnumerable<IncidentDef> options;
				for (;;)
				{
					if (triedCategories.Count >= this.Props.categoryWeights.Count)
					{
						break;
					}
					IncidentCategoryDef incidentCategoryDef = this.DecideCategory(target, triedCategories);
					triedCategories.Add(incidentCategoryDef);
					IncidentParms parms = this.GenerateParms(incidentCategoryDef, target);
					options = from d in base.UsableIncidentsInCategory(incidentCategoryDef, target)
					where !d.NeedsParmsPoints || d.minThreatPoints <= parms.points
					select d;
					if (options.Any<IncidentDef>())
					{
						goto Block_3;
					}
				}
				yield break;
				Block_3:
				IncidentDef incDef;
				if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
				{
					yield return new FiringIncident(incDef, this, this.GenerateParms(incDef.category, target));
				}
			}
			yield break;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x000811B4 File Offset: 0x0007F5B4
		private IncidentCategoryDef DecideCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig))
			{
				int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
				if ((float)num > 60000f * this.Props.maxThreatBigIntervalDays)
				{
					return IncidentCategoryDefOf.ThreatBig;
				}
			}
			return (from cw in this.Props.categoryWeights
			where !skipCategories.Contains(cw.category)
			select cw).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0008126C File Offset: 0x0007F66C
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
			incidentParms.points *= Rand.Range(0.5f, 1.5f);
			return incidentParms;
		}
	}
}
