using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000186 RID: 390
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		// Token: 0x0400037E RID: 894
		private IntVec3 meetingPoint;

		// Token: 0x06000819 RID: 2073 RVA: 0x0004E67E File Offset: 0x0004CA7E
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0004E690 File Offset: 0x0004CA90
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0004E6B0 File Offset: 0x0004CAB0
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0004E6C8 File Offset: 0x0004CAC8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}
	}
}
