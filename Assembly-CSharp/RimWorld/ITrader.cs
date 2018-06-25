using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000766 RID: 1894
	public interface ITrader
	{
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060029D5 RID: 10709
		TraderKindDef TraderKind { get; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060029D6 RID: 10710
		IEnumerable<Thing> Goods { get; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060029D7 RID: 10711
		int RandomPriceFactorSeed { get; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060029D8 RID: 10712
		string TraderName { get; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060029D9 RID: 10713
		bool CanTradeNow { get; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060029DA RID: 10714
		float TradePriceImprovementOffsetForPlayer { get; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060029DB RID: 10715
		Faction Faction { get; }

		// Token: 0x060029DC RID: 10716
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		// Token: 0x060029DD RID: 10717
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		// Token: 0x060029DE RID: 10718
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
