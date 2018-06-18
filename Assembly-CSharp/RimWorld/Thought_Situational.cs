using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200053B RID: 1339
	public class Thought_Situational : Thought
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x000577FC File Offset: 0x00055BFC
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x00057820 File Offset: 0x00055C20
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060018E1 RID: 6369 RVA: 0x0005783C File Offset: 0x00055C3C
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

		// Token: 0x060018E2 RID: 6370 RVA: 0x00057888 File Offset: 0x00055C88
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

		// Token: 0x060018E3 RID: 6371 RVA: 0x000578E0 File Offset: 0x00055CE0
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}

		// Token: 0x04000EA8 RID: 3752
		private int curStageIndex = -1;

		// Token: 0x04000EA9 RID: 3753
		protected string reason;
	}
}
