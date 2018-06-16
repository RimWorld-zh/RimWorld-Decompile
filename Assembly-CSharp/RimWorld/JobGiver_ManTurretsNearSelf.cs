using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000D8 RID: 216
	public class JobGiver_ManTurretsNearSelf : JobGiver_ManTurrets
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x000358BC File Offset: 0x00033CBC
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
