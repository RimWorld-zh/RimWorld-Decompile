using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E0 RID: 480
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x06000978 RID: 2424 RVA: 0x00056904 File Offset: 0x00054D04
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
