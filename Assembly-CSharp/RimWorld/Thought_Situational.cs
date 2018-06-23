using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000537 RID: 1335
	public class Thought_Situational : Thought
	{
		// Token: 0x04000EA5 RID: 3749
		private int curStageIndex = -1;

		// Token: 0x04000EA6 RID: 3750
		protected string reason;

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060018D6 RID: 6358 RVA: 0x00057840 File Offset: 0x00055C40
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060018D7 RID: 6359 RVA: 0x00057864 File Offset: 0x00055C64
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x00057880 File Offset: 0x00055C80
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

		// Token: 0x060018D9 RID: 6361 RVA: 0x000578CC File Offset: 0x00055CCC
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

		// Token: 0x060018DA RID: 6362 RVA: 0x00057924 File Offset: 0x00055D24
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}
	}
}
