using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200019C RID: 412
	public class LordToil_Sleep : LordToil
	{
		// Token: 0x0600088F RID: 2191 RVA: 0x000517C0 File Offset: 0x0004FBC0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.SleepForever);
			}
		}
	}
}
