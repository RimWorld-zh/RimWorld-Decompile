using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BF RID: 447
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x06000932 RID: 2354 RVA: 0x00055F38 File Offset: 0x00054338
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
