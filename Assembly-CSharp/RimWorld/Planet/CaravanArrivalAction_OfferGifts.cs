using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CB RID: 1483
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x06001CD2 RID: 7378 RVA: 0x000F770F File Offset: 0x000F5B0F
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000F7718 File Offset: 0x000F5B18
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x000F7728 File Offset: 0x000F5B28
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x000F7748 File Offset: 0x000F5B48
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

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000F777C File Offset: 0x000F5B7C
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

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000F77E0 File Offset: 0x000F5BE0
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000F7817 File Offset: 0x000F5C17
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000F7834 File Offset: 0x000F5C34
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000F78C4 File Offset: 0x000F5CC4
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000F7904 File Offset: 0x000F5D04
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement);
		}

		// Token: 0x04001152 RID: 4434
		private Settlement settlement;
	}
}
