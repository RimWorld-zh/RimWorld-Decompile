using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001EA RID: 490
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x0600098F RID: 2447 RVA: 0x00056C8C File Offset: 0x0005508C
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
