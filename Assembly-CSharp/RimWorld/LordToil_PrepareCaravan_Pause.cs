using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000185 RID: 389
	public class LordToil_PrepareCaravan_Pause : LordToil
	{
		// Token: 0x06000818 RID: 2072 RVA: 0x0004E628 File Offset: 0x0004CA28
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Pause);
			}
		}
	}
}
