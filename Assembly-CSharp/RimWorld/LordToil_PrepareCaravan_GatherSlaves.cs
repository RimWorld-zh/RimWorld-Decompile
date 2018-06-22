using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000183 RID: 387
	public class LordToil_PrepareCaravan_GatherSlaves : LordToil
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x0004E302 File Offset: 0x0004C702
		public LordToil_PrepareCaravan_GatherSlaves(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0004E314 File Offset: 0x0004C714
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0004E334 File Offset: 0x0004C734
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0004E34C File Offset: 0x0004C74C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.RaceProps.Animal)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherPawns, this.meetingPoint, -1f);
					pawn.mindState.duty.pawnsToGather = PawnsToGather.Slaves;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0004E404 File Offset: 0x0004C804
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.meetingPoint, "AllSlavesGathered", (Pawn x) => !x.IsColonist && !x.RaceProps.Animal, (Pawn x) => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(x));
			}
		}

		// Token: 0x04000378 RID: 888
		private IntVec3 meetingPoint;
	}
}
