using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CF RID: 1487
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x06001CD9 RID: 7385 RVA: 0x000F7643 File Offset: 0x000F5A43
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000F764C File Offset: 0x000F5A4C
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x000F765C File Offset: 0x000F5A5C
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x000F767C File Offset: 0x000F5A7C
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

		// Token: 0x06001CDD RID: 7389 RVA: 0x000F76B0 File Offset: 0x000F5AB0
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

		// Token: 0x06001CDE RID: 7390 RVA: 0x000F7714 File Offset: 0x000F5B14
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000F774B File Offset: 0x000F5B4B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000F7768 File Offset: 0x000F5B68
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000F77F8 File Offset: 0x000F5BF8
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000F7838 File Offset: 0x000F5C38
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement);
		}

		// Token: 0x04001155 RID: 4437
		private Settlement settlement;
	}
}
