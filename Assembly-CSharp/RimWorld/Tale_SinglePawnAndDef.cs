using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		public TaleData_Def defData;

		public Tale_SinglePawnAndDef()
		{
		}

		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", new object[0]);
		}

		[DebuggerHidden]
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			Tale_SinglePawnAndDef.<SpecialTextGenerationRules>c__Iterator134 <SpecialTextGenerationRules>c__Iterator = new Tale_SinglePawnAndDef.<SpecialTextGenerationRules>c__Iterator134();
			<SpecialTextGenerationRules>c__Iterator.<>f__this = this;
			Tale_SinglePawnAndDef.<SpecialTextGenerationRules>c__Iterator134 expr_0E = <SpecialTextGenerationRules>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom(DefDatabase<ResearchProjectDef>.GetRandom());
		}
	}
}
