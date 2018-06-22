using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077A RID: 1914
	public static class TradeSession
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002A45 RID: 10821 RVA: 0x0016695C File Offset: 0x00164D5C
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x0016697C File Offset: 0x00164D7C
		public static void SetupWith(ITrader newTrader, Pawn newPlayerNegotiator, bool giftMode)
		{
			if (!newTrader.CanTradeNow)
			{
				Log.Warning("Called SetupWith with a trader not willing to trade now.", false);
			}
			TradeSession.trader = newTrader;
			TradeSession.playerNegotiator = newPlayerNegotiator;
			TradeSession.giftMode = giftMode;
			TradeSession.deal = new TradeDeal();
			if (!giftMode && TradeSession.deal.cannotSellReasons.Count > 0)
			{
				Messages.Message("MessageCannotSellItemsReason".Translate() + TradeSession.deal.cannotSellReasons.ToCommaList(true), MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x00166A01 File Offset: 0x00164E01
		public static void Close()
		{
			TradeSession.trader = null;
		}

		// Token: 0x040016CB RID: 5835
		public static ITrader trader;

		// Token: 0x040016CC RID: 5836
		public static Pawn playerNegotiator;

		// Token: 0x040016CD RID: 5837
		public static TradeDeal deal;

		// Token: 0x040016CE RID: 5838
		public static bool giftMode;
	}
}
