using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000D7 RID: 215
	public class JobGiver_ManTurretsNearPoint : JobGiver_ManTurrets
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x00035878 File Offset: 0x00033C78
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.GetLord().CurLordToil.FlagLoc;
		}
	}
}
