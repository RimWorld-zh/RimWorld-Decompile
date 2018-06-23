using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CE RID: 1486
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x04001155 RID: 4437
		private Settlement settlement;

		// Token: 0x06001CEF RID: 7407 RVA: 0x000F7F4B File Offset: 0x000F634B
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000F7F54 File Offset: 0x000F6354
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x000F7F64 File Offset: 0x000F6364
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
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x000F7F98 File Offset: 0x000F6398
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

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000F7FCC File Offset: 0x000F63CC
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

		// Token: 0x06001CF4 RID: 7412 RVA: 0x000F8030 File Offset: 0x000F6430
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

		// Token: 0x06001CF5 RID: 7413 RVA: 0x000F8086 File Offset: 0x000F6486
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000F80A0 File Offset: 0x000F64A0
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000F80D4 File Offset: 0x000F64D4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), caravan, settlement.Tile, settlement);
		}
	}
}
