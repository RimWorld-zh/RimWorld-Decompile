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
			_003CMakeIntervalIncidents_003Ec__Iterator0 _003CMakeIntervalIncidents_003Ec__Iterator = (_003CMakeIntervalIncidents_003Ec__Iterator0)/*Error near IL_0032: stateMachine*/;
			if (DebugSettings.enableRandomDiseases && target.Tile != -1)
			{
				BiomeDef biome = Find.WorldGrid[target.Tile].biome;
				float mtb2 = biome.diseaseMtbDays;
				mtb2 *= Find.Storyteller.difficulty.diseaseIntervalFactor;
				if (!Rand.MTBEventOccurs(mtb2, 60000f, 1000f))
					yield break;
				IncidentDef inc;
				if (!(from d in DefDatabase<IncidentDef>.AllDefs
				where d.Worker.CanFireNow(target) && d.category == IncidentCategory.Disease
				select d).TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)((IncidentDef d) => biome.CommonalityOfDisease(d)), out inc))
					yield break;
				yield return new FiringIncident(inc, this, this.GenerateParms(inc.category, target));
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
