using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000B4 RID: 180
	public class JobGiver_AIDefendEscortee : JobGiver_AIDefendPawn
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x00032AC4 File Offset: 0x00030EC4
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return ((Thing)pawn.mindState.duty.focus) as Pawn;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00032AF4 File Offset: 0x00030EF4
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
