using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)base.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			IncidentDef selectedDef;
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f) && this.UsableIncidentsInCategory(this.Props.category, target).TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)((IncidentDef incDef) => ((_003CMakeIntervalIncidents_003Ec__IteratorA8)/*Error near IL_0066: stateMachine*/)._003C_003Ef__this.IncidentChanceFinal(incDef)), out selectedDef))
			{
				yield return new FiringIncident(selectedDef, this, this.GenerateParms(selectedDef.category, target));
			}
		}
	}
}
