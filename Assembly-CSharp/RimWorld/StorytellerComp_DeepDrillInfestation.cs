using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000369 RID: 873
	public class StorytellerComp_DeepDrillInfestation : StorytellerComp
	{
		// Token: 0x0400094C RID: 2380
		private static List<Thing> tmpDrills = new List<Thing>();

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x000806A8 File Offset: 0x0007EAA8
		protected StorytellerCompProperties_DeepDrillInfestation Props
		{
			get
			{
				return (StorytellerCompProperties_DeepDrillInfestation)this.props;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x000806C8 File Offset: 0x0007EAC8
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

		// Token: 0x06000F2B RID: 3883 RVA: 0x00080718 File Offset: 0x0007EB18
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
	}
}
