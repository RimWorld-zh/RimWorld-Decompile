using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CA RID: 458
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x06000949 RID: 2377 RVA: 0x00056208 File Offset: 0x00054608
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
