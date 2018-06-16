using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001C8 RID: 456
	public class ThinkNode_ConditionalHasLord : ThinkNode_Conditional
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x00056190 File Offset: 0x00054590
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null;
		}
	}
}
