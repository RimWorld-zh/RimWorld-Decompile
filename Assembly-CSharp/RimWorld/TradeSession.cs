using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077C RID: 1916
	public static class TradeSession
	{
		// Token: 0x040016CF RID: 5839
		public static ITrader trader;

		// Token: 0x040016D0 RID: 5840
		public static Pawn playerNegotiator;

		// Token: 0x040016D1 RID: 5841
		public static TradeDeal deal;

		// Token: 0x040016D2 RID: 5842
		public static bool giftMode;

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x00166D0C File Offset: 0x0016510C
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x00166D2C File Offset: 0x0016512C
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

		// Token: 0x06002A4A RID: 10826 RVA: 0x00166DB1 File Offset: 0x001651B1
		public static void Close()
		{
			TradeSession.trader = null;
		}
	}
}
