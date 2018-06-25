using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000182 RID: 386
	public class LordToil_PrepareCaravan_GatherItems : LordToil
	{
		// Token: 0x04000378 RID: 888
		private IntVec3 meetingPoint;

		// Token: 0x06000803 RID: 2051 RVA: 0x0004E0DB File Offset: 0x0004C4DB
		public LordToil_PrepareCaravan_GatherItems(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0004E0EC File Offset: 0x0004C4EC
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0004E10C File Offset: 0x0004C50C
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0004E124 File Offset: 0x0004C524
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherItems);
				}
				else if (pawn.RaceProps.Animal)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait);
				}
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0004E1DC File Offset: 0x0004C5DC
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (Find.TickManager.TicksGame % 120 == 0)
			{
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (pawn.IsColonist && pawn.mindState.lastJobTag != JobTag.WaitingForOthersToFinishGatheringItems)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					List<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
					for (int j = 0; j < allPawnsSpawned.Count; j++)
					{
						if (allPawnsSpawned[j].CurJob != null && allPawnsSpawned[j].jobs.curDriver is JobDriver_PrepareCaravan_GatherItems && allPawnsSpawned[j].CurJob.lord == this.lord)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("AllItemsGathered");
				}
			}
		}
	}
}
