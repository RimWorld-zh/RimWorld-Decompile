using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000196 RID: 406
	public class LordToil_LoadAndEnterTransporters : LordToil
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x0004FF05 File Offset: 0x0004E305
		public LordToil_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0004FF1C File Offset: 0x0004E31C
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0004FF34 File Offset: 0x0004E334
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterTransporters);
				pawnDuty.transportersGroup = this.transportersGroup;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x0400038B RID: 907
		private int transportersGroup = -1;
	}
}
