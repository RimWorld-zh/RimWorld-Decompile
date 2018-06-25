using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066F RID: 1647
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x04001388 RID: 5000
		public TaleData_Trader traderData;

		// Token: 0x06002284 RID: 8836 RVA: 0x001253E4 File Offset: 0x001237E4
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x001253ED File Offset: 0x001237ED
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00125404 File Offset: 0x00123804
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x0012543B File Offset: 0x0012383B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x0012545C File Offset: 0x0012385C
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

		// Token: 0x06002289 RID: 8841 RVA: 0x00125486 File Offset: 0x00123886
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}
	}
}
