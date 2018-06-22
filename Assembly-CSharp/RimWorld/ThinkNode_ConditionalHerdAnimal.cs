using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CF RID: 463
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x00056400 File Offset: 0x00054800
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
