using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E0 RID: 480
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x0600097B RID: 2427 RVA: 0x000568F4 File Offset: 0x00054CF4
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
