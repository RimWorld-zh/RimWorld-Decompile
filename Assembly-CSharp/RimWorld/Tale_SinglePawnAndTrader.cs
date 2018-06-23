using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066D RID: 1645
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x04001388 RID: 5000
		public TaleData_Trader traderData;

		// Token: 0x06002280 RID: 8832 RVA: 0x00125294 File Offset: 0x00123694
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x0012529D File Offset: 0x0012369D
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x001252B4 File Offset: 0x001236B4
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x001252EB File Offset: 0x001236EB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x0012530C File Offset: 0x0012370C
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

		// Token: 0x06002285 RID: 8837 RVA: 0x00125336 File Offset: 0x00123736
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}
	}
}
