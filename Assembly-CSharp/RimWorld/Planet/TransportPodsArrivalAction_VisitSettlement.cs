using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class TransportPodsArrivalAction_VisitSettlement : TransportPodsArrivalAction_FormCaravan
	{
		private Settlement settlement;

		public TransportPodsArrivalAction_VisitSettlement()
		{
		}

		public TransportPodsArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
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
				result = TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, this.settlement);
			}
			return result;
		}

		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			FloatMenuAcceptanceReport result;
			if (settlement == null || !settlement.Spawned || !settlement.Visitable)
			{
				result = false;
			}
			else if (!TransportPodsArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer))
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSettlement>(() => TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportPodsArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile);
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__AnonStorey0
		{
			internal IEnumerable<IThingHolder> pods;

			internal Settlement settlement;

			public <GetFloatMenuOptions>c__AnonStorey0()
			{
			}

			internal FloatMenuAcceptanceReport <>m__0()
			{
				return TransportPodsArrivalAction_VisitSettlement.CanVisit(this.pods, this.settlement);
			}

			internal TransportPodsArrivalAction_VisitSettlement <>m__1()
			{
				return new TransportPodsArrivalAction_VisitSettlement(this.settlement);
			}
		}
	}
}
