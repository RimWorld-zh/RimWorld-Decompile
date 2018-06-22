using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020009F5 RID: 2549
	public class LordToil_ExitMapTraderFighting : LordToil
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003944 RID: 14660 RVA: 0x001E6DD4 File Offset: 0x001E51D4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x001E6DEC File Offset: 0x001E51EC
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x001E6E04 File Offset: 0x001E5204
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = pawn.GetTraderCaravanRole();
				if (traderCaravanRole == TraderCaravanRole.Carrier || traderCaravanRole == TraderCaravanRole.Chattel)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
					pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				}
			}
		}
	}
}
