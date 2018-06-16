using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077E RID: 1918
	public static class TradeSession
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x001666F0 File Offset: 0x00164AF0
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x00166710 File Offset: 0x00164B10
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

		// Token: 0x06002A4C RID: 10828 RVA: 0x00166795 File Offset: 0x00164B95
		public static void Close()
		{
			TradeSession.trader = null;
		}

		// Token: 0x040016CD RID: 5837
		public static ITrader trader;

		// Token: 0x040016CE RID: 5838
		public static Pawn playerNegotiator;

		// Token: 0x040016CF RID: 5839
		public static TradeDeal deal;

		// Token: 0x040016D0 RID: 5840
		public static bool giftMode;
	}
}
