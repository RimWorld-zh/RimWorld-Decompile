using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A0 RID: 416
	public class LordToil_TakeWoundedGuest : LordToil
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x00051A1C File Offset: 0x0004FE1C
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00051A34 File Offset: 0x0004FE34
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00051A4C File Offset: 0x0004FE4C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.TakeWoundedGuest);
			}
		}
	}
}
