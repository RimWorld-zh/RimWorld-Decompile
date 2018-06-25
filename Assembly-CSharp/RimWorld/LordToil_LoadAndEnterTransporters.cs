using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000196 RID: 406
	public class LordToil_LoadAndEnterTransporters : LordToil
	{
		// Token: 0x0400038C RID: 908
		private int transportersGroup = -1;

		// Token: 0x06000862 RID: 2146 RVA: 0x0004FF01 File Offset: 0x0004E301
		public LordToil_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0004FF18 File Offset: 0x0004E318
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0004FF30 File Offset: 0x0004E330
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterTransporters);
				pawnDuty.transportersGroup = this.transportersGroup;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}
	}
}
