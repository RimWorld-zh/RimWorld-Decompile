using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200018C RID: 396
	public class LordToil_DefendBase : LordToil
	{
		// Token: 0x06000833 RID: 2099 RVA: 0x0004EF66 File Offset: 0x0004D366
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0004EF78 File Offset: 0x0004D378
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0004EF94 File Offset: 0x0004D394
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}

		// Token: 0x04000383 RID: 899
		public IntVec3 baseCenter;
	}
}
