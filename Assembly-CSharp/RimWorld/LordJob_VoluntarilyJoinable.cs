using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200017E RID: 382
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x060007F9 RID: 2041 RVA: 0x0004C400 File Offset: 0x0004A800
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}
	}
}
