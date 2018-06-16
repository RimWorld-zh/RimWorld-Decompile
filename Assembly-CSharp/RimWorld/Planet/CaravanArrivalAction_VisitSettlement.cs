using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D2 RID: 1490
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x06001CF6 RID: 7414 RVA: 0x000F7E7F File Offset: 0x000F627F
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000F7E88 File Offset: 0x000F6288
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x000F7E98 File Offset: 0x000F6298
		public override string Label
		{
			get
			{
				return "VisitSettlement".Translate(new object[]
				{
					this.settlement.Label
				});
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x000F7ECC File Offset: 0x000F62CC
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(new object[]
				{
					this.settlement.Label
				});
			}
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000F7F00 File Offset: 0x000F6300
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
				result = CaravanArrivalAction_VisitSettlement.CanVisit(caravan, this.settlement);
			}
			return result;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000F7F64 File Offset: 0x000F6364
		public override void Arrived(Caravan caravan)
		{
			if (caravan.IsPlayerControlled)
			{
				Messages.Message("MessageCaravanIsVisitingSettlement".Translate(new object[]
				{
					caravan.Label,
					this.settlement.Label
				}).CapitalizeFirst(), caravan, MessageTypeDefOf.TaskCompletion, true);
			}
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000F7FBA File Offset: 0x000F63BA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000F7FD4 File Offset: 0x000F63D4
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000F8008 File Offset: 0x000F6408
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), caravan, settlement.Tile, settlement);
		}

		// Token: 0x04001158 RID: 4440
		private Settlement settlement;
	}
}
