using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000185 RID: 389
	public class LordToil_PrepareCaravan_Pause : LordToil
	{
		// Token: 0x06000819 RID: 2073 RVA: 0x0004E640 File Offset: 0x0004CA40
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Pause);
			}
		}
	}
}
