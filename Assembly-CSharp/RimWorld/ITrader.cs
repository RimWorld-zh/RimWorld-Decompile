using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000768 RID: 1896
	public interface ITrader
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060029D9 RID: 10713
		TraderKindDef TraderKind { get; }

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060029DA RID: 10714
		IEnumerable<Thing> Goods { get; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060029DB RID: 10715
		int RandomPriceFactorSeed { get; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060029DC RID: 10716
		string TraderName { get; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060029DD RID: 10717
		bool CanTradeNow { get; }

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060029DE RID: 10718
		float TradePriceImprovementOffsetForPlayer { get; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060029DF RID: 10719
		Faction Faction { get; }

		// Token: 0x060029E0 RID: 10720
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		// Token: 0x060029E1 RID: 10721
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		// Token: 0x060029E2 RID: 10722
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
