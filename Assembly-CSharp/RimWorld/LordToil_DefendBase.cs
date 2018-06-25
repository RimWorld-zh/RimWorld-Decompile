using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200018C RID: 396
	public class LordToil_DefendBase : LordToil
	{
		// Token: 0x04000384 RID: 900
		public IntVec3 baseCenter;

		// Token: 0x06000832 RID: 2098 RVA: 0x0004EF4E File Offset: 0x0004D34E
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0004EF60 File Offset: 0x0004D360
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0004EF7C File Offset: 0x0004D37C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}
	}
}
