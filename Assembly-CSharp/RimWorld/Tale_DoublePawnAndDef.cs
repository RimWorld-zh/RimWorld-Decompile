using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		public TaleData_Def defData;

		public Tale_DoublePawnAndDef()
		{
		}

		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule item in base.SpecialTextGenerationRules())
			{
				yield return item;
			}
			foreach (Rule rule in this.defData.GetRules(base.def.defSymbol))
			{
				yield return rule;
			}
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom(DefDatabase<TrainableDef>.GetRandom());
		}
	}
}
