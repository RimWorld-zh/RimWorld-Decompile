using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000613 RID: 1555
	public class TransportPodsArrivalAction_GiveGift : TransportPodsArrivalAction
	{
		// Token: 0x0400124A RID: 4682
		private Settlement settlement;

		// Token: 0x06001F4E RID: 8014 RVA: 0x00110114 File Offset: 0x0010E514
		public TransportPodsArrivalAction_GiveGift()
		{
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0011011D File Offset: 0x0010E51D
		public TransportPodsArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0011012D File Offset: 0x0010E52D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x00110148 File Offset: 0x0010E548
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

		// Token: 0x06001F52 RID: 8018 RVA: 0x001101AC File Offset: 0x0010E5AC
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			FactionGiftUtility.GiveGift(pods, this.settlement);
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x001101BC File Offset: 0x0010E5BC
		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x00110224 File Offset: 0x0010E624
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveGift>(() => TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportPodsArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(new object[]
			{
				settlement.Faction.Name,
				FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()
			}), representative, settlement.Tile);
		}
	}
}
