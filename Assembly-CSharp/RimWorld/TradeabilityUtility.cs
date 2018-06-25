using System;

namespace RimWorld
{
	// Token: 0x0200077F RID: 1919
	public static class TradeabilityUtility
	{
		// Token: 0x06002A6A RID: 10858 RVA: 0x0016768C File Offset: 0x00165A8C
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x001676B0 File Offset: 0x00165AB0
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
