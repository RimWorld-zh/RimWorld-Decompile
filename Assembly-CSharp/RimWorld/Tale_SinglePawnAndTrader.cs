using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000671 RID: 1649
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x06002286 RID: 8838 RVA: 0x001250E4 File Offset: 0x001234E4
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x001250ED File Offset: 0x001234ED
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00125104 File Offset: 0x00123504
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0012513B File Offset: 0x0012353B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0012515C File Offset: 0x0012355C
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

		// Token: 0x0600228B RID: 8843 RVA: 0x00125186 File Offset: 0x00123586
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x0400138A RID: 5002
		public TaleData_Trader traderData;
	}
}
