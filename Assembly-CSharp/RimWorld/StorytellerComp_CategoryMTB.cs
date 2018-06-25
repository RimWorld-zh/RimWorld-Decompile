using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000365 RID: 869
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0007FDE4 File Offset: 0x0007E1E4
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0007FE04 File Offset: 0x0007E204
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float mtbNow = this.Props.mtbDays;
			if (this.Props.mtbDaysFactorByDaysPassedCurve != null)
			{
				mtbNow *= this.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (Rand.MTBEventOccurs(mtbNow, 60000f, 1000f))
			{
				IncidentDef selectedDef;
				if (base.UsableIncidentsInCategory(this.Props.category, target).TryRandomElementByWeight((IncidentDef incDef) => base.IncidentChanceFinal(incDef), out selectedDef))
				{
					yield return new FiringIncident(selectedDef, this, this.GenerateParms(selectedDef.category, target));
				}
			}
			yield break;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0007FE38 File Offset: 0x0007E238
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}
