using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000363 RID: 867
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0007FC84 File Offset: 0x0007E084
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0007FCA4 File Offset: 0x0007E0A4
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

		// Token: 0x06000F1E RID: 3870 RVA: 0x0007FCD8 File Offset: 0x0007E0D8
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}
