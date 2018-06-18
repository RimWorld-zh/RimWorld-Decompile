using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200066C RID: 1644
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x0600226A RID: 8810 RVA: 0x00124268 File Offset: 0x00122668
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00124271 File Offset: 0x00122671
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00124288 File Offset: 0x00122688
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x001242BF File Offset: 0x001226BF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x001242E0 File Offset: 0x001226E0
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

		// Token: 0x0600226F RID: 8815 RVA: 0x0012430A File Offset: 0x0012270A
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04001386 RID: 4998
		public TaleData_Trader traderData;
	}
}
