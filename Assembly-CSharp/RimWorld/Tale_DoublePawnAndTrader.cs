using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		public TaleData_Trader traderData;

		public Tale_DoublePawnAndTrader()
		{
		}

		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
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

		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule item in base.SpecialTextGenerationRules())
			{
				yield return item;
			}
			foreach (Rule rule in this.traderData.GetRules("trader"))
			{
				yield return rule;
			}
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}
	}
}
