using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000197 RID: 407
	public class LordToil_ManClosestTurrets : LordToil
	{
		// Token: 0x06000866 RID: 2150 RVA: 0x0004FF9C File Offset: 0x0004E39C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ManClosestTurret, this.lord.ownedPawns[i].Position, -1f);
			}
		}
	}
}
