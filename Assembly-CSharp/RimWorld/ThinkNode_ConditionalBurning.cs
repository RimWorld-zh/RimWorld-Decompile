using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CC RID: 460
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x0600094E RID: 2382 RVA: 0x0005635C File Offset: 0x0005475C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
