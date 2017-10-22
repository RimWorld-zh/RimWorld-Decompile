using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)base.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (this.Props.incident.TargetAllowed(target) && Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				yield return new FiringIncident(this.Props.incident, this, this.GenerateParms(this.Props.incident.category, target));
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
