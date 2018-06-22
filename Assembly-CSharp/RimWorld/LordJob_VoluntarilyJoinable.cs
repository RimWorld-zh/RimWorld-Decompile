using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200017E RID: 382
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x060007F9 RID: 2041 RVA: 0x0004C3EC File Offset: 0x0004A7EC
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}
	}
}
