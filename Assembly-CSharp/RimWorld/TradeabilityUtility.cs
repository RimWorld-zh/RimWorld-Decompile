using System;

namespace RimWorld
{
	// Token: 0x0200077F RID: 1919
	public static class TradeabilityUtility
	{
		// Token: 0x06002A69 RID: 10857 RVA: 0x001678EC File Offset: 0x00165CEC
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x00167910 File Offset: 0x00165D10
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
