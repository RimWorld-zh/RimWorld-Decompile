using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F5 RID: 2549
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x06003940 RID: 14656 RVA: 0x001E6DC5 File Offset: 0x001E51C5
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003941 RID: 14657 RVA: 0x001E6DF4 File Offset: 0x001E51F4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003942 RID: 14658 RVA: 0x001E6E0C File Offset: 0x001E520C
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x001E6E24 File Offset: 0x001E5224
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x001E6E44 File Offset: 0x001E5244
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
