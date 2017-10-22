using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_AllyAssistance : StorytellerComp
	{
		private StorytellerCompProperties_AllyAssistance Props
		{
			get
			{
				return (StorytellerCompProperties_AllyAssistance)base.props;
			}
		}

		private float IncidentMTBDays
		{
			get
			{
				return this.Props.baseMtb * StorytellerUtility.AllyIncidentMTBMultiplier();
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float mtb = this.IncidentMTBDays;
			if (!(mtb < 0.0) && Rand.MTBEventOccurs(mtb, 60000f, 1000f))
			{
				Map map = target as Map;
				if (map != null && (int)map.dangerWatcher.DangerRating >= 2)
				{
					IncidentDef incident = null;
					if (this.UsableIncidentsInCategory(IncidentCategory.AllyAssistance, target).TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)((IncidentDef d) => d.baseChance), out incident))
					{
						yield return new FiringIncident(incident, this, this.GenerateParms(incident.category, target));
					}
				}
			}
		}
	}
}
