using System;

namespace RimWorld
{
	// Token: 0x02000781 RID: 1921
	public static class TradeabilityUtility
	{
		// Token: 0x06002A6D RID: 10861 RVA: 0x00167364 File Offset: 0x00165764
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00167388 File Offset: 0x00165788
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
