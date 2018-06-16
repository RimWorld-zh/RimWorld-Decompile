using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200036F RID: 879
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x00080E24 File Offset: 0x0007F224
		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00080E44 File Offset: 0x0007F244
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

		// Token: 0x06000F3C RID: 3900 RVA: 0x00080E78 File Offset: 0x0007F278
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

		// Token: 0x06000F3D RID: 3901 RVA: 0x00080F30 File Offset: 0x0007F330
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
			incidentParms.points *= Rand.Range(0.5f, 1.5f);
			return incidentParms;
		}
	}
}
