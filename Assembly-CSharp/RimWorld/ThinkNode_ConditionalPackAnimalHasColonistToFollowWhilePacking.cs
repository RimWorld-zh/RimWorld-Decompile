using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001EA RID: 490
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x06000990 RID: 2448 RVA: 0x00056C90 File Offset: 0x00055090
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
