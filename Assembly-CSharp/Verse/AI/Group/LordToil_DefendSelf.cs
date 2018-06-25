using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F4 RID: 2548
	public class LordToil_DefendSelf : LordToil
	{
		// Token: 0x0600393D RID: 14653 RVA: 0x001E7034 File Offset: 0x001E5434
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, this.lord.ownedPawns[i].Position, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
