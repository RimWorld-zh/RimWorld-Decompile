using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CD RID: 1485
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x04001156 RID: 4438
		private Settlement settlement;

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000F7AC7 File Offset: 0x000F5EC7
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000F7AD0 File Offset: 0x000F5ED0
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x000F7AE0 File Offset: 0x000F5EE0
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001CD8 RID: 7384 RVA: 0x000F7B00 File Offset: 0x000F5F00
		public override string ReportString
		{
			get
			{
				return "CaravanOfferingGifts".Translate(new object[]
				{
					this.settlement.Label
				});
			}
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000F7B34 File Offset: 0x000F5F34
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this.settlement);
			}
			return result;
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000F7B98 File Offset: 0x000F5F98
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000F7BCF File Offset: 0x000F5FCF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000F7BEC File Offset: 0x000F5FEC
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000F7C7C File Offset: 0x000F607C
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000F7CBC File Offset: 0x000F60BC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement);
		}
	}
}
