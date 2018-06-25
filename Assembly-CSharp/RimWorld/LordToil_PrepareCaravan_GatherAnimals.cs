using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000180 RID: 384
	public class LordToil_PrepareCaravan_GatherAnimals : LordToil
	{
		// Token: 0x04000375 RID: 885
		private IntVec3 meetingPoint;

		// Token: 0x060007F9 RID: 2041 RVA: 0x0004DE36 File Offset: 0x0004C236
		public LordToil_PrepareCaravan_GatherAnimals(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0004DE48 File Offset: 0x0004C248
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0004DE68 File Offset: 0x0004C268
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0004DE80 File Offset: 0x0004C280
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist || pawn.RaceProps.Animal)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherPawns, this.meetingPoint, -1f);
					pawn.mindState.duty.pawnsToGather = PawnsToGather.Animals;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait);
				}
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0004DF30 File Offset: 0x0004C330
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.meetingPoint, "AllAnimalsGathered", (Pawn x) => x.RaceProps.Animal, (Pawn x) => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(x));
			}
		}
	}
}
