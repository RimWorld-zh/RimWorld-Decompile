using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000668 RID: 1640
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x06002262 RID: 8802 RVA: 0x001243A0 File Offset: 0x001227A0
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x001243A9 File Offset: 0x001227A9
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x001243C0 File Offset: 0x001227C0
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x001243F7 File Offset: 0x001227F7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x00124418 File Offset: 0x00122818
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

		// Token: 0x06002267 RID: 8807 RVA: 0x00124442 File Offset: 0x00122842
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04001384 RID: 4996
		public TaleData_Trader traderData;
	}
}
