using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E1 RID: 481
	public class ThinkNode_ConditionalRandom : ThinkNode_Conditional
	{
		// Token: 0x0600097D RID: 2429 RVA: 0x00056954 File Offset: 0x00054D54
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRandom thinkNode_ConditionalRandom = (ThinkNode_ConditionalRandom)base.DeepCopy(resolve);
			thinkNode_ConditionalRandom.chance = this.chance;
			return thinkNode_ConditionalRandom;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00056984 File Offset: 0x00054D84
		protected override bool Satisfied(Pawn pawn)
		{
			return Rand.Value < this.chance;
		}

		// Token: 0x040003E3 RID: 995
		public float chance = 0.5f;
	}
}
