using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class ScenarioDef : Def
	{
		public Scenario scenario;

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = this.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = this.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			ScenarioDef.<ConfigErrors>c__Iterator93 <ConfigErrors>c__Iterator = new ScenarioDef.<ConfigErrors>c__Iterator93();
			<ConfigErrors>c__Iterator.<>f__this = this;
			ScenarioDef.<ConfigErrors>c__Iterator93 expr_0E = <ConfigErrors>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
