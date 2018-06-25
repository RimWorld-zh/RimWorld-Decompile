using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000539 RID: 1337
	public class Thought_Situational : Thought
	{
		// Token: 0x04000EA9 RID: 3753
		private int curStageIndex = -1;

		// Token: 0x04000EAA RID: 3754
		protected string reason;

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060018D9 RID: 6361 RVA: 0x0005783C File Offset: 0x00055C3C
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x00057860 File Offset: 0x00055C60
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x0005787C File Offset: 0x00055C7C
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

		// Token: 0x060018DC RID: 6364 RVA: 0x000578C8 File Offset: 0x00055CC8
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

		// Token: 0x060018DD RID: 6365 RVA: 0x00057920 File Offset: 0x00055D20
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}
	}
}
