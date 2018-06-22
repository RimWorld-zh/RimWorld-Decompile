using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D1 RID: 465
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x00056470 File Offset: 0x00054870
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
