using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000376 RID: 886
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x00081880 File Offset: 0x0007FC80
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x000818A0 File Offset: 0x0007FCA0
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000818D4 File Offset: 0x0007FCD4
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
