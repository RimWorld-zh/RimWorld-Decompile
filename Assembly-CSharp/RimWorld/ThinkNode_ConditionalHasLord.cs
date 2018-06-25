using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001C8 RID: 456
	public class ThinkNode_ConditionalHasLord : ThinkNode_Conditional
	{
		// Token: 0x06000945 RID: 2373 RVA: 0x000561A0 File Offset: 0x000545A0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null;
		}
	}
}
