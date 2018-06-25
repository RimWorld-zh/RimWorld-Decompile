using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066A RID: 1642
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x04001388 RID: 5000
		public TaleData_Trader traderData;

		// Token: 0x06002265 RID: 8805 RVA: 0x00124758 File Offset: 0x00122B58
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x00124761 File Offset: 0x00122B61
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00124778 File Offset: 0x00122B78
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x001247AF File Offset: 0x00122BAF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x001247D0 File Offset: 0x00122BD0
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule r in this.<SpecialTextGenerationRules>__BaseCallProxy0())
			{
				yield return r;
			}
			foreach (Rule r2 in this.traderData.GetRules("TRADER"))
			{
				yield return r2;
			}
			yield break;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x001247FA File Offset: 0x00122BFA
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}
	}
}
