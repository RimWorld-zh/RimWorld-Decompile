using System.Collections.Generic;

namespace RimWorld
{
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.TraderCaravanArrival;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
