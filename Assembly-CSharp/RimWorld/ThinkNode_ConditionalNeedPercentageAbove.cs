using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D8 RID: 472
	public class ThinkNode_ConditionalNeedPercentageAbove : ThinkNode_Conditional
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x000565C8 File Offset: 0x000549C8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalNeedPercentageAbove thinkNode_ConditionalNeedPercentageAbove = (ThinkNode_ConditionalNeedPercentageAbove)base.DeepCopy(resolve);
			thinkNode_ConditionalNeedPercentageAbove.need = this.need;
			thinkNode_ConditionalNeedPercentageAbove.threshold = this.threshold;
			return thinkNode_ConditionalNeedPercentageAbove;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00056604 File Offset: 0x00054A04
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.TryGetNeed(this.need).CurLevelPercentage > this.threshold;
		}

		// Token: 0x040003E0 RID: 992
		private NeedDef need = null;

		// Token: 0x040003E1 RID: 993
		private float threshold = 0f;
	}
}
