using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E8 RID: 488
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x040003E4 RID: 996
		public PawnKindDef pawnKind;

		// Token: 0x0600098A RID: 2442 RVA: 0x00056BF4 File Offset: 0x00054FF4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00056C24 File Offset: 0x00055024
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}
	}
}
