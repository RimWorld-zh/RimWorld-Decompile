using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001EA RID: 490
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x00056C7C File Offset: 0x0005507C
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
