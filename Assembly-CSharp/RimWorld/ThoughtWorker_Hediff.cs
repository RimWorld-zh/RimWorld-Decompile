using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000218 RID: 536
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x000590B4 File Offset: 0x000574B4
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
