using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CE RID: 462
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x000563C0 File Offset: 0x000547C0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
