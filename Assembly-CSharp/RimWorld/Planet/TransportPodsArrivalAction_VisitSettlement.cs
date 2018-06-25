using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000616 RID: 1558
	public class TransportPodsArrivalAction_VisitSettlement : TransportPodsArrivalAction_FormCaravan
	{
		// Token: 0x0400124B RID: 4683
		private Settlement settlement;

		// Token: 0x06001F64 RID: 8036 RVA: 0x001104AA File Offset: 0x0010E8AA
		public TransportPodsArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x001104B3 File Offset: 0x0010E8B3
		public TransportPodsArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x001104C3 File Offset: 0x0010E8C3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x001104E0 File Offset: 0x0010E8E0
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

		// Token: 0x06001F68 RID: 8040 RVA: 0x00110544 File Offset: 0x0010E944
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

		// Token: 0x06001F69 RID: 8041 RVA: 0x001105A4 File Offset: 0x0010E9A4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSettlement>(() => TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportPodsArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile);
		}
	}
}
