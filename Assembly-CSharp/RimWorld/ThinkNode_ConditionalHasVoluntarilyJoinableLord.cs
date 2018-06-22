using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001C9 RID: 457
	public class ThinkNode_ConditionalHasVoluntarilyJoinableLord : ThinkNode_Conditional
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x000561D0 File Offset: 0x000545D0
		protected override bool Satisfied(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_VoluntarilyJoinable;
		}
	}
}
