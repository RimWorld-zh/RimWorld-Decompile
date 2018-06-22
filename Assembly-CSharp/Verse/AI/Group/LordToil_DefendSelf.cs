using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F1 RID: 2545
	public class LordToil_DefendSelf : LordToil
	{
		// Token: 0x06003938 RID: 14648 RVA: 0x001E6BDC File Offset: 0x001E4FDC
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
