using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000367 RID: 871
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00080558 File Offset: 0x0007E958
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00080578 File Offset: 0x0007E978
		private float DeepDrillInfestationMTBDaysPerDrill
		{
			get
			{
				DifficultyDef difficulty = Find.Storyteller.difficulty;
				float result;
				if (difficulty.deepDrillInfestationChanceFactor <= 0f)
				{
					result = -1f;
				}
				else
				{
					result = this.Props.baseMtbDaysPerDrill / difficulty.deepDrillInfestationChanceFactor;
				}
				return result;
			}
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x000805C8 File Offset: 0x0007E9C8
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = (Map)target;
			StorytellerComp_DeepDrillInfestation.tmpDrills.Clear();
			DeepDrillInfestationIncidentUtility.GetUsableDeepDrills(map, StorytellerComp_DeepDrillInfestation.tmpDrills);
			if (!StorytellerComp_DeepDrillInfestation.tmpDrills.Any<Thing>())
			{
				yield break;
			}
			float mtb = this.DeepDrillInfestationMTBDaysPerDrill;
			for (int i = 0; i < StorytellerComp_DeepDrillInfestation.tmpDrills.Count; i++)
			{
				if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
				{
					IncidentDef def;
					if (base.UsableIncidentsInCategory(IncidentCategoryDefOf.DeepDrillInfestation, target).TryRandomElement(out def))
					{
						IncidentParms parms = this.GenerateParms(def.category, target);
						yield return new FiringIncident(def, this, parms);
					}
				}
			}
			yield break;
		}

		// Token: 0x0400094C RID: 2380
		private static List<Thing> tmpDrills = new List<Thing>();
	}
}
