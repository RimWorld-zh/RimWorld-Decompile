using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000B5 RID: 181
	public class JobGiver_AIDefendSelf : JobGiver_AIDefendPawn
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00032B00 File Offset: 0x00030F00
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00032B18 File Offset: 0x00030F18
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
