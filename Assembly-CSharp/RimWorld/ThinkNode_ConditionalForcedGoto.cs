using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D1 RID: 465
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x0005646C File Offset: 0x0005486C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
