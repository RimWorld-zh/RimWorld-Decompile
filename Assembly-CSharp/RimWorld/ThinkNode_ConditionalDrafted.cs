using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CA RID: 458
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x000561F8 File Offset: 0x000545F8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
