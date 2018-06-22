using System;

namespace RimWorld
{
	// Token: 0x0200077D RID: 1917
	public static class TradeabilityUtility
	{
		// Token: 0x06002A66 RID: 10854 RVA: 0x0016753C File Offset: 0x0016593C
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x00167560 File Offset: 0x00165960
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
