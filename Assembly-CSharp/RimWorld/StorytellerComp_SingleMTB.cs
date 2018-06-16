using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000374 RID: 884
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x00081534 File Offset: 0x0007F934
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00081554 File Offset: 0x0007F954
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

		// Token: 0x06000F4A RID: 3914 RVA: 0x00081588 File Offset: 0x0007F988
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
