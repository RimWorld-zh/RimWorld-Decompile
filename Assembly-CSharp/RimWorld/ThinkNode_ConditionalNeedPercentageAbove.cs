using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D8 RID: 472
	public class ThinkNode_ConditionalNeedPercentageAbove : ThinkNode_Conditional
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x000565DC File Offset: 0x000549DC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalNeedPercentageAbove thinkNode_ConditionalNeedPercentageAbove = (ThinkNode_ConditionalNeedPercentageAbove)base.DeepCopy(resolve);
			thinkNode_ConditionalNeedPercentageAbove.need = this.need;
			thinkNode_ConditionalNeedPercentageAbove.threshold = this.threshold;
			return thinkNode_ConditionalNeedPercentageAbove;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00056618 File Offset: 0x00054A18
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.TryGetNeed(this.need).CurLevelPercentage > this.threshold;
		}

		// Token: 0x040003DE RID: 990
		private NeedDef need = null;

		// Token: 0x040003DF RID: 991
		private float threshold = 0f;
	}
}
