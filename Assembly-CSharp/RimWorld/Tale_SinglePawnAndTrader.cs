using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000671 RID: 1649
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x06002288 RID: 8840 RVA: 0x0012515C File Offset: 0x0012355C
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00125165 File Offset: 0x00123565
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0012517C File Offset: 0x0012357C
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x001251B3 File Offset: 0x001235B3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x001251D4 File Offset: 0x001235D4
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

		// Token: 0x0600228D RID: 8845 RVA: 0x001251FE File Offset: 0x001235FE
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x0400138A RID: 5002
		public TaleData_Trader traderData;
	}
}
