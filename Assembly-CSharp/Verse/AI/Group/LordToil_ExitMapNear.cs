using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F8 RID: 2552
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x04002478 RID: 9336
		private IntVec3 near;

		// Token: 0x04002479 RID: 9337
		private float radius;

		// Token: 0x0400247A RID: 9338
		private LocomotionUrgency locomotion = LocomotionUrgency.None;

		// Token: 0x0400247B RID: 9339
		private bool canDig;

		// Token: 0x0600394B RID: 14667 RVA: 0x001E6FCC File Offset: 0x001E53CC
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x001E6FFC File Offset: 0x001E53FC
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x001E7014 File Offset: 0x001E5414
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x001E702C File Offset: 0x001E542C
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
