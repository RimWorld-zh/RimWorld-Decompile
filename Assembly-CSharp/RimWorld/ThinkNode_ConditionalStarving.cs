using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D5 RID: 469
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		// Token: 0x06000960 RID: 2400 RVA: 0x00056534 File Offset: 0x00054934
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && pawn.needs.food.CurCategory >= HungerCategory.Starving;
		}
	}
}
