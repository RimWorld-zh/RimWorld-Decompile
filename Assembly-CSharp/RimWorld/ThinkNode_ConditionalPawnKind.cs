using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E8 RID: 488
	public class ThinkNode_ConditionalPawnKind : ThinkNode_Conditional
	{
		// Token: 0x0600098B RID: 2443 RVA: 0x00056BF8 File Offset: 0x00054FF8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalPawnKind thinkNode_ConditionalPawnKind = (ThinkNode_ConditionalPawnKind)base.DeepCopy(resolve);
			thinkNode_ConditionalPawnKind.pawnKind = this.pawnKind;
			return thinkNode_ConditionalPawnKind;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00056C28 File Offset: 0x00055028
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.kindDef == this.pawnKind;
		}

		// Token: 0x040003E3 RID: 995
		public PawnKindDef pawnKind;
	}
}
