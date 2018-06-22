using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D5 RID: 469
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		// Token: 0x06000961 RID: 2401 RVA: 0x00056538 File Offset: 0x00054938
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && pawn.needs.food.CurCategory >= HungerCategory.Starving;
		}
	}
}
