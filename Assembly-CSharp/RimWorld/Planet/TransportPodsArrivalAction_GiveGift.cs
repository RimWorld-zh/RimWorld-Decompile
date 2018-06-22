using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000611 RID: 1553
	public class TransportPodsArrivalAction_GiveGift : TransportPodsArrivalAction
	{
		// Token: 0x06001F4B RID: 8011 RVA: 0x0010FD5C File Offset: 0x0010E15C
		public TransportPodsArrivalAction_GiveGift()
		{
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0010FD65 File Offset: 0x0010E165
		public TransportPodsArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0010FD75 File Offset: 0x0010E175
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0010FD90 File Offset: 0x0010E190
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

		// Token: 0x06001F4F RID: 8015 RVA: 0x0010FDF4 File Offset: 0x0010E1F4
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			FactionGiftUtility.GiveGift(pods, this.settlement);
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0010FE04 File Offset: 0x0010E204
		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0010FE6C File Offset: 0x0010E26C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveGift>(() => TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportPodsArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(new object[]
			{
				settlement.Faction.Name,
				FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()
			}), representative, settlement.Tile);
		}

		// Token: 0x04001246 RID: 4678
		private Settlement settlement;
	}
}
