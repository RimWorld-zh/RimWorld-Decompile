using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F6 RID: 2550
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x06003947 RID: 14663 RVA: 0x001E6EA0 File Offset: 0x001E52A0
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x001E6ED0 File Offset: 0x001E52D0
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x001E6EE8 File Offset: 0x001E52E8
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x001E6F00 File Offset: 0x001E5300
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

		// Token: 0x04002477 RID: 9335
		private IntVec3 near;

		// Token: 0x04002478 RID: 9336
		private float radius;

		// Token: 0x04002479 RID: 9337
		private LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x0400247A RID: 9338
		private bool canDig;
	}
}
