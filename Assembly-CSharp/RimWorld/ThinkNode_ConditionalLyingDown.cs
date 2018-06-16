using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DE RID: 478
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x00056810 File Offset: 0x00054C10
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
