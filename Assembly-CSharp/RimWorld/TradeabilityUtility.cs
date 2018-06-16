using System;

namespace RimWorld
{
	// Token: 0x02000781 RID: 1921
	public static class TradeabilityUtility
	{
		// Token: 0x06002A6B RID: 10859 RVA: 0x001672D0 File Offset: 0x001656D0
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x001672F4 File Offset: 0x001656F4
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
