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

		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
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
			foreach (Rule item in base.SpecialTextGenerationRules())
			{
				yield return item;
			}
			foreach (Rule rule in this.thingData.GetRules("thing"))
			{
				yield return rule;
			}
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}
	}
}
