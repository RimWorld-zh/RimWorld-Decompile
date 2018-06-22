using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CE RID: 462
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x06000953 RID: 2387 RVA: 0x000563D4 File Offset: 0x000547D4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
