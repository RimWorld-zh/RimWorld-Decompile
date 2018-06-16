using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000615 RID: 1557
	public class TransportPodsArrivalAction_GiveGift : TransportPodsArrivalAction
	{
		// Token: 0x06001F52 RID: 8018 RVA: 0x0010FC90 File Offset: 0x0010E090
		public TransportPodsArrivalAction_GiveGift()
		{
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0010FC99 File Offset: 0x0010E099
		public TransportPodsArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0010FCA9 File Offset: 0x0010E0A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0010FCC4 File Offset: 0x0010E0C4
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
				result = TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, this.settlement);
			}
			return result;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0010FD28 File Offset: 0x0010E128
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			FactionGiftUtility.GiveGift(pods, this.settlement);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0010FD38 File Offset: 0x0010E138
		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0010FDA0 File Offset: 0x0010E1A0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveGift>(() => TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportPodsArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(new object[]
			{
				settlement.Faction.Name,
				FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()
			}), representative, settlement.Tile);
		}

		// Token: 0x04001249 RID: 4681
		private Settlement settlement;
	}
}
