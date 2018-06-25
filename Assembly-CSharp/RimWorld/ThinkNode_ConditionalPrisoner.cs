using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BB RID: 443
	public class ThinkNode_ConditionalPrisoner : ThinkNode_Conditional
	{
		// Token: 0x0600092A RID: 2346 RVA: 0x00055E60 File Offset: 0x00054260
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
