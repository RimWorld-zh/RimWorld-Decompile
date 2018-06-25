using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200036B RID: 875
	public class StorytellerComp_Disease : StorytellerComp
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x00080994 File Offset: 0x0007ED94
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x000809B4 File Offset: 0x0007EDB4
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

		// Token: 0x06000F30 RID: 3888 RVA: 0x000809E8 File Offset: 0x0007EDE8
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incidentCategory;
		}
	}
}
