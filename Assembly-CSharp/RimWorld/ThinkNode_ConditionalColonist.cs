using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BF RID: 447
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x06000935 RID: 2357 RVA: 0x00055F28 File Offset: 0x00054328
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
