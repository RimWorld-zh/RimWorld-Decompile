using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200036B RID: 875
	public class StorytellerComp_Disease : StorytellerComp
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x00080984 File Offset: 0x0007ED84
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x000809A4 File Offset: 0x0007EDA4
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!DebugSettings.enableRandomDiseases)
			{
				yield break;
			}
			if (target.Tile == -1)
			{
				yield break;
			}
			BiomeDef biome = Find.WorldGrid[target.Tile].biome;
			float mtb = biome.diseaseMtbDays;
			mtb *= Find.Storyteller.difficulty.diseaseIntervalFactor;
			if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
			{
				IncidentDef inc;
				if (base.UsableIncidentsInCategory(this.Props.incidentCategory, target).TryRandomElementByWeight((IncidentDef d) => biome.CommonalityOfDisease(d), out inc))
				{
					yield return new FiringIncident(inc, this, this.GenerateParms(inc.category, target));
				}
			}
			yield break;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x000809D8 File Offset: 0x0007EDD8
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incidentCategory;
		}
	}
}
