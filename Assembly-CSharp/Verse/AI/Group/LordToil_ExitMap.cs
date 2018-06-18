using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F7 RID: 2551
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x06003942 RID: 14658 RVA: 0x001E6A59 File Offset: 0x001E4E59
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x001E6A88 File Offset: 0x001E4E88
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003944 RID: 14660 RVA: 0x001E6AA0 File Offset: 0x001E4EA0
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x001E6AB8 File Offset: 0x001E4EB8
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x001E6AD8 File Offset: 0x001E4ED8
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
