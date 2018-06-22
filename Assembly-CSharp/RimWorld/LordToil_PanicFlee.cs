using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000198 RID: 408
	public class LordToil_PanicFlee : LordToil
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x00050020 File Offset: 0x0004E420
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00050038 File Offset: 0x0004E438
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00050050 File Offset: 0x0004E450
		public override void Init()
		{
			base.Init();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!this.HasFleeingDuty(pawn) || pawn.mindState.duty.def == DutyDefOf.ExitMapRandom)
				{
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, null, false);
				}
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000500DC File Offset: 0x0004E4DC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!this.HasFleeingDuty(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapRandom);
				}
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00050144 File Offset: 0x0004E544
		private bool HasFleeingDuty(Pawn pawn)
		{
			return pawn.mindState.duty != null && (pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal || pawn.mindState.duty.def == DutyDefOf.Kidnap);
		}
	}
}
