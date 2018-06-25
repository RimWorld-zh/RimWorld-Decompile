using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		public ThoughtWorker_Hediff()
		{
		}

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
