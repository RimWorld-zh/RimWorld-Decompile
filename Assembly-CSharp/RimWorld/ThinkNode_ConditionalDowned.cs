using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001B9 RID: 441
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x00055DD4 File Offset: 0x000541D4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
