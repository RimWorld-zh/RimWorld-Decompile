using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E4 RID: 484
	public class ThinkNode_ConditionalAnyAutoJoinableCaravan : ThinkNode_Conditional
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x00056A5C File Offset: 0x00054E5C
		protected override bool Satisfied(Pawn pawn)
		{
			return CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null;
		}
	}
}
