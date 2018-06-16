using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000186 RID: 390
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		// Token: 0x0600081A RID: 2074 RVA: 0x0004E696 File Offset: 0x0004CA96
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0004E6A8 File Offset: 0x0004CAA8
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0004E6C8 File Offset: 0x0004CAC8
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0004E6E0 File Offset: 0x0004CAE0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}

		// Token: 0x0400037D RID: 893
		private IntVec3 meetingPoint;
	}
}
