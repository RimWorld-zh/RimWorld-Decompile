using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E0 RID: 480
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x06000979 RID: 2425 RVA: 0x00056908 File Offset: 0x00054D08
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
