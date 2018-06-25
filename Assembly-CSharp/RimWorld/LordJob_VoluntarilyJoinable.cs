using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200017E RID: 382
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x0004C3E8 File Offset: 0x0004A7E8
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}
	}
}
