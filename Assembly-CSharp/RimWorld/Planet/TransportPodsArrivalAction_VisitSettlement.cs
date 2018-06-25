using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000616 RID: 1558
	public class TransportPodsArrivalAction_VisitSettlement : TransportPodsArrivalAction_FormCaravan
	{
		// Token: 0x0400124F RID: 4687
		private Settlement settlement;

		// Token: 0x06001F63 RID: 8035 RVA: 0x00110712 File Offset: 0x0010EB12
		public TransportPodsArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0011071B File Offset: 0x0010EB1B
		public TransportPodsArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0011072B File Offset: 0x0010EB2B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x00110748 File Offset: 0x0010EB48
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

		// Token: 0x06001F67 RID: 8039 RVA: 0x001107AC File Offset: 0x0010EBAC
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

		// Token: 0x06001F68 RID: 8040 RVA: 0x0011080C File Offset: 0x0010EC0C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSettlement>(() => TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportPodsArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile);
		}
	}
}
