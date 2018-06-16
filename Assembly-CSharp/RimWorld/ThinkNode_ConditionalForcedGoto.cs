using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D1 RID: 465
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x0005645C File Offset: 0x0005485C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
