using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000216 RID: 534
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x060009FA RID: 2554 RVA: 0x00058EF0 File Offset: 0x000572F0
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
