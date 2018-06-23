using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E1 RID: 481
	public class ThinkNode_ConditionalRandom : ThinkNode_Conditional
	{
		// Token: 0x040003E1 RID: 993
		public float chance = 0.5f;

		// Token: 0x0600097B RID: 2427 RVA: 0x00056968 File Offset: 0x00054D68
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRandom thinkNode_ConditionalRandom = (ThinkNode_ConditionalRandom)base.DeepCopy(resolve);
			thinkNode_ConditionalRandom.chance = this.chance;
			return thinkNode_ConditionalRandom;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00056998 File Offset: 0x00054D98
		protected override bool Satisfied(Pawn pawn)
		{
			return Rand.Value < this.chance;
		}
	}
}
