using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000618 RID: 1560
	public class TransportPodsArrivalAction_VisitSettlement : TransportPodsArrivalAction_FormCaravan
	{
		// Token: 0x06001F67 RID: 8039 RVA: 0x0011028E File Offset: 0x0010E68E
		public TransportPodsArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x00110297 File Offset: 0x0010E697
		public TransportPodsArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x001102A7 File Offset: 0x0010E6A7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x001102C4 File Offset: 0x0010E6C4
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

		// Token: 0x06001F6B RID: 8043 RVA: 0x00110328 File Offset: 0x0010E728
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

		// Token: 0x06001F6C RID: 8044 RVA: 0x00110388 File Offset: 0x0010E788
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_VisitSettlement>(() => TransportPodsArrivalAction_VisitSettlement.CanVisit(pods, settlement), () => new TransportPodsArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(new object[]
			{
				settlement.Label
			}), representative, settlement.Tile);
		}

		// Token: 0x0400124E RID: 4686
		private Settlement settlement;
	}
}
