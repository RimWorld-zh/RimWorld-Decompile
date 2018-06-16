using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000369 RID: 873
	public class StorytellerComp_Disease : StorytellerComp
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00080648 File Offset: 0x0007EA48
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00080668 File Offset: 0x0007EA68
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

		// Token: 0x06000F2D RID: 3885 RVA: 0x0008069C File Offset: 0x0007EA9C
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incidentCategory;
		}
	}
}
