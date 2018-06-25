using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class TransportPodsArrivalAction_GiveGift : TransportPodsArrivalAction
	{
		private Settlement settlement;

		public TransportPodsArrivalAction_GiveGift()
		{
		}

		public TransportPodsArrivalAction_GiveGift(Settlement settlement)
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
				result = TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, this.settlement);
			}
			return result;
		}

		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			FactionGiftUtility.GiveGift(pods, this.settlement);
		}

		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			return TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_GiveGift>(() => TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportPodsArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(new object[]
			{
				settlement.Faction.Name,
				FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()
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
				return TransportPodsArrivalAction_GiveGift.CanGiveGiftTo(this.pods, this.settlement);
			}

			internal TransportPodsArrivalAction_GiveGift <>m__1()
			{
				return new TransportPodsArrivalAction_GiveGift(this.settlement);
			}
		}
	}
}
