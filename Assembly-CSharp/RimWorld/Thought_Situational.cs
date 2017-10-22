using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Thought_Situational : Thought
	{
		private int curStageIndex = -1;

		protected string reason;

		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		public override string LabelCap
		{
			get
			{
				return this.reason.NullOrEmpty() ? base.LabelCap : string.Format(base.CurStage.label, this.reason).CapitalizeFirst();
			}
		}

		public void RecalculateState()
		{
			ThoughtState thoughtState = this.CurrentStateInternal();
			if (thoughtState.Active)
			{
				this.curStageIndex = Mathf.Min(thoughtState.StageIndex, base.def.stages.Count - 1);
				this.reason = thoughtState.Reason;
			}
			else
			{
				this.curStageIndex = -1;
			}
		}

		protected virtual ThoughtState CurrentStateInternal()
		{
			return base.def.Worker.CurrentState(base.pawn);
		}
	}
}
