﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		private SettlementBase settlement;

		public CaravanArrivalAction_VisitSettlement()
		{
		}

		public CaravanArrivalAction_VisitSettlement(SettlementBase settlement)
		{
			this.settlement = settlement;
		}

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

		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitSettlement.CanVisit(caravan, this.settlement);
		}

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

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<SettlementBase>(ref this.settlement, "settlement", false);
		}

		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, SettlementBase settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, SettlementBase settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), caravan, settlement.Tile, settlement);
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__AnonStorey0
		{
			internal Caravan caravan;

			internal SettlementBase settlement;

			public <GetFloatMenuOptions>c__AnonStorey0()
			{
			}

			internal FloatMenuAcceptanceReport <>m__0()
			{
				return CaravanArrivalAction_VisitSettlement.CanVisit(this.caravan, this.settlement);
			}

			internal CaravanArrivalAction_VisitSettlement <>m__1()
			{
				return new CaravanArrivalAction_VisitSettlement(this.settlement);
			}
		}
	}
}
