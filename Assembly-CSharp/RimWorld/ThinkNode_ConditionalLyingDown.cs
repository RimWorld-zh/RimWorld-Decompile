using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DE RID: 478
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06000974 RID: 2420 RVA: 0x00056820 File Offset: 0x00054C20
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
