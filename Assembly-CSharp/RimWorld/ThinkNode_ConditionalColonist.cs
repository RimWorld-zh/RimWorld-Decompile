using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BF RID: 447
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x06000933 RID: 2355 RVA: 0x00055F3C File Offset: 0x0005433C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
