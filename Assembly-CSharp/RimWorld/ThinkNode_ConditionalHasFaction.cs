using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CE RID: 462
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x06000952 RID: 2386 RVA: 0x000563D0 File Offset: 0x000547D0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
