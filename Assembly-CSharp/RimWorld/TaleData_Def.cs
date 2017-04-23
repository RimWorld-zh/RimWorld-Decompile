using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleData_Def : TaleData
	{
		public Def def;

		private string tmpDefName;

		private Type tmpDefType;

		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpDefName = this.def.defName;
				this.tmpDefType = this.def.GetType();
			}
			Scribe_Values.Look<string>(ref this.tmpDefName, "defName", null, false);
			Scribe_Values.Look<Type>(ref this.tmpDefType, "defType", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.def = GenDefDatabase.GetDef(this.tmpDefType, this.tmpDefName, true);
			}
		}

		[DebuggerHidden]
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			TaleData_Def.<GetRules>c__Iterator12A <GetRules>c__Iterator12A = new TaleData_Def.<GetRules>c__Iterator12A();
			<GetRules>c__Iterator12A.prefix = prefix;
			<GetRules>c__Iterator12A.<$>prefix = prefix;
			<GetRules>c__Iterator12A.<>f__this = this;
			TaleData_Def.<GetRules>c__Iterator12A expr_1C = <GetRules>c__Iterator12A;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}
	}
}
