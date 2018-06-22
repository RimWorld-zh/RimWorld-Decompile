using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F3 RID: 2547
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x0600393C RID: 14652 RVA: 0x001E6C99 File Offset: 0x001E5099
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x001E6CC8 File Offset: 0x001E50C8
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600393E RID: 14654 RVA: 0x001E6CE0 File Offset: 0x001E50E0
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600393F RID: 14655 RVA: 0x001E6CF8 File Offset: 0x001E50F8
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x001E6D18 File Offset: 0x001E5118
		public override void UpdateAllDuties()
		{
			LordToilData_ExitMap data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.ExitMapBest);
				pawnDuty.locomotion = data.locomotion;
				pawnDuty.canDig = data.canDig;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}
	}
}
