using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000216 RID: 534
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x060009F8 RID: 2552 RVA: 0x00058F34 File Offset: 0x00057334
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			ThoughtState result;
			if (firstHediffOfDef == null || firstHediffOfDef.def.stages == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				int stageIndex = Mathf.Min(new int[]
				{
					firstHediffOfDef.CurStageIndex,
					firstHediffOfDef.def.stages.Count - 1,
					this.def.stages.Count - 1
				});
				result = ThoughtState.ActiveAtStage(stageIndex);
			}
			return result;
		}
	}
}
