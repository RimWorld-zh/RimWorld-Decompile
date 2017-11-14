using System.Collections.Generic;
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

		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader)
			: base(pawn)
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
			using (IEnumerator<Rule> enumerator = base.SpecialTextGenerationRules().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Rule r2 = enumerator.Current;
					yield return r2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<Rule> enumerator2 = this.traderData.GetRules("trader").GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Rule r = enumerator2.Current;
					yield return r;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0154:
			/*Error near IL_0155: Unexpected return in MoveNext()*/;
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}
	}
}
