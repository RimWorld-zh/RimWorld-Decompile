using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066A RID: 1642
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x04001384 RID: 4996
		public TaleData_Trader traderData;

		// Token: 0x06002266 RID: 8806 RVA: 0x001244F0 File Offset: 0x001228F0
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x001244F9 File Offset: 0x001228F9
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00124510 File Offset: 0x00122910
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00124547 File Offset: 0x00122947
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00124568 File Offset: 0x00122968
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

		// Token: 0x0600226B RID: 8811 RVA: 0x00124592 File Offset: 0x00122992
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}
	}
}
