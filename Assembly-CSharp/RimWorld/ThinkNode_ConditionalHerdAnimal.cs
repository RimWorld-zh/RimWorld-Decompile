using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CF RID: 463
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		// Token: 0x06000957 RID: 2391 RVA: 0x000563EC File Offset: 0x000547EC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
