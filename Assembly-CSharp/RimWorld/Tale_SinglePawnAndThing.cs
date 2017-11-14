using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		public TaleData_Thing thingData;

		public Tale_SinglePawnAndThing()
		{
		}

		public Tale_SinglePawnAndThing(Pawn pawn, Thing item)
			: base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", new object[0]);
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
			using (IEnumerator<Rule> enumerator2 = this.thingData.GetRules("thing").GetEnumerator())
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
			this.thingData = TaleData_Thing.GenerateRandom();
		}
	}
}
