using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(base.def.hediff, false);
			ThoughtState result;
			if (firstHediffOfDef == null || firstHediffOfDef.def.stages == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				int stageIndex = Mathf.Min(firstHediffOfDef.CurStageIndex, firstHediffOfDef.def.stages.Count - 1, base.def.stages.Count - 1);
				result = ThoughtState.ActiveAtStage(stageIndex);
			}
			return result;
		}
	}
}
