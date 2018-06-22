using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CA RID: 458
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x0600094A RID: 2378 RVA: 0x0005620C File Offset: 0x0005460C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
