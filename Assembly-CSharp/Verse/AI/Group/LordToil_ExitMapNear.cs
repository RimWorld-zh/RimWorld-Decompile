using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F9 RID: 2553
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x04002488 RID: 9352
		private IntVec3 near;

		// Token: 0x04002489 RID: 9353
		private float radius;

		// Token: 0x0400248A RID: 9354
		private LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x0400248B RID: 9355
		private bool canDig;

		// Token: 0x0600394C RID: 14668 RVA: 0x001E72F8 File Offset: 0x001E56F8
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x001E7328 File Offset: 0x001E5728
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x001E7340 File Offset: 0x001E5740
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x001E7358 File Offset: 0x001E5758
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
	}
}
