using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009FA RID: 2554
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x0600394B RID: 14667 RVA: 0x001E6B8C File Offset: 0x001E4F8C
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x001E6BBC File Offset: 0x001E4FBC
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x001E6BD4 File Offset: 0x001E4FD4
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x001E6BEC File Offset: 0x001E4FEC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.ExitMapNearDutyTarget, this.near, this.radius);
				pawnDuty.locomotion = this.locomotion;
				pawnDuty.canDig = this.canDig;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x0400247C RID: 9340
		private IntVec3 near;

		// Token: 0x0400247D RID: 9341
		private float radius;

		// Token: 0x0400247E RID: 9342
		private LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x0400247F RID: 9343
		private bool canDig;
	}
}
