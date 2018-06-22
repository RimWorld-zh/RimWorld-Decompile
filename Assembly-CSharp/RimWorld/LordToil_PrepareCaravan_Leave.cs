using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000184 RID: 388
	public class LordToil_PrepareCaravan_Leave : LordToil
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x0004E4C2 File Offset: 0x0004C8C2
		public LordToil_PrepareCaravan_Leave(IntVec3 exitSpot)
		{
			this.exitSpot = exitSpot;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0004E4D4 File Offset: 0x0004C8D4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0004E4EC File Offset: 0x0004C8EC
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0004E50C File Offset: 0x0004C90C
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0004E524 File Offset: 0x0004C924
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0004E53C File Offset: 0x0004C93C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.TravelOrWait, this.exitSpot, -1f);
				pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0004E5B8 File Offset: 0x0004C9B8
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.exitSpot, "ReadyToExitMap", (Pawn x) => true, null);
			}
		}

		// Token: 0x0400037B RID: 891
		private IntVec3 exitSpot;
	}
}
