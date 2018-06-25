using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000361 RID: 865
	public class StorytellerComp_AllyAssistance : StorytellerComp
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0007F728 File Offset: 0x0007DB28
		private StorytellerCompProperties_AllyAssistance Props
		{
			get
			{
				return (StorytellerCompProperties_AllyAssistance)this.props;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0007F748 File Offset: 0x0007DB48
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map == null || map.dangerWatcher.DangerRating < StoryDanger.High)
			{
				yield break;
			}
			float mtb = this.Props.baseMtbDays * StorytellerUtility.AllyIncidentMTBMultiplier(false);
			if (mtb <= 0f || !Rand.MTBEventOccurs(mtb, 60000f, 1000f))
			{
				yield break;
			}
			IncidentDef incident;
			if (!base.UsableIncidentsInCategory(IncidentCategoryDefOf.AllyAssistance, target).TryRandomElementByWeight((IncidentDef d) => d.baseChance, out incident))
			{
				yield break;
			}
			yield return new FiringIncident(incident, this, this.GenerateParms(incident.category, target));
			yield break;
		}
	}
}
