using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_Disease : StorytellerComp
	{
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (DebugSettings.enableRandomDiseases && target.Tile != -1)
			{
				BiomeDef biome = Find.WorldGrid[target.Tile].biome;
				float mtb2 = biome.diseaseMtbDays;
				mtb2 *= Find.Storyteller.difficulty.diseaseIntervalFactor;
				IncidentDef inc;
				if (Rand.MTBEventOccurs(mtb2, 60000f, 1000f) && (from d in DefDatabase<IncidentDef>.AllDefs
				where d.TargetAllowed(((_003CMakeIntervalIncidents_003Ec__IteratorAA)/*Error near IL_00b2: stateMachine*/).target) && d.category == IncidentCategory.Disease
				select d).TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)((IncidentDef d) => ((_003CMakeIntervalIncidents_003Ec__IteratorAA)/*Error near IL_00c3: stateMachine*/)._003Cbiome_003E__0.CommonalityOfDisease(d)), out inc))
				{
					yield return new FiringIncident(inc, this, this.GenerateParms(inc.category, target));
				}
			}
		}
	}
}
