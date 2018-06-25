using System;
using Verse;

namespace RimWorld
{
	public class Thought_Situational : Thought
	{
		private int curStageIndex = -1;

		protected string reason;

		public Thought_Situational()
		{
		}

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
				string result;
				if (!this.reason.NullOrEmpty())
				{
					result = string.Format(base.CurStage.label, this.reason).CapitalizeFirst();
				}
				else
				{
					result = base.LabelCap;
				}
				return result;
			}
		}

		public void RecalculateState()
		{
			ThoughtState thoughtState = this.CurrentStateInternal();
			if (thoughtState.ActiveFor(this.def))
			{
				this.curStageIndex = thoughtState.StageIndexFor(this.def);
				this.reason = thoughtState.Reason;
			}
			else
			{
				this.curStageIndex = -1;
			}
		}

		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}
	}
}
