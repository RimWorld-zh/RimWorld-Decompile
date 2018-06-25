using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D0 RID: 1488
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x04001155 RID: 4437
		private Settlement settlement;

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000F809B File Offset: 0x000F649B
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x000F80A4 File Offset: 0x000F64A4
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000F80B4 File Offset: 0x000F64B4
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
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x000F80E8 File Offset: 0x000F64E8
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

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000F811C File Offset: 0x000F651C
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

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000F8180 File Offset: 0x000F6580
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

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000F81D6 File Offset: 0x000F65D6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000F81F0 File Offset: 0x000F65F0
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000F8224 File Offset: 0x000F6624
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), caravan, settlement.Tile, settlement);
		}
	}
}
