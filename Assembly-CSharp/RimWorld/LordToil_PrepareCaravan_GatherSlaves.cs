using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000183 RID: 387
	public class LordToil_PrepareCaravan_GatherSlaves : LordToil
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x0004E316 File Offset: 0x0004C716
		public LordToil_PrepareCaravan_GatherSlaves(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0004E328 File Offset: 0x0004C728
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0004E348 File Offset: 0x0004C748
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0004E360 File Offset: 0x0004C760
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

		// Token: 0x0600080D RID: 2061 RVA: 0x0004E418 File Offset: 0x0004C818
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
