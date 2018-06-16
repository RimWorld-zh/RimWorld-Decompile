using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E8 RID: 488
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x00056BE4 File Offset: 0x00054FE4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00056C14 File Offset: 0x00055014
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}

		// Token: 0x040003E5 RID: 997
		public PawnKindDef pawnKind;
	}
}
