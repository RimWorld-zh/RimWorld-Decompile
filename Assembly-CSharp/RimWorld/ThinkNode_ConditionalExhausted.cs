using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D9 RID: 473
	public class ThinkNode_ConditionalExhausted : ThinkNode_Conditional
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x00056650 File Offset: 0x00054A50
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.Exhausted;
		}
	}
}
