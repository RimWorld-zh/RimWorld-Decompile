using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		public TaleData_Trader traderData;

		public Tale_SinglePawnAndTrader()
		{
		}

		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", new object[0]);
		}

		[DebuggerHidden]
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			Tale_SinglePawnAndTrader.<SpecialTextGenerationRules>c__Iterator137 <SpecialTextGenerationRules>c__Iterator = new Tale_SinglePawnAndTrader.<SpecialTextGenerationRules>c__Iterator137();
			<SpecialTextGenerationRules>c__Iterator.<>f__this = this;
			Tale_SinglePawnAndTrader.<SpecialTextGenerationRules>c__Iterator137 expr_0E = <SpecialTextGenerationRules>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}
	}
}
